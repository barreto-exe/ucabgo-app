using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App;

public partial class AppShell : Shell
{
    private readonly INavigationService navigationService;
    public AppShell(INavigationService navigationService)
    {
        InitializeComponent();
        this.navigationService = navigationService;

        //Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
        //Routing.RegisterRoute(nameof(RoleSelectionView), typeof(RoleSelectionView));
    }

    protected override async void OnParentSet()
    {
        base.OnParentSet();

        if(Parent is not null)
        {
            await navigationService.InitializeAsync();
        }
    }
}
