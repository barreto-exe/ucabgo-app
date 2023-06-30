using Android.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Api.Services.Driver;
using UcabGo.App.Models;
using UcabGo.App.Services;

namespace UcabGo.App.ViewModel
{
    public partial class ActiveRiderViewModel : ViewModelBase
    {
        readonly IDriverApi driverApi;

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

        public ActiveRiderViewModel(ISettingsService settingsService, INavigationService navigation, IDriverApi driverApi) : base(settingsService, navigation)
        {
            this.driverApi = driverApi;

            ride = new();
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
            IsLoading = true;
            IsDataVisible = false;

            var rides = await driverApi.GetRides(onlyAvailable: true);
            if(rides?.Message == "RIDES_FOUND")
            {
                var activeRide = rides.Data.FirstOrDefault(x => x.IsAvailable);
                if(activeRide != null)
                {
                    Ride = activeRide;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No hay cola activa.", "Aceptar");
                    await navigation.GoBackAsync();
                }
            }

            var passengers = await driverApi.GetPassengers(Ride.Id);
            if(passengers?.Message == "PASSENGERS_FOUND")
            {
                passengers.Data = passengers.Data
                    .Where(x => x.IsShowed)
                    .OrderBy(x => x.TimeAccepted)
                    .ThenBy(x => x.TimeSolicited)
                    .ToList();
                Passengers = new(passengers.Data);
            }

            int seats = Ride?.AvailableSeats ?? 0;
            string seatsText = seats switch
            {
                0 => "No quedan asientos disponibles.",
                1 => "1 asiento disponible.",
                _ => $"{seats} asientos disponibles.",
            };

            string acceptText;
            if (!Ride.IsStarted)
            {
                acceptText = "Comenzar viaje";
            }
            else
            {
                acceptText = "Finalizar viaje";
            }

            SeatsText = seatsText;
            ButtonText = acceptText;
            IsCancelButtonEnabled = !Ride.IsStarted;
            IsLoading = false;
            IsDataVisible = true;
            IsPassengersVisible = Passengers.Any();
            IsPassengersEmpty = !Passengers.Any();
        }

        [RelayCommand]
        async Task CancelRide()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Está seguro que desea cancelar el viaje?", "Aceptar", "Cancelar");
            if(!confirm)
            {
                return;
            }

            var response = await driverApi.CancelRide(Ride.Id);
            if(response?.Message == "RIDE_CANCELED")
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "El viaje ha sido cancelado.", "Aceptar");
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
            if(!Ride.IsStarted)
            {
                await StartRide();
            }
            else
            {
                await CompleteRide();
            }
        }

        async Task StartRide()
        {
            if(!Ride.Passengers.Any())
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay pasajeros en el viaje.", "Aceptar");
                return;
            }

            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Está seguro que desea empezar el viaje?", "Aceptar", "Cancelar");
            if (!confirm)
            {
                return;
            }

            var response = await driverApi.StartRide(Ride.Id);
            if (response?.Message == "RIDE_STARTED")
            {
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
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Está seguro que desea finalizar el viaje?", "Aceptar", "Cancelar");
            if (!confirm)
            {
                return;
            }
            var response = await driverApi.CompleteRide(Ride.Id);
            if (response?.Message == "RIDE_COMPLETED")
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "El viaje ha sido completado.", "Aceptar");
                await navigation.GoBackAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo completar el viaje.", "Aceptar");
            }
        }
    }
}
