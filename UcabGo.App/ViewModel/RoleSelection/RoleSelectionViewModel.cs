using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UcabGo.App.Api.Services.Driver;
using UcabGo.App.Api.Services.PassengerService;
using UcabGo.App.Api.Services.SosContacts;
using UcabGo.App.Api.Services.Vehicles;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class RoleSelectionViewModel : ViewModelBase
    {
        readonly IDriverApi driverApi;
        readonly IPassengerApi passengerApi;
        readonly ISosContactsApi sosContactApi;
        readonly IVehiclesApi vehiclesApi;
        bool hasSosContacts;
        bool hasVehicles;

        [ObservableProperty]
        bool areButtonsEnabled;

        [ObservableProperty]
        bool isLoading;

        [ObservableProperty]
        double progressBarValue;

        public RoleSelectionViewModel(
            ISettingsService settingsService, INavigationService navigationService, IDriverApi driverApi, ISosContactsApi contactsApi, IVehiclesApi vehiclesApi, IPassengerApi passengerApi) : base(settingsService, navigationService)
        {
            ValidateToken().Wait();
            this.driverApi = driverApi;
            this.passengerApi = passengerApi;
            this.sosContactApi = contactsApi;
            this.vehiclesApi = vehiclesApi;
        }
        public override async void OnAppearing()
        {
            base.OnAppearing();

            IsLoading = true;
            AreButtonsEnabled = false;
            ProgressBarValue = 0;

            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                var ridesTask = driverApi.GetRides(onlyAvailable: true);
                var passengerTask = passengerApi.GetRides(onlyAvailable: true);
                var contactsTask = sosContactApi.GetSosContacts();
                var vehiclesTask = vehiclesApi.GetVehicles();

                ProgressBarValue = 0.2;

                await Task.WhenAll(ridesTask, contactsTask, vehiclesTask, passengerTask);

                await LoadingAnimation();

                var rides = await ridesTask;
                if (rides?.Data?.Count() > 0)
                {
                    await navigation.NavigateToAsync<ActiveRiderView>();
                }
                var passenger = await passengerTask;
                if (passenger?.Data?.Count() > 0)
                {
                    await navigation.NavigateToAsync<ActivePassengerView>();
                }

                var contacts = await contactsTask;
                hasSosContacts = contacts?.Data?.Count() > 0;
                if (hasSosContacts)
                {
                    settings.SosContacts = contacts.Data;
                }

                var vehicles = await vehiclesTask;
                hasVehicles = vehicles?.Data?.Count() > 0;
                if (hasVehicles)
                {
                    settings.Vehicles = vehicles.Data;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Es necesario que aceptes los permisos de ubicación para poder continuar. Actívalos cuando la app lo solicite, o puedes hacerlo manualmente en la configuración de tu dispositivo.", "Aceptar");
                await Logout();
            }

            IsLoading = false;
            AreButtonsEnabled = true;

            async Task LoadingAnimation()
            {
                await Task.Delay(100);
                ProgressBarValue = 0.5;
                await Task.Delay(100);
                ProgressBarValue = 0.6;
                await Task.Delay(100);
                ProgressBarValue = 0.75;
                await Task.Delay(100);
                ProgressBarValue = 0.95;
                await Task.Delay(100);
                ProgressBarValue = 1;
            }
        }

        [RelayCommand]
        async Task Driver()
        {
            bool isValid = await ValidateInitialValues(true);
            if(!isValid){
                return;
            }
            await navigation.NavigateToAsync<DestinationsListView>();
        }
        [RelayCommand]
        async Task Passenger()
        {
            bool isValid = await ValidateInitialValues(false);
            if(!isValid){
                return;
            }
            await navigation.NavigateToAsync<SelectDestinationView>();
        }

        [RelayCommand]
        async Task Logout()
        {
            settings.User = null;
            settings.AccessToken = null;

            await navigation.RestartSession();
        }

        async Task ValidateToken()
        {
            if (string.IsNullOrEmpty(settings.AccessToken))
            {
                await Logout();
            }
        }

        async Task<bool> ValidateInitialValues(bool isDriver)
        {
            List<string> options = new List<string>();  
                      // string.IsNullOrEmpty(settings.Home)
            if(settings.Home == null ){
                options.Add("Mi dirección de casa");
            }
            if(settings.User.Phone == null){
                options.Add("Número de teléfono");
            }
            if(settings.User.WalkingDistance == 0 && !isDriver){
                options.Add("Distancia de caminata");
            }
            if(hasSosContacts == false){
                options.Add("Contactos de emergencia");
            }
            if(hasVehicles == false && isDriver){
                options.Add("Vehículos");
            }

            if(options.Count > 0){
                var option =
                    await Application.Current.MainPage
                    .DisplayActionSheet(
                        "Por favor, complete la siguiente información:",
                        "Cancelar",
                        null,
                        options.ToArray());

                switch(option){
                    case "Mi dirección de casa":
                        await navigation.NavigateToAsync<MapView>();
                        break;
                    case "Número de teléfono":
                        await navigation.NavigateToAsync<PhoneView>();
                        break;
                    case "Distancia de caminata":
                        await navigation.NavigateToAsync<WalkingDistanceView>();
                        break;
                    case "Contactos de emergencia":
                        await navigation.NavigateToAsync<SosContactsView>();
                        break;
                    case "Vehículos":
                        await navigation.NavigateToAsync<VehiclesView>();
                        break;
                    default:
                        return false;
                }

                return false;
            }
                
            return true;
        }
    }
}
