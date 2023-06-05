using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Shells;
using UcabGo.App.ViewModel;
using UcabGo.App.Views;

namespace UcabGo.App.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly ISettingsService settings;
        public NavigationService(ISettingsService settings)
        {
            this.settings = settings;
        }

        public async Task RestartSession()
        {
            bool hasToken = !string.IsNullOrEmpty(settings.AccessToken);

            if (hasToken)
            {
                App.GoToAppShell();
            }
            else
            {
                App.GoToSessionShell();
            }
        }
        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
        {
            return
                routeParameters != null
                    ? Shell.Current.GoToAsync(route, routeParameters)
                    : Shell.Current.GoToAsync(route);
        }
        public Task PopAsync()
        {
            throw new NotImplementedException();
        }
        public Task GoBackAsync()
        {
            throw new NotImplementedException();
        }
    }
}
