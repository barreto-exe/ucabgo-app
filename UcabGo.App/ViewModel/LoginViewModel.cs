using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using UcabGo.App.Api.Interfaces;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel;

public partial class LoginViewModel : ViewModelBase
{
    readonly IAuthApi authService;

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

    public LoginViewModel(
        IAuthApi authService, 
        ISettingsService settingsService,
        INavigationService navigationService) : base(settingsService, navigationService)
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
            settings.User = response.User;
            settings.AccessToken = response.Token;

            await navigation.RestartSession();
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
