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

            //await navigation.NavigateToAsync<RateUserView>();
            //return;

            IsLoading = true;
            AreButtonsEnabled = false;

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

                await Task.WhenAny(ridesTask, contactsTask, vehiclesTask, passengerTask);

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
            List<string> options = new();

            string myHome = "➡🏡 Mi dirección de casa";
            string myPhone = "➡👤 Información personal";
            string myWalkingDistance = "➡🚶🏼 Distancia de caminata";
            string mySosContacts = "➡📞 Contactos de emergencia";
            string myVehicles = "➡🚗 Vehículos";

            if(settings.Home == null ){
                options.Add(myHome);
            }
            if(settings.User.Phone == null){
                options.Add(myPhone);
            }
            if(settings.User.WalkingDistance == 0 && !isDriver){
                options.Add(myWalkingDistance);
            }
            if(hasSosContacts == false){
                options.Add(mySosContacts);
            }
            if(hasVehicles == false && isDriver){
                options.Add(myVehicles);
            }

            if(options.Count > 0){
                var option =
                    await Application.Current.MainPage
                    .DisplayActionSheet(
                        "Por favor, complete la siguiente información:",
                        "Cancelar",
                        null,
                        options.ToArray());

                if(option == myHome)
                {
                    await navigation.NavigateToAsync<MapView>();
                }
                else if(option == myPhone)
                {
                    await navigation.NavigateToAsync<PhoneView>();
                }
                else if(option == myWalkingDistance)
                {
                    await navigation.NavigateToAsync<WalkingDistanceView>();
                }
                else if(option == mySosContacts)
                {
                    await navigation.NavigateToAsync<SosContactsView>();
                }
                else if(option == myVehicles)
                {
                    await navigation.NavigateToAsync<VehiclesView>();
                }

                return false;
            }
                
            return true;
        }
    }
}
