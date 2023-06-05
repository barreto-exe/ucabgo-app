using System.Runtime.CompilerServices;
using UcabGo.App.Services;
using UcabGo.App.Shells;

namespace UcabGo.App;

public partial class App : Application
{
    private readonly ISettingsService settingsService;
    private readonly INavigationService navigationService;
    private static IServiceProvider ServiceProvider { get; set; }

    public App(ISettingsService settingsService, INavigationService navigationService, IServiceProvider serviceProvider)
    {
        InitializeComponent();

        this.settingsService = settingsService;
        this.navigationService = navigationService;
        ServiceProvider = serviceProvider;
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var navigationService = ServiceProvider.GetService<INavigationService>();
        bool isLoggedIn = settingsService.IsLoggedIn;
        if (isLoggedIn)
        {
            return new Window(new AppShell(navigationService));
        }
        else
        {
            return new Window(new SessionShell(navigationService));
        }
    }

    public static void GoToAppShell()
    {
        var navigationService = ServiceProvider.GetService<INavigationService>();
        Current.MainPage = new AppShell(navigationService);
    }
    public static void GoToSessionShell()
    {
        var navigationService = ServiceProvider.GetService<INavigationService>();
        Current.MainPage = new SessionShell(navigationService);
    }
}
