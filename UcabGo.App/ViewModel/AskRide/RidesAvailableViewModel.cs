using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.Rides;
using UcabGo.App.Models;
using UcabGo.App.Services;
using Location = UcabGo.App.Models.Location;


namespace UcabGo.App.ViewModel
{
    [QueryProperty(nameof(SelectedDestination), "destination")]
    public partial class RidesAvailableViewModel : ViewModelBase
    {
        readonly IRidesApi ridesApi;

        [ObservableProperty]
        Location selectedDestination;

        [ObservableProperty]
        ObservableCollection<RideMatching> rides;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string greeting;

        [ObservableProperty]
        bool isModalVisible;

        [ObservableProperty]
        bool noRidesFound;

        [ObservableProperty]
        bool ridesFound;

        public RidesAvailableViewModel(ISettingsService settingsService, INavigationService navigation, IRidesApi ridesApi) : base(settingsService, navigation)
        {
            this.ridesApi = ridesApi;
            Rides = new();
        }

        public override async void OnAppearing()
        {
            //Go back if no destination is selected
            if (SelectedDestination == null)
            {
                await navigation.GoBackAsync();
                return;
            }

            Greeting = $"Hola, {settings.User.Name}.";
            IsModalVisible = false;

            await Refresh();
        }

        [RelayCommand]
        async Task Refresh()
        {
            Rides.Clear();
            IsRefreshing = true;
            RidesFound = false;
            NoRidesFound = false;

            bool goingToCampus = SelectedDestination.Alias.ToLower().Contains("ucab");
            var response = await ridesApi.GetMatchingRides(SelectedDestination, Convert.ToInt32(settings.User.WalkingDistance), goingToCampus);

            if (response?.Message == "RIDES_FOUND")
            {
                Rides = new(response.Data.Where(x => x.Ride.SeatQuantity > 0));
            }

            RidesFound = Rides?.Count > 0;
            NoRidesFound = !RidesFound;
            IsRefreshing = false;
        }

        [RelayCommand]
        async Task SelectRide(RideMatching ride)
        {
            //Confirm message
            var response = await Application.Current.MainPage.DisplayAlert("Confirmar", $"¿Desea solicitar el viaje de {ride.Ride.Driver.Name}?", "Sí", "No");
        }
    }
}
