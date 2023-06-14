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
            await navigation.NavigateToAsync<MapView>();
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
