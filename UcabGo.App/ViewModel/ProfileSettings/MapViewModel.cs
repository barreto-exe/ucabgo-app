using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.GoogleMaps;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.GoogleMaps;
using UcabGo.App.Api.Services.Locations;
using UcabGo.App.Services;
using UcabGo.App.Utils;
using Map = Maui.GoogleMaps.Map;

namespace UcabGo.App.ViewModel
{
    public partial class MapViewModel : ViewModelBase
    {
        readonly IGoogleMapsApi mapsService;
        readonly ILocationsApiService locationsApiService;

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

        Map map;
        public Map Map
        {
            set
            {
                map = value;
                map.MapClicked += Map_MapClicked;

                map.DrawCampus();
                map.MoveCameraToCampus();
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

        Pin currentPin;
        public Pin CurrentPin
        {
            get => currentPin;
            private set
            {
                currentPin = value;
                map.SetPinOnMap(currentPin);
            }
        }


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

            if (settings.Home != null)
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
                map.MoveCameraToCampus();
            }
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            CurrentPin = MapHelper.GetSelectedPositionPin(e.Point);
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

        [RelayCommand]
        async Task PerformSearch()
        {
            var currentLocation = await MapHelper.GetCurrentLocation();

            var results = await mapsService.GetPlaces(new GooglePlaceFilter
            {
                Query = SearchQuery,
                Latitude = currentLocation?.Latitude,
                Longitude = currentLocation?.Longitude,
            });

            if (results != null)
            {
                SearchResults = new(results.Take(3));
            }

            UpdateSearchResults();
        }
        [RelayCommand]
        async Task PerformSelection()
        {
            IsResultsVisible = false;
            CurrentPin = new()
            {
                Type = PinType.Place,
                Label = SelectedResult.Name,
                Address = SelectedResult.Address,
                Position = new Position(SelectedResult.Latitude, SelectedResult.Longitude),
                Rotation = 0f,
                Tag = "id_selected",
                IsVisible = true
            };
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
            if (CurrentPin == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar una ubicación", "Aceptar");
                return;
            }

            ButtonText = "Guardando...";
            IsButtonEnabled = false;

            var geocode = await mapsService.GetGeocode(
                CurrentPin.Position.Latitude,
                CurrentPin.Position.Longitude);
            var opciones = geocode.Components.Select(x => "📍 " + x).Append("Otro...").ToArray();

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

            if (zone == "Otro..." || string.IsNullOrEmpty(zone))
            {
                zone = await Application.Current.MainPage
                    .DisplayPromptAsync(
                        "Zona",
                        message: "Indica la zona de tu destino.",
                        cancel: "Cancelar",
                        placeholder: "Ej: Alta Vista, Unare...",
                        accept: "Aceptar");
            }
            else
            {
                zone = zone.Replace("📍 ", "");
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
                        placeholder: "Ej: Al final de la calle.",
                        accept: "Aceptar");
            if (detail == "Cancelar" || string.IsNullOrEmpty(detail))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe indicar el detalle", "Aceptar");

                ButtonText = "Guardar";
                IsButtonEnabled = true;
                return;
            }

            var location = new Models.Location
            {
                Latitude = CurrentPin.Position.Latitude,
                Longitude = CurrentPin.Position.Longitude,
                Alias = "Casa",
                Zone = zone,
                Detail = detail,
            };

            var myHomeResponse = await locationsApiService.PostUserHome(location);

            if (myHomeResponse.Message == "HOME_UPDATED")
            {
                settings.Home = myHomeResponse.Data;
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

        private async Task SetMapOnCurrentLocation()
        {
            var currentLocation = await MapHelper.GetCurrentLocation();
            if (currentLocation == null)
            {
                return;
            }

            var position = new Position(currentLocation.Latitude, currentLocation.Longitude);

            CurrentPin = MapHelper.GetCurrentPositionPin(position);
        }
        private void SetMapOnHome()
        {
            CurrentPin = MapHelper.GetHomePin(settings.Home);
        }
        private void UpdateSearchResults()
        {
            IsResultsVisible = SearchResults?.Count > 0 && !string.IsNullOrEmpty(SearchQuery);
        }
    }
}
