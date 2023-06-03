using UcabGo.App.ApiAccess;
using UcabGo.App.ApiAccess.Services;
using UcabGo.App.ViewModel;

namespace UcabGo.App.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}

