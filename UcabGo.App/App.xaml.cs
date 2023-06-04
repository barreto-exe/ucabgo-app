using UcabGo.App.Services;

namespace UcabGo.App;

public partial class App : Application
{
    public App(INavigationService service)
    {
        InitializeComponent();

        MainPage = new AppShell(service);
    }
}
