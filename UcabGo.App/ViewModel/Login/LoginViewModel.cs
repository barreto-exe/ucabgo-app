using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using UcabGo.App.Api.Interfaces;
using UcabGo.App.Api.Services.Locations;
using UcabGo.App.Services;

namespace UcabGo.App.ViewModel;

public partial class LoginViewModel : ViewModelBase
{
    readonly IAuthApi authService;
    readonly ILocationsApiService locationsApiService;

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
        ILocationsApiService locationsApiService,
        ISettingsService settingsService,
        INavigationService navigationService) : base(settingsService, navigationService)
    {
        this.authService = authService;
        this.locationsApiService = locationsApiService;

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

        var responseData = await authService.LoginAsync(Email, Password);
        var loginData = responseData?.Data;
        var message = responseData?.Message;

        if (loginData != null)
        {
            settings.User = loginData.User;
            settings.AccessToken = loginData.Token;

            var homeLocation = (await locationsApiService.GetUserHome()).Data;
            settings.Home = homeLocation;

            await navigation.RestartSession();
        }
        else if (message == "WRONG_CREDENTIALS")
        {
            IsInvalidCredentialsVisible = true;
        }

        IsEnabled = true;
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
