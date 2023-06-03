using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class RoleSelectionViewModel : ObservableObject
    {
        public RoleSelectionViewModel()
        {
            ValidateToken().Wait();
        }

        [RelayCommand]
        async Task Logout()
        {
            Preferences.Set("User", string.Empty);
            Preferences.Set("Token", string.Empty);
            await Shell.Current.GoToAsync($"{nameof(LoginView)}");
        }

        async Task ValidateToken()
        {
            var token = Preferences.Get("Token", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
               await Logout();
            }
        }
    }
}
