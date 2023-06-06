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
        async Task Logout()
        {
            settings.User = null;
            settings.AccessToken = null;

            await navigation.RestartSession();
        }
    }
}
