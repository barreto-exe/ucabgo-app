using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.Shells;

public partial class SessionShell : Shell
{
    private readonly INavigationService navigationService;
    public SessionShell(INavigationService navigationService)
    {
        InitializeComponent();
        this.navigationService = navigationService;

        Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
    }

    protected override async void OnParentSet()
    {
        base.OnParentSet();

        //if (Parent is not null)
        //{
        //    await navigationService.InitializeAsync();
        //}
    }
}
