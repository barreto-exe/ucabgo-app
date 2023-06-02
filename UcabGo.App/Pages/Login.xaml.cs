using UcabGo.App.ApiAccess;
using UcabGo.App.ApiAccess.Services;

namespace UcabGo.App.Pages;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private async void BtnLogin_Clicked(object sender, EventArgs e)
    {
		var email = TxtEmail.Text;
		var password = TxtPassword.Text;

		var auth = new AuthService();
		var login = await auth.LoginAsync(email, password);
    }
}

