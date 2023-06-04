using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.ViewModel;
using UcabGo.App.Views;

namespace UcabGo.App.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly ISettingsService settings;
        public NavigationService(ISettingsService settingsService)
        {
            this.settings = settingsService;
        }


        public async Task InitializeAsync()
        {
            await NavigateToAsync(string.IsNullOrEmpty(settings.AccessToken)
                ? nameof(LoginView)
                : nameof(RoleSelectionView));
        }
        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
        {
            if(!route.Contains(nameof(LoginView)))
            {
                route = "//Main/" + route;
            }
            else
            {
                route = "//" + route;
            }

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
