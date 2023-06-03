using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using UcabGo.App.ApiAccess.Interfaces;
using UcabGo.App.ApiAccess.Services;

namespace UcabGo.App.ViewModel;

public partial class LoginViewModel : ObservableObject
{
    readonly IAuthService authService;

    [ObservableProperty]
    string email;

    [ObservableProperty]
    string password;

    [ObservableProperty]
    bool isEnabled;

    [ObservableProperty]
    bool isEmailErrorVisible;

    [ObservableProperty]
    bool isPasswordErrorVisible;

    [ObservableProperty]
    bool isInvalidCredentialsVisible;

    public LoginViewModel(AuthService authService)
    {
        this.authService = authService;
        isEnabled = true;

#if DEBUG
        email = "luis@est.ucab.edu.ve";
        password = "Good-1234";
#endif
    }

    [RelayCommand]
    async void Login()
    {
        IsInvalidCredentialsVisible = false;
        IsEmailErrorVisible = !IsValidEmail();
        IsPasswordErrorVisible = !IsValidPassword();

        if (IsEmailErrorVisible || IsPasswordErrorVisible) return;

        IsEnabled = false;

        var response = await authService.LoginAsync(Email, Password);

        if(response != null)
        {
            //Navigate to RolSelection
        }
        else
        {
            IsInvalidCredentialsVisible = true;
            IsEnabled = true;
        }
    }

    bool IsValidEmail()
    {
        var pattern = @"^[a-zA-Z0-9._%+-]+@(?:[a-zA-Z0-9-]+\.)?ucab\.edu\.ve$";
        return Regex.IsMatch(Email, pattern);
    }

    bool IsValidPassword()
    {
        var pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
        return Regex.IsMatch(Password, pattern);
    }
}
