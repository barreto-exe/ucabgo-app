using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.GoogleMaps;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.GoogleMaps;
using UcabGo.App.Api.Services.Locations;
using UcabGo.App.Models;
using UcabGo.App.Services;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using Map = Maui.GoogleMaps.Map;

namespace UcabGo.App.ViewModel
{
    public partial class MapViewModel : ViewModelBase
    {
        readonly IGoogleMapsApi mapsService;
        readonly ILocationsApiService locationsApiService;

        Map map;
        public Map Map 
        { 
            set
            {
                map = value;

                map.MapClicked += Map_MapClicked;

                DrawCampus();
                SetMapOnCampus();
            }
        }
        SearchBar searchBar;
        public SearchBar SearchBar
        {
            set
            {
                searchBar = value;
                searchBar.TextChanged += SearchBar_TextChanged;
            }
        }

        [ObservableProperty]
        string searchQuery;

        [ObservableProperty]
        ObservableCollection<PlaceDto> searchResults;

        [ObservableProperty]
        PlaceDto selectedResult;

        [ObservableProperty]
        bool isResultsVisible;

        [ObservableProperty]
        string buttonText;

        [ObservableProperty]
        bool isButtonEnabled;

        Pin pin;

        public MapViewModel(ISettingsService settingsService, INavigationService navigation, IGoogleMapsApi mapsService, ILocationsApiService locationsApiService) : base(settingsService, navigation)
        {
            this.mapsService = mapsService; 
            this.locationsApiService = locationsApiService;
            this.searchResults = new();
        }
        public override void OnAppearing()
        {
            base.OnAppearing();

            SearchQuery = string.Empty;
            IsResultsVisible = false;
            ButtonText = "Guardar";
            IsButtonEnabled = true;

            if(settings.Home != null)
            {
                map.InitialCameraUpdate = 
                    CameraUpdateFactory
                    .NewPositionZoom(
                        new Position(settings.Home.Latitude, settings.Home.Longitude), 
                        16);

                SetMapOnHome();
            }
            else
            {
                SetMapOnCampus();
            }
        }


        //Events
        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            pin = new Pin
            {
                Type = PinType.Place,
                Label = $"Ubicación seleccionada",
                Position = e.Point,
                Rotation = 0f,
                Tag = "id_selected",
                IsVisible = true
            };

