using UcabGo.App.ViewModel;

namespace UcabGo.App.Services
{
    public interface INavigationService
    {
        Task InitializeAsync();

        Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null);

        Task PopAsync();

        Task GoBackAsync();
    }
}
