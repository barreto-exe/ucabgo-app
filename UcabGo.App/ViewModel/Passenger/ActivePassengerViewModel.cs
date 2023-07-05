using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.PassengerService;
using UcabGo.App.Models;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class ActivePassengerViewModel : ViewModelBase
    {
        readonly IPassengerApi passengerApi;

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

        CancellationTokenSource cancellationToken { get; set; }

        public ActivePassengerViewModel(ISettingsService settingsService, INavigationService navigation, IPassengerApi passengerApi) : base(settingsService, navigation)
        {
            this.passengerApi = passengerApi;

            ride = new();
            passenger = new();
            passengers = new();
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await Refresh();
        }

        [RelayCommand]
        async Task Refresh()
        {
            cancellationToken = new();

            IsLoading = true;
            IsWaiting = false;
            IsAccepted = false;
            TimerText = string.Empty;

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
                await RunTimer();
            }
            else
            {
                cancellationToken.Cancel();
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
                var doubleCheck = await DoubleCheckRide();

                switch (doubleCheck)
                {
                    case DoubleCheckResult.ArrivedOk:
                        {
                            //TODO - Rate driver screen
                            await navigation.GoBackAsync();
                            return false;
                        }
                    case DoubleCheckResult.Complaint:
                        {
                            //TODO - Complaint screen
                            await navigation.GoBackAsync();
                            return false;
                        }
                    case DoubleCheckResult.ApiError:
                        {
                            break;
                        }
                }
            }
            else
            {
                //If user is in this screen previosly, it means that the ride was canceled
                await Application.Current.MainPage.DisplayAlert("Atención", "El viaje ha sido cancelado por el conductor.", "Aceptar.");

                await navigation.GoBackAsync();

                return false;
            }

            Passenger = Ride.Passengers.Where(x => x.User.Id == settings.User.Id).FirstOrDefault(x => !x.IsEnded);
            Passengers = new(Ride.Passengers.Where(x => x.IsActive).DistinctBy(x => x.User.Id));

            if (Ride?.Destination?.Alias.ToLower().Contains("ucab") == true)
            {
                DestinationText = "UCAB Guayana"; 
            }
            else
            {
                DestinationText = Ride?.Destination?.Zone;
            }

            return true;
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
                    //TODO - Rate driver screen
                    await navigation.GoBackAsync();

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
                if(cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var timePassed = DateTime.Now.ToUniversalTime() - Passenger.TimeSolicited;

                var minutesLeft = TimeSpan.FromMinutes(15) - timePassed;

#if DEBUG
                minutesLeft = TimeSpan.FromSeconds(10);
#endif

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
