using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UcabGo.App.Api.Services.Driver;
using UcabGo.App.Api.Services.SignalR;
using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class ActiveRiderViewModel : ViewModelBase
    {
        readonly IDriverApi driverApi;

        readonly HubConnection hubConnection;
        CancellationTokenSource tokenSource;

        [ObservableProperty]
        Ride ride;

        [ObservableProperty]
        bool isLoading;

        [ObservableProperty]
        bool isDataVisible;

        [ObservableProperty]
        string seatsText;

        [ObservableProperty]
        string buttonText;

        [ObservableProperty]
        bool isRideStarted;

        [ObservableProperty]
        bool isCancelButtonEnabled;

        [ObservableProperty]
        ObservableCollection<Passenger> passengers;

        [ObservableProperty]
        bool isPassengersVisible;

        [ObservableProperty]
        bool isPassengersEmpty;

        public ActiveRiderViewModel(ISettingsService settingsService, INavigationService navigation, IDriverApi driverApi, IHubConnectionFactory hubConnectionFactory) : base(settingsService, navigation)
        {
            this.driverApi = driverApi;

            ride = new();
            passengers = new();

            hubConnection = hubConnectionFactory.GetHubConnection(ApiRoutes.ACTIVE_RIDE_HUB);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await Refresh(true);

            await RunHubConnection();
        }

        private async Task RunHubConnection()
        {
            hubConnection.On<int>(ApiRoutes.ACTIVE_RIDE_RECEIVE_UPDATE, async (rideId) =>
            {
                if (rideId == Ride.Id)
                {
                    await Refresh(false);
                }
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

            tokenSource.Cancel();
            await hubConnection.StopAsync();
        }

        async Task Refresh(bool withAnimation)
        {
            bool closeView = false;
            IsLoading = true && withAnimation;
            if(withAnimation) IsDataVisible = false;

            var rides = await driverApi.GetRides(onlyAvailable: true);
            if (rides?.Message == "RIDES_FOUND")
            {
                var activeRide = rides.Data.FirstOrDefault();
                if (activeRide != null)
                {
                    Ride = activeRide;
                }
                else
                {
                    closeView = true;
                }
            }
            else
            {
                closeView = true;
            }

            if (closeView)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay cola activa.", "Aceptar");
                await navigation.GoBackAsync();
            }

            await UpdatePassengers();

            string acceptText;
            if (!Ride.IsStarted)
            {
                acceptText = "Comenzar viaje";
            }
            else
            {
                acceptText = "Finalizar viaje";
            }

            ButtonText = acceptText;
            IsCancelButtonEnabled = !Ride.IsStarted;
            IsLoading = false;
            IsDataVisible = true;
        }

        private async Task UpdatePassengers()
        {
            var passengers = await driverApi.GetPassengers(Ride.Id);
            if (passengers?.Message == "PASSENGERS_FOUND")
            {
                passengers.Data = passengers.Data
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.TimeAccepted)
                    .ThenBy(x => x.TimeSolicited)
                    .ToList();
                Passengers = new(passengers.Data);
            }

            IsPassengersVisible = Passengers.Any();
            IsPassengersEmpty = !Passengers.Any();

            Ride.AvailableSeats = Ride.SeatQuantity - Passengers.Where(x => x.IsAccepted && x.IsActive).Count();
            int seats = Ride.AvailableSeats > 0 ? Ride.AvailableSeats : 0;
            SeatsText = seats switch
            {
                0 => "No quedan asientos disponibles.",
                1 => "1 asiento disponible.",
                _ => $"{seats} asientos disponibles.",
            };
        }

        [RelayCommand]
        async Task CancelRide()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Desea cancelar el viaje?", "Aceptar", "Cancelar");
            if (!confirm)
            {
                return;
            }

            var response = await driverApi.CancelRide(Ride.Id);
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
        async Task StartCompleteRide()
        {
            if (!Ride.IsStarted)
            {
                await StartRide();
            }
            else
            {
                await CompleteRide();
            }
        }

        [RelayCommand]
        async Task AcceptPassenger(Passenger passenger)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Desea aceptar al pasajero?", "Aceptar", "Cancelar");
            if (!confirm)
            {
                return;
            }

            string message;
            var response = await driverApi.AcceptPassenger(Ride.Id, passenger.Id);
            if (response?.Message == "PASSENGER_ACCEPTED")
            {
                message = string.Empty;
            }
            else if (response?.Message == "PASSENGER_NOT_FOUND")
            {
                message = "El pasajero no se encuentra en la cola.";
            }
            else if (response?.Message == "REQUEST_NOT_AVAILABLE_OR_ACCEPTED")
            {
                message = "El pasajero ya ha sido aceptado.";
            }
            else
            {
                message = "No se pudo aceptar al pasajero.";
            }
                
            if (!string.IsNullOrEmpty(message))
            {
                await Application.Current.MainPage.DisplayAlert("Error", message, "Aceptar");
            }
            await UpdatePassengers();
        }

        [RelayCommand]
        async Task CancelPassenger(Passenger passenger)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Desea rechazar al pasajero?", "Aceptar", "Cancelar");
            if (!confirm)
            {
                return;
            }

            if(passenger.IsWaiting)
            {
                await IgnorePassengerApi(passenger);
            }
            else if(passenger.IsAccepted)
            {
                await CancelPassengerApi(passenger);
            }
        }
        private async Task CancelPassengerApi(Passenger passenger)
        {
            string message;
            var response = await driverApi.CancelPassenger(Ride.Id, passenger.Id);
            if (response?.Message == "PASSENGER_CANCELLED")
            {
                message = string.Empty;
            }
            else if (response?.Message == "REQUEST_ALREADY_CANCELED_OR_NOT_ACCEPTED")
            {
                message = "El pasajero ya ha sido cancelado.";
            }
            else
            {
                message = "No se pudo cancelar al pasajero.";
            }
            
            if (!string.IsNullOrEmpty(message))
            {
                await Application.Current.MainPage.DisplayAlert("Error", message, "Aceptar");
            }
            await UpdatePassengers();
        }
        private async Task IgnorePassengerApi(Passenger passenger)
        {
            string message;
            var response = await driverApi.IgnorePassenger(Ride.Id, passenger.Id);
            if (response?.Message == "PASSENGER_IGNORED")
            {
                message = string.Empty;
            }
            else if (response?.Message == "REQUEST_NOT_AVAILABLE_OR_ACCEPTED")
            {
                message = "El pasajero ya ha sido aceptado.";
            }
            else
            {
                message = "No se pudo ignorar al pasajero.";
            }

            if (!string.IsNullOrEmpty(message))
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", message, "Aceptar");
            }
            await UpdatePassengers();
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

        async Task StartRide()
        {
            if (!Passengers.Any(x => x.IsAccepted && x.IsActive))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay pasajeros en el viaje.", "Aceptar");
                return;
            }

            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Desea empezar el viaje?", "Aceptar", "Cancelar");
            if (!confirm)
            {
                return;
            }

            var response = await driverApi.StartRide(Ride.Id);
            if (response?.Message == "RIDE_STARTED")
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "El viaje ha comenzado.", "Aceptar");

                Ride = response.Data;
                IsRideStarted = true;
                IsCancelButtonEnabled = false;
                ButtonText = "Finalizar viaje";
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo iniciar el viaje.", "Aceptar");
            }
        }
        async Task CompleteRide()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Desea finalizar el viaje?", "Aceptar", "Cancelar");
            if (!confirm)
            {
                return;
            }
            var response = await driverApi.CompleteRide(Ride.Id);
            if (response?.Message == "RIDE_COMPLETED")
            {
                //TODO - Go to rating page
                await navigation.GoBackAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo completar el viaje.", "Aceptar");
            }
        }
    }
}
