using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
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

        public ActiveRiderViewModel(ISettingsService settingsService, INavigationService navigation, IDriverApi driverApi) : base(settingsService, navigation)
        {
            this.driverApi = driverApi;

            ride = new();
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

            IsLoading = false;
            IsDataVisible = true;
        }

        [RelayCommand]
        async Task CancelRide()
        {
            //Are you sure?
            var accept = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Está seguro que desea cancelar el viaje?", "Aceptar", "Cancelar");
            if(!accept)
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
    }
}