            SetPinOnMap(pin);
        }
        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchResults = new();
            UpdateSearchResults();

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                await PerformSearch();
            }

            UpdateSearchResults();
        }
        private void UpdateSearchResults()
        {
            IsResultsVisible = SearchResults?.Count > 0 && !string.IsNullOrEmpty(SearchQuery);
        }

        [RelayCommand]
        async Task PerformSearch()
        {
            var currentLocation = await CurrentLocation();

            var results = await mapsService.GetPlaces(new GooglePlaceFilter
            {
                Query = SearchQuery,
                Latitude = currentLocation?.Latitude,
                Longitude = currentLocation?.Longitude,
            });

            if(results != null)
            {
                SearchResults = new(results.Take(3));
            }

            UpdateSearchResults();
        }
        [RelayCommand]
        async Task PerformSelection()
        {
            IsResultsVisible = false;
            pin = new()
            {
                Type = PinType.Place,
                Label = SelectedResult.Name,
                Address = SelectedResult.Address,
                Position = new Position(SelectedResult.Latitude, SelectedResult.Longitude),
                Rotation = 0f,
                Tag = "id_selected",
                IsVisible = true
            };
            SetPinOnMap(pin);
        }
        [RelayCommand]
        async Task GoToCurrentLocation()
        {
            await SetMapOnCurrentLocation();
        }
        [RelayCommand]
        async Task Cancel()
        {
            await navigation.GoBackAsync();
        }
        [RelayCommand]
        async Task Save()
        {
            if (pin == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar una ubicación", "Aceptar");
                return;
            }

            ButtonText = "Guardando...";
            IsButtonEnabled = false;

            var geocode = await mapsService.GetGeocode(pin.Position.Latitude, pin.Position.Longitude);
            var opciones = geocode.Components.Append("Otro...").ToArray();

            string zone = "";
            if (geocode.Components.Count() > 1)
            {
                zone = 
                    await Application.Current.MainPage
                    .DisplayActionSheet(
                        "Indica la zona de tu destino",
                        "Cancelar", 
                        null, 
                        opciones);
            }

            if(zone == "Otro..." || string.IsNullOrEmpty(zone))
            {
                zone = await Application.Current.MainPage
                    .DisplayPromptAsync(
                        "Zona",
                        message: "Indica la zona de tu destino.",
                        cancel: "Cancelar",
                        placeholder: "Ej: Alta Vista, Unare...");
            }

            if (zone == "Cancelar" || string.IsNullOrEmpty(zone))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe indicar la zona", "Aceptar");

                ButtonText = "Guardar";
                IsButtonEnabled = true;
                return;
            }

            string detail = await Application.Current.MainPage
                    .DisplayPromptAsync(
                        "Detalle",
                        message: "Punto de referencia o información extra para el conductor.",
                        cancel: "Cancelar",
                        placeholder: "Ej: Al final de la calle.");
            if (detail == "Cancelar" || string.IsNullOrEmpty(detail))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe indicar el detalle", "Aceptar");

                ButtonText = "Guardar";
                IsButtonEnabled = true;
                return;
            }

            var location = new Models.Location
            {
                Latitude = pin.Position.Latitude,
                Longitude = pin.Position.Longitude,
                Zone = zone,
                Detail = detail,
            };

            var response = await locationsApiService.PostUserHome(location);
            if (response.Message == "HOME_UPDATED")
            {
                settings.Home = response.Data;
                await Application.Current.MainPage.DisplayAlert("Éxito", "Ubicación guardada", "Aceptar");
                await navigation.GoBackAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo guardar la ubicación", "Aceptar");
            }

            ButtonText = "Guardar";
            IsButtonEnabled = true;
        }

        private void SetMapOnCampus()
        {
            pin = new()
            {
                Type = PinType.Place,
                Label = "UCAB Guayana",
                Address = "Puerto Ordaz, Bolívar, Venezuela",
                Position = new Position(8.29727428d, -62.71308436d),
                Rotation = 0f,
                Tag = "id_ucab",
                IsVisible = true
            };
            SetPinOnMap(pin);
        }
        private async Task SetMapOnCurrentLocation()
        {
            var currentLocation = await CurrentLocation();

            if (currentLocation == null)
            {
                return;
            }

            pin = new()
            {
                Type = PinType.Place,
                Label = "Ubicación actual",
                Position = new Position(currentLocation.Latitude, currentLocation.Longitude),
                Rotation = 0f,
                Tag = "id_current",
                IsVisible = true
            };

            SetPinOnMap(pin);
        }
        private void SetMapOnHome()
        {
            var home = settings.Home;

            pin = new()
            {
                Type = PinType.Place,
                Label = "Casa",
                Address = home.Zone,
                Position = new Position(home.Latitude, home.Longitude),
                Rotation = 0f,
                Tag = "id_home",
                IsVisible = true
            };
            SetPinOnMap(pin);
        }
        private void DrawCampus()
        {
            //The coordinates of the campus are:
            //8.295203, -62.712272
            //8.297396, -62.709854
            //8.298067, -62.709864
            //8.298469, -62.709638
            //8.298696, -62.709724
            //8.299402, -62.710098
            //8.300754, -62.710833
            //8.301193, -62.709328
            //8.302388, -62.709542
            //8.301965, -62.710951
            //8.301633, -62.711809
            //8.301132, -62.712760
            //8.300564, -62.712958
            //8.299417, -62.713092
            //8.299067, -62.713178
            //8.296013, -62.713944

            var polygon = new Polygon();
            polygon.Positions.Add(new Position(8.295203d, -62.712272d));
            polygon.Positions.Add(new Position(8.297396d, -62.709854d));
            polygon.Positions.Add(new Position(8.298067d, -62.709864d));
            polygon.Positions.Add(new Position(8.298469d, -62.709638d));
            polygon.Positions.Add(new Position(8.298696d, -62.709724d));
            polygon.Positions.Add(new Position(8.299402d, -62.710098d));
            polygon.Positions.Add(new Position(8.300754d, -62.710833d));
            polygon.Positions.Add(new Position(8.301193d, -62.709328d));
            polygon.Positions.Add(new Position(8.302388d, -62.709542d));
            polygon.Positions.Add(new Position(8.301965d, -62.710951d));
            polygon.Positions.Add(new Position(8.301633d, -62.711809d));
            polygon.Positions.Add(new Position(8.301132d, -62.712760d));
            polygon.Positions.Add(new Position(8.300564d, -62.712958d));
            polygon.Positions.Add(new Position(8.299417d, -62.713092d));
            polygon.Positions.Add(new Position(8.299067d, -62.713178d));
            polygon.Positions.Add(new Position(8.296013d, -62.713944d));

            Color color = Colors.Green;
            polygon.StrokeColor = color;
            polygon.StrokeWidth = 3f;

            //Fill color the same green with 50% transparency
            polygon.FillColor = Color.FromRgba(0, 97, 37, 0.1);

            map.Polygons.Add(polygon);
        }
        private void SetPinOnMap(Pin pin)
        {
            map.Pins.Clear();
            map.Pins.Add(pin);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromKilometers(0.5)));
        }
        async Task<Location> CurrentLocation()
        {
            var location =
                await Geolocation.GetLocationAsync() ??
                await Geolocation.GetLastKnownLocationAsync();

            if (location == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo obtener la ubicación actual", "Aceptar");
            }
            return location;
        }
    }
}
