using UcabGo.App.ViewModel;

namespace UcabGo.App.Services
{
    public interface INavigationService
    {
        Task RestartSession();

        Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null);

        Task PopAsync();

        Task GoBackAsync();
    }
}
