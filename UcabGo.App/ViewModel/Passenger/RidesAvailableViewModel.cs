using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.PassengerService;
using UcabGo.App.Api.Services.PassengerService.Inputs;
using UcabGo.App.Api.Services.Rides;
using UcabGo.App.Models;
using UcabGo.App.Services;
using UcabGo.App.Utils;
using UcabGo.App.Views;
using Location = UcabGo.App.Models.Location;


namespace UcabGo.App.ViewModel
{
    [QueryProperty(nameof(SelectedDestination), "destination")]
    public partial class RidesAvailableViewModel : ViewModelBase
    {
        readonly IRidesApi ridesApi;
        readonly IPassengerApi passengerApi;

        [ObservableProperty]
        Location selectedDestination;

        [ObservableProperty]
        ObservableCollection<RideMatching> rides;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string greeting;

        [ObservableProperty]
        bool noRidesFound;

        [ObservableProperty]
        bool ridesFound;

        public RidesAvailableViewModel(ISettingsService settingsService, INavigationService navigation, IRidesApi ridesApi, IPassengerApi passengerApi) : base(settingsService, navigation)
        {
            this.ridesApi = ridesApi;
            this.passengerApi = passengerApi;
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
            
            var ridesTask = ridesApi.GetMatchingRides(SelectedDestination, Convert.ToInt32(settings.User.WalkingDistance), goingToCampus);
            var passengerTask = passengerApi.GetRides(onlyAvailable: true);

            await Task.WhenAll(ridesTask, passengerTask);

            var ridesResponse = await ridesTask;
            var passengerResponse = await passengerTask;

            if (ridesResponse?.Message == "RIDES_FOUND")
            {
                if (!goingToCampus)
                {
                    Rides = new(ridesResponse.Data.Where(x => x.Ride.AvailableSeats > 0));
                }
                else
                {
                    Rides = new(ridesResponse.Data.Where(x => x.Ride.AvailableSeats > 0).Select(r =>
                    {
                        r.Ride.Destination.Zone = "UCAB Guayana.";
                        return r;
                    }));
                }
            }

            if (passengerResponse?.Message == "RIDES_FOUND" && passengerResponse.Data.Any(x => x.IsAvailable))
            {
                //Is a passenger that's not been ignored or cancelled
                var passengers = passengerResponse.Data.First(x => x.IsAvailable).Passengers;
                bool isPassenger = passengers.Any(x => x.Id == settings.User.Id && x.TimeIgnored == null && x.TimeCancelled == null);

                if (isPassenger)
                {
                    await navigation.NavigateToAsync<ActivePassengerView>();
                }
            }

            RidesFound = Rides?.Count > 0;
            NoRidesFound = !RidesFound;
            IsRefreshing = false;
        }

        [RelayCommand]
        async Task SelectRide(RideMatching ride)
        {
            var isSolicited = await Application.Current.MainPage.DisplayAlert("Confirmar", $"¿Desea solicitar el viaje de {ride.Ride.Driver.Name}?", "Sí", "No");

            if(isSolicited)
            {
                var location = await MapHelper.GetCurrentLocation();

                var input = new PassengerInput
                {
                    Ride = ride.Ride.Id,
                    FinalLocation = SelectedDestination.Id,
                    LatitudeOrigin = location.Latitude,
                    LongitudeOrigin = location.Longitude,
                };

                var response = await passengerApi.AskForRide(input);
                if (response?.Message == "ASKED_FOR_RIDE")
                {
                    await navigation.NavigateToAsync<ActivePassengerView>();
                }
                else if(response?.Message != null)
                {
                    string errorMessage = response.Message switch
                    {
                        "RIDE_NOT_FOUND" => "El viaje no existe.",
                        "LOCATION_NOT_FOUND" => "La ubicación no existe.",
                        "NO_AVAILABLE_SEATS" => "No hay asientos disponibles.",
                        "ALREADY_IN_RIDE" => "Ya has solicitado este viaje.",
                        _ => "Ha ocurrido un error al solicitar el viaje. Inténtalo de nuevo más tarde.",
                    };

                    await Application.Current.MainPage.DisplayAlert("Atención", errorMessage, "Aceptar");
                }
            }
        }
    }
}
