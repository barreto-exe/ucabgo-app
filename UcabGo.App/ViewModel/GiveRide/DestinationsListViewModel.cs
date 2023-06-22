using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.Destinations;
using UcabGo.App.Api.Services.Vehicles;
using UcabGo.App.Models;
using UcabGo.App.Services;
using UcabGo.App.Views;
using Location = UcabGo.App.Models.Location;

namespace UcabGo.App.ViewModel
{
    public partial class DestinationsListViewModel : ViewModelBase
    {
        readonly IDestinationsService destinationsService;
        readonly IVehiclesApi vehiclesApi;

        [ObservableProperty]
        ObservableCollection<Location> destinations;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string greeting;

        [ObservableProperty]
        bool isModalVisible;

        [ObservableProperty]
        ObservableCollection<Vehicle> vehicles;

        [ObservableProperty]
        Vehicle selectedVehicle;

        [ObservableProperty]
        int seatQuantity;

        Location SelectedDestination { get; set; }

        public DestinationsListViewModel(ISettingsService settingsService, INavigationService navigation, IDestinationsService destinationsService, IVehiclesApi vehiclesApi) : base(settingsService, navigation)
        {
            this.destinationsService = destinationsService;
            this.vehiclesApi = vehiclesApi;

            destinations = new();
            vehicles = new();
        }

        public override async void OnAppearing()
        {
            Greeting = $"Hola, {settings.User.Name}.";
            IsModalVisible = false;
            SeatQuantity = 1;
            
            await Refresh();
        }

        [RelayCommand]
        async Task Refresh()
        {
            Destinations.Clear();
            IsRefreshing = true;

            var taskDestinations = destinationsService.GetDriverDestinations();
            var taskVehicles = vehiclesApi.GetVehicles();

            await Task.WhenAll(taskDestinations, taskVehicles);

            var destinations = await taskDestinations;

            if (destinations?.Message == "DESTINATIONS_FOUND")
            {
                foreach (var destination in destinations.Data)
                {
                    Destinations.Add(destination);
                }
            }

            var vehicles = await taskVehicles;
            if(vehicles?.Message == "VEHICLES_FOUND" && vehicles.Data.Any())
            {
                Vehicles = new(vehicles.Data);
                SelectedVehicle = Vehicles[0];
            }

            IsRefreshing = false;
        }

        [RelayCommand]
        async Task Add()
        {
            await navigation.NavigateToAsync<DestinationAddView>();
        }
        [RelayCommand]
        void Configure(Location destination)
        {
            IsModalVisible = true;
            SelectedDestination = destination;
        }
        [RelayCommand]
        async Task Start()
        {
            //Display test message
            await Application.Current.MainPage.DisplayAlert("Test", 
                $"Ride {SelectedDestination.Alias} - {SeatQuantity} started", "Ok");
        }
        [RelayCommand]
        void AddSeat()
        {
            SeatQuantity++;
        }
        [RelayCommand]
        void RemoveSeat()
        {
            if (SeatQuantity > 1)
            {
                SeatQuantity--;
            }
        }

        [RelayCommand]
        async Task Cancel()
        {
            IsModalVisible = false;
        }

        [RelayCommand]
        async Task Delete(Location destination)
        {
            var apiResponse = await destinationsService.DeleteDriverDestination(destination);
            if (apiResponse?.Message == "DESTINATION_DELETED")
            {
                Destinations.Remove(destination);
            }
        }
    }
}
