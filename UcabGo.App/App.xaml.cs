using System.Runtime.CompilerServices;
using UcabGo.App.Services;
using UcabGo.App.Shells;

namespace UcabGo.App;

public partial class App : Application
{
    public static SessionShell SessionShell { get; set; }
    public static AppShell AppShell { get; set; }
    private readonly ISettingsService settingsService;

    public App(AppShell appShell, SessionShell sessionShell, ISettingsService settingsService)
    {
        InitializeComponent();

        AppShell = appShell;
        SessionShell = sessionShell;

        this.settingsService = settingsService;
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        bool isLoggedIn = settingsService.IsLoggedIn;
        if (isLoggedIn)
        {
            return new Window(AppShell);
        }
        else
        {
            return new Window(SessionShell);
        }
    }

    public static void GoToAppShell()
    {
        Current.MainPage = AppShell;
    }
    public static void GoToSessionShell()
    {
        Current.MainPage = SessionShell;
    }
}
