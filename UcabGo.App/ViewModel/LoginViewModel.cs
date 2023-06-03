using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using UcabGo.App.ApiAccess.Interfaces;
using UcabGo.App.Views;

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

    public LoginViewModel(IAuthService authService)
    {
        this.authService = authService;
        isEnabled = true;

#if DEBUG
        email = "luis@est.ucab.edu.ve";
        password = "Good-1234";
#endif
    }

    [RelayCommand]
    async Task Login()
    {
        IsInvalidCredentialsVisible = false;
        IsEmailErrorVisible = !IsValidEmail();
        IsPasswordErrorVisible = !IsValidPassword();

        if (IsEmailErrorVisible || IsPasswordErrorVisible) return;

        IsEnabled = false;

        var response = await authService.LoginAsync(Email, Password);

        if(response != null)
        {
            var userJson = JsonConvert.SerializeObject(response.User);

            Preferences.Set("User", userJson);
            Preferences.Set("Token", response.Token);

            await Shell.Current.GoToAsync(nameof(RoleSelectionView));
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
