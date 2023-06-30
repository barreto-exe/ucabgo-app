using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class LoginView : ContentPage
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as LoginViewModel)?.OnAppearing();
    }
}

