using CommunityToolkit.Mvvm.Input;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class ProfileViewModel : ViewModelBase
    {
        public ProfileViewModel(
            ISettingsService settingsService,
            INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        [RelayCommand]
        async Task ChangePassword()
        {
            await navigation.NavigateToAsync<PasswordView>();
        }

        [RelayCommand]
        async Task ChangePhone()
        {
            await navigation.NavigateToAsync<PhoneView>();
        }
        [RelayCommand]
        async Task SosContacts()
        {
            await navigation.NavigateToAsync<SosContactsView>();
        }
        [RelayCommand]
        async Task Vehicles()
        {
            await navigation.NavigateToAsync<VehiclesView>();
        }
        [RelayCommand]
        async Task MyTrips()
        {
            //await navigation.NavigateToAsync<MyTripsView>();
        }

        [RelayCommand]
        async Task MyHouse()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                await navigation.NavigateToAsync<MapView>();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Es necesario que aceptes los permisos de ubicación para poder continuar", "Aceptar");
            }
        }

        [RelayCommand]
        async Task WalkingDistance()
        {
            await navigation.NavigateToAsync<WalkingDistanceView>();
        }

        [RelayCommand]
        async Task Logout()
        {
            settings.User = null;
            settings.AccessToken = null;

            await navigation.RestartSession();
        }
    }
}
