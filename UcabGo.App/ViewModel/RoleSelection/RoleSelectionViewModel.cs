using CommunityToolkit.Mvvm.ComponentModel;
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

            var rides = await driverApi.GetRides(onlyAvailable: true);
            if(rides?.Data?.Count > 0)
            {
                await navigation.NavigateToAsync<ActiveRiderView>();
            }
        }

        [RelayCommand]
        async Task Driver()
        {
            await navigation.NavigateToAsync<DestinationsListView>();
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
