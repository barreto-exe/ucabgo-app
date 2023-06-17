using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.Destinations;
using UcabGo.App.Models;
using UcabGo.App.Services;
using Location = UcabGo.App.Models.Location;

namespace UcabGo.App.ViewModel
{
    public partial class DestinationsListViewModel : ViewModelBase
    {
        readonly IDestinationsService destinationsService;

        [ObservableProperty]
        ObservableCollection<Location> destinations;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string greeting;


        public DestinationsListViewModel(ISettingsService settingsService, INavigationService navigation, IDestinationsService destinationsService) : base(settingsService, navigation)
        {
            destinations = new ObservableCollection<Location>();
            this.destinationsService = destinationsService;
        }

        public override async void OnAppearing()
        {
            Greeting = $"Hola, {settings.User.Name}.";
            await Refresh();
        }

        [RelayCommand]
        async Task Refresh()
        {
            Destinations.Clear();
            IsRefreshing = true;

            var destinations = await destinationsService.GetDriverDestinations();
            if (destinations?.Message == "DESTINATIONS_FOUND")
            {
                foreach (var destination in destinations.Data)
                {
                    Destinations.Add(destination);
                }
            }

            IsRefreshing = false;
        }

        [RelayCommand]
        async Task Add()
        {
            //await navigation.NavigateToAsync<DestinationAddView>();
        }
        [RelayCommand]
        async Task Start(Location destination)
        {
            //await navigation.NavigateToAsync<DestinationUpdateView>(destination);
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
