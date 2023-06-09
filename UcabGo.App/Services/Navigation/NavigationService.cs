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
        public Task PopAsync()
        {
            throw new NotImplementedException();
        }
        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        public Task NavigateToAsync<T>(IDictionary<string, object> routeParameters = null) where T : Page
        {
            return
                routeParameters != null
                    ? Shell.Current.GoToAsync(typeof(T).Name, routeParameters)
                    : Shell.Current.GoToAsync(typeof(T).Name);
        }

        public Task NavigateToAsync(string route)
        {
            return Shell.Current.GoToAsync(route);
        }
    }
}
