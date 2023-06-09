﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UcabGo.App.Api.Models;
using UcabGo.App.Api.Services.PassengerService;
using UcabGo.App.Api.Services.SignalR;
using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class ActivePassengerViewModel : ViewModelBase
    {
        readonly IPassengerApi passengerApi;

        readonly IHubConnectionFactory hubConnectionFactory;
        HubConnection hubConnection;
        CancellationTokenSource tokenSource;

        [ObservableProperty]
        Ride ride;

        [ObservableProperty]
        Passenger passenger;

        [ObservableProperty]
        bool isLoading;

        [ObservableProperty]
        bool isWaiting;

        [ObservableProperty]
        bool isAccepted;

        [ObservableProperty]
        bool isCancelButtonEnabled;

        [ObservableProperty]
        bool isAcceptButtonEnabled;

        [ObservableProperty]
        ObservableCollection<Passenger> passengers;

        [ObservableProperty]
        string destinationText;

        [ObservableProperty]
        string timerText;

        CancellationTokenSource timerCancellationToken;

        public ActivePassengerViewModel(ISettingsService settingsService, INavigationService navigation, IPassengerApi passengerApi, IHubConnectionFactory hubConnectionFactory) : base(settingsService, navigation)
        {
            this.passengerApi = passengerApi;

            ride = new();
            passenger = new();
            passengers = new();

            this.hubConnectionFactory = hubConnectionFactory;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await Refresh(true);

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await RunHubConnection();
            });
        }

        private async Task RunHubConnection()
        {
            var requestsInProcess = new List<string>();

            hubConnection = hubConnectionFactory.GetHubConnection(ApiRoutes.ACTIVE_RIDE_HUB);
            hubConnection.On<int, string>(ApiRoutes.ACTIVE_RIDE_RECEIVE_UPDATE, async (rideId, sender) =>
            {
                if (rideId == Ride.Id && !requestsInProcess.Contains(sender))
                {
                    requestsInProcess.Add(sender);

                    await Refresh(false);
                }

                requestsInProcess.Remove(sender);
            });

            tokenSource = new();
            try
            {
                if (hubConnection.State == HubConnectionState.Disconnected)
                {
                    await hubConnection.StartAsync(tokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(hubConnection.State.ToString() + ex.Message);
                await RunHubConnection();
            }
        }

        public override async void OnDisappearing()
        {
            base.OnDisappearing();

            try
            {
                tokenSource.Cancel();
                await hubConnection.StopAsync();
            }
            catch { }
        }

        async Task Refresh(bool withAnimation)
        {
            timerCancellationToken = new();

            IsLoading = true && withAnimation;
            TimerText = string.Empty;
            if(withAnimation)
            {
                IsWaiting = false;
                IsAccepted = false;
            }

            bool isInfoLoaded = await LoadRideInformation();
            if (!isInfoLoaded)
            {
                return;
            }

            IsWaiting = Passenger.IsWaiting;
            IsAccepted = Passenger.IsAccepted;
            IsCancelButtonEnabled = !Ride.IsStarted;
            IsAcceptButtonEnabled = Ride.IsStarted;
            IsLoading = false;

            if (IsWaiting)
            {
                //Run timer inside dispatcher to avoid cross thread exception
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await RunTimer();
                });
            }
            else
            {
                timerCancellationToken.Cancel();
            }
        }

        private async Task<bool> LoadRideInformation()
        {
            var activeRideTask = passengerApi.GetRides(onlyAvailable: true);
            var endedRideTask = passengerApi.GetRides(onlyAvailable: false);

            await Task.WhenAll(activeRideTask, endedRideTask);

            var activeRide = await activeRideTask;
            var endedRide = await endedRideTask;

            if (activeRide?.Message == "RIDES_FOUND" && activeRide.Data.Any())
            {
                Ride = activeRide.Data.First();
            }
            else if (endedRide?.Message == "RIDES_FOUND" && endedRide.Data.Any(RideNeedsDoubleCheck))
            {
                Ride = endedRide.Data.First(RideNeedsDoubleCheck);

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var doubleCheck = await DoubleCheckRide();
                    switch (doubleCheck)
                    {
                        case DoubleCheckResult.ArrivedOk:
                            {
                                await GoToRatingView();
                                break;
                            }
                        case DoubleCheckResult.Complaint:
                            {
                                //TODO - Complaint screen
                                await navigation.NavigateToAsync("//" + nameof(RoleSelectionView));
                                break;
                            }
                        case DoubleCheckResult.ApiError:
                            {
                                break;
                            }
                    }
                });
                 

                return false;
            }
            else
            {
                //If user is in this screen previosly, it means that the ride was canceled
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Atención", "El viaje ha sido cancelado por el conductor.", "Aceptar.");
                    await navigation.GoBackAsync();
                });


                return false;
            }

            Passenger = Ride.Passengers.Where(x => x.User.Id == settings.User.Id).FirstOrDefault(x => !x.IsEnded);
            Passengers = new(Ride.Passengers.Where(x => x.IsAccepted && x.IsActive).DistinctBy(x => x.User.Id));
            
            DestinationText = Ride?.Destination?.DestinationText;

            return true;
        }

        private async Task GoToRatingView()
        {
            string userJson = JsonConvert.SerializeObject(new List<User>() { Ride.Driver });

            var parameters = new Dictionary<string, object>
                                {
                                    { "usersJson", userJson },
                                    { "rideId", Ride.Id },
                                    { "isDriver", false }
                                };

            await navigation.NavigateToAsync<RateUserView>(parameters);
        }

        private async Task<DoubleCheckResult> DoubleCheckRide()
        {
            var completeResponse = await passengerApi.FinishRide(Ride.Id);
            if (completeResponse?.Message == "RIDE_FINISHED")
            {
                bool arrivedOk = await Application.Current.MainPage.DisplayAlert("Completar viaje", "El viaje ha sido marcado como completado por el conductor. ¿Has llegado a tu destino a través de esta cola?", "Sí", "No");

                if (arrivedOk)
                {
                    return DoubleCheckResult.ArrivedOk;
                }
                else
                {
                    return DoubleCheckResult.Complaint;
                }
            }
            else
            {
                return DoubleCheckResult.ApiError;
            }
        }

        private bool RideNeedsDoubleCheck(Ride ride)
        {
            return ride.TimeEnded != null && 
                   ride.Passengers.Any(x => x.User.Id == settings.User.Id && x.IsAccepted && x.IsActive);
        }

        [RelayCommand]
        async Task CancelRide()
        {
            bool cancel = await Application.Current.MainPage.DisplayAlert("Cancelar viaje", "¿Estás seguro que deseas cancelar el viaje?", "Sí", "No");
            if (cancel)
            {
                IsWaiting = false;
                IsAccepted = false;
                IsLoading = true;

                await CancelRideApi();

                IsWaiting = true;
                IsAccepted = true;
                IsLoading = false;
            }
        }
        private async Task CancelRideApi()
        {
            var response = await passengerApi.CancelRide(Ride.Id);
            if (response?.Message == "RIDE_CANCELED")
            {
                await navigation.GoBackAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cancelar el viaje.", "Aceptar");
            }
        }

        [RelayCommand]
        async Task CompleteRide()
        {
            bool complete = await Application.Current.MainPage.DisplayAlert("Completar viaje", "Indica como completado este viaje sólo si ya has llegado a tu destino.", "Sí", "No");
            if (complete)
            {
                IsLoading = true;

                var response = await passengerApi.FinishRide(Ride.Id);
                if (response?.Message == "RIDE_FINISHED")
                {
                    await GoToRatingView();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo completar el viaje.", "Aceptar");
                }

                IsLoading = false;
            }
        }

        async Task RunTimer()
        {
            while (true)
            {
                if(timerCancellationToken.IsCancellationRequested)
                {
                    break;
                }

#if DEBUG
                var timePassed = DateTime.Now.ToUniversalTime() - Passenger.TimeSolicited.ToUniversalTime();
#elif RELEASE
                var timePassed = DateTime.Now.ToUniversalTime() - Passenger.TimeSolicited;
#endif

                var minutesLeft = TimeSpan.FromMinutes(15) - timePassed;


                if (minutesLeft.TotalSeconds <= 0)
                {
                    await CancelRideApi();
                    break;
                }
                else
                {
                    TimerText = $"{minutesLeft.Minutes:00}:{minutesLeft.Seconds:00}";
                }

                await Task.Delay(1000);
            }
        }

        [RelayCommand]
        async Task CallSosContacts()
        {
            var contact = await Application.Current.MainPage.DisplayActionSheet("Contactar a", "Cancelar", null, settings.SosContacts.Select(x => x.Name).ToArray());
            if (!string.IsNullOrEmpty(contact) && !contact.Equals("Cancelar"))
            {
                if (PhoneDialer.Default.IsSupported)
                {
                    var contactSelected = settings.SosContacts.FirstOrDefault(x => x.Name.Equals(contact));
                    PhoneDialer.Default.Open(contactSelected.Phone);
                }
            }
        }

        [RelayCommand]
        async Task Chat()
        {
            var parameters = new Dictionary<string, object>()
            {
                { "rideId", Ride.Id },
            };

            await navigation.NavigateToAsync<ChatView>(parameters);
        }

        public enum DoubleCheckResult
        {
            ArrivedOk,
            Complaint,
            ApiError,
        }
    }
}
