using UcabGo.App.Views;

namespace UcabGo.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
        Routing.RegisterRoute(nameof(RoleSelectionView), typeof(RoleSelectionView));
    }
}
