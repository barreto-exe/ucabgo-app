using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maui.GoogleMaps;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.Destinations;
using UcabGo.App.Api.Services.GoogleMaps;
using UcabGo.App.Services;
using UcabGo.App.Utils;
using UcabGo.App.Views;
using Map = Maui.GoogleMaps.Map;

namespace UcabGo.App.ViewModel
{
    public partial class SelectDestinationViewModel : ViewModelBase
    {
        readonly IGoogleMapsApi mapsService;
        readonly IDestinationsService destinationsService;

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

                map.DrawCampus(CampusClicked);
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

        public SelectDestinationViewModel(ISettingsService settingsService, INavigationService navigation, IGoogleMapsApi mapsService, IDestinationsService destinationsService) : base(settingsService, navigation)
        {
            this.mapsService = mapsService;
            this.destinationsService = destinationsService;
            this.searchResults = new();
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            CurrentPin = null;
            SearchQuery = string.Empty;
            IsResultsVisible = false;
            ButtonText = "Aceptar";
            IsButtonEnabled = true;

            //Set camera over current location or default
            //MapSpan mapSpan;
            //var location = await MapHelper.GetCurrentLocation();
            //if (location != null)
            //{
            //    //cameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(new Position(location.Latitude, location.Longitude), 13, 0, 0));
            //    mapSpan = MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(1));
            //    map.MoveToRegion(mapSpan);
            //}
            //else
            //{
            //    map.MoveCameraToCampus();
            //}
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
        async Task HomeClicked()
        {
            var home = settings.Home;
            if (home != null)
            {
                CurrentPin = new()
                {
                    Type = PinType.Place,
                    Label = home.Alias,
                    Address = home.Zone,
                    Position = new Position(home.Latitude, home.Longitude),
                    Rotation = 0f,
                    Tag = "id_home",
                    IsVisible = true
                };
            }
        }
        [RelayCommand]
        async Task CampusClicked()
        {
            var campus = settings.Campus;
            if (campus != null)
            {
                CurrentPin = new()
                {
                    Type = PinType.Place,
                    Label = campus.Alias,
                    Address = campus.Zone,
                    Position = new Position(campus.Latitude, campus.Longitude),
                    Rotation = 0f,
                    Tag = "id_campus",
                    IsVisible = true
                };
            }
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

            ButtonText = "Enviando...";
            IsButtonEnabled = false;

            Models.Location location = null;
            if (CurrentPin.Tag.ToString() != "id_home" && CurrentPin.Tag.ToString() != "id_campus")
            {
                var geocode = await mapsService.GetGeocode(
                    CurrentPin.Position.Latitude,
                    CurrentPin.Position.Longitude);
                var opciones = geocode.Components.Select(x => "📍 " + x).Append("Otro...").ToArray();

                string zone = string.Empty;
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

                    ButtonText = "Enviar";
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

                    ButtonText = "Enviar";
                    IsButtonEnabled = true;
                    return;
                }

                location = new Models.Location
                {
                    Latitude = CurrentPin.Position.Latitude,
                    Longitude = CurrentPin.Position.Longitude,
                    Alias = $"Destino {DateTime.Now:dd/MM/yy}",
                    Zone = zone,
                    Detail = detail,
                };

                var result = await destinationsService.AddDestination(location);
                if (result.Message == "LOCATION_CREATED")
                {
                    location.Id = result.Data.Id;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo enviar la ubicación", "Aceptar");
                }
            }
            else if (CurrentPin.Tag.ToString() == "id_home")
            {
                location = settings.Home;
            }
            else if (CurrentPin.Tag.ToString() == "id_campus")
            {
                location = settings.Campus;
            }

            var parameters = new Dictionary<string, object>
            {
                { "destination", location }
            };

            await navigation.GoBackAsync();
            await navigation.NavigateToAsync<RidesAvailableView>(parameters);

            ButtonText = "Enviar";
            IsButtonEnabled = true;
        }

        private void UpdateSearchResults()
        {
            IsResultsVisible = SearchResults?.Count > 0 && !string.IsNullOrEmpty(SearchQuery);
        }
    }
}
