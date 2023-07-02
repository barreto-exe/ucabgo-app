using CommunityToolkit.Mvvm.Input;
using UcabGo.App.Api.Services.Driver;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class RoleSelectionViewModel : ViewModelBase
    {
        readonly IDriverApi driverApi;

        public RoleSelectionViewModel(
            ISettingsService settingsService, INavigationService navigationService, IDriverApi driverApi) : base(settingsService, navigationService)
        {
            ValidateToken().Wait();
            this.driverApi = driverApi;
        }
        public override async void OnAppearing()
        {
            base.OnAppearing();

            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                var rides = await driverApi.GetRides(onlyAvailable: true);
                if (rides?.Data?.Count > 0)
                {
                    await navigation.NavigateToAsync<ActiveRiderView>();
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Es necesario que aceptes los permisos de ubicación para poder continuar. Actívalos cuando la app lo solicite, o puedes hacerlo manualmente en la configuración de tu dispositivo.", "Aceptar");
                await Logout();
            }
        }

        [RelayCommand]
        async Task Driver()
        {
            await navigation.NavigateToAsync<DestinationsListView>();
        }
        [RelayCommand]
        async Task Passenger()
        {
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
    }
}
