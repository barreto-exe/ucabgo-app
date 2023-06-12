namespace UcabGo.App.Services
{
    public interface INavigationService
    {
        Task RestartSession();

        Task NavigateToAsync<T>(IDictionary<string, object> routeParameters = null) where T : Page;

        Task PopAsync();

        Task GoBackAsync();
    }
}
