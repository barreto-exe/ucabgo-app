using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Services;

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
        async Task Logout()
        {
            settings.User = null;
            settings.AccessToken = null;

            await navigation.RestartSession();
        }
    }
}
