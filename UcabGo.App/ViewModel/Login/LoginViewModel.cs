using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using UcabGo.App.Api.Interfaces;
using UcabGo.App.Api.Services.Locations;
using UcabGo.App.Services;
using UcabGo.App.Utils;
using UcabGo.App.Views;

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
        email = "luisbarr1@est.ucab.edu.ve";
        password = "Good-1234";
#endif
    }

    [RelayCommand]
    async Task Login()
    {
        IsInvalidCredentialsVisible = false;
        IsEmailErrorVisible = !Email.IsValidEmail();
        IsPasswordErrorVisible = !Password.IsValidPassword();

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

            var campusLocation = await locationsApiService.GetUserCampus();
            settings.Campus = campusLocation;

            settings.ReloadImage = true;

            await navigation.RestartSession();
        }
        else if (message == "WRONG_CREDENTIALS")
        {
            IsInvalidCredentialsVisible = true;
        }

        IsEnabled = true;
    }

    [RelayCommand]
    async Task Register()
    {
        await navigation.NavigateToAsync<RegisterView>();
    }
}
