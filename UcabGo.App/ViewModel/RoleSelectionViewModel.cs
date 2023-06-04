using CommunityToolkit.Mvvm.Input;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class RoleSelectionViewModel : ViewModelBase
    {
        public RoleSelectionViewModel(
            ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
            ValidateToken().Wait();
        }

        [RelayCommand]
        async Task Logout()
        {
            settings.User = null;
            settings.AccessToken = null;

            await navigation.NavigateToAsync(nameof(LoginView));
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
