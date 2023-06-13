using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.Vehicles;
using UcabGo.App.Models;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class VehiclesViewModel : ViewModelBase
    {
        readonly IVehiclesApi vehiclesApi;

        [ObservableProperty]
        ObservableCollection<Vehicle> vehicles;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        bool isEmpty;

        public VehiclesViewModel(ISettingsService settingsService, INavigationService navigation, IVehiclesApi vehiclesApi) : base(settingsService, navigation)
        {
            vehicles = new ObservableCollection<Vehicle>();
            this.vehiclesApi = vehiclesApi;
        }

        public override async void OnAppearing()
        {
            IsEmpty = false;
            await Refresh();
        }

        [RelayCommand]
        async Task Refresh()
        {
            Vehicles.Clear();
            IsRefreshing = true;

            var vehicles = await vehiclesApi.GetVehicles();
            if (vehicles?.Message == "VEHICLES_FOUND")
            {
                IsEmpty = vehicles.Data.Count == 0;

                foreach (var vehicle in vehicles.Data)
                {
                    Vehicles.Add(vehicle);
                }
            }

            IsRefreshing = false;
        }

        [RelayCommand]
        async Task AddVehicle()
        {
            await navigation.NavigateToAsync<VehiclesAddView>();
        }

        [RelayCommand]
        async Task UpdateVehicle(Vehicle vehicle)
        {
            var parameters = new Dictionary<string, object>
            {
                { "vehicle", vehicle }
            };

            await navigation.NavigateToAsync<VehiclesAddView>(parameters);
        }

        [RelayCommand]
        async Task DeleteVehicle(Vehicle vehicle)
        {
            var apiResponse = await vehiclesApi.DeleteVehicle(vehicle.Id);
            if (apiResponse.Message == "VEHICLE_DELETED")
            {
                Vehicles.Remove(vehicle);
                IsEmpty = Vehicles.Count == 0;
            }
        }
    }
}
