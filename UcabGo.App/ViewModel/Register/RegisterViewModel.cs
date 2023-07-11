using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UcabGo.App.Api.Interfaces;
using UcabGo.App.Api.Services.Locations;
using UcabGo.App.Api.Services.User;
using UcabGo.App.Models.Inputs;
using UcabGo.App.Services;
using UcabGo.App.Utils;

namespace UcabGo.App.ViewModel
{
    public partial class RegisterViewModel : ViewModelBase
    {
        [ObservableProperty]
        string phone;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string lastName;

        [ObservableProperty]
        string buttonText;

        [ObservableProperty]
        bool isButtonEnabled;

        [ObservableProperty]
        bool isPhoneErrorVisible;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private bool isCurrentPasswordErrorVisible;

        [ObservableProperty]
        private bool isNewPasswordErrorVisible;

        [ObservableProperty]
        private bool isConfirmPasswordErrorVisible;

        [ObservableProperty]
        string email;

        [ObservableProperty]
        bool isEmailErrorVisible;

        [ObservableProperty]
        bool isPasswordErrorVisible;

        [ObservableProperty]
        bool isInvalidCredentialsVisible;

        readonly IAuthApi authService;

        readonly ILocationsApiService locationsApiService;

        public RegisterViewModel(ISettingsService settingsService, INavigationService navigation, IAuthApi authService, ILocationsApiService locationsApiService) : base(settingsService, navigation)
        {
            this.authService = authService;
            this.locationsApiService = locationsApiService;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            ButtonText = "Guardar";
            IsButtonEnabled = true;

            IsInvalidCredentialsVisible = false;
            IsPhoneErrorVisible = false;

            CleanFields();
        }

        void CleanFields()
        {
            Email = string.Empty;
            Password = string.Empty;
            Phone = string.Empty;
            Name = string.Empty;
            LastName = string.Empty;
            ConfirmPassword = string.Empty;
        }

        [RelayCommand]
        async Task Register()
        {
            //Validate all fields phone and names are not empty
            if (string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(LastName))
            {
                return;
            }

            IsInvalidCredentialsVisible = false;
            IsPhoneErrorVisible = false;
            IsPhoneErrorVisible = !Phone.IsValidPhone();
            IsEmailErrorVisible = !Email.IsValidEmail();
            IsPasswordErrorVisible = !Password.IsValidPassword();
            IsConfirmPasswordErrorVisible = Password != ConfirmPassword;


            if (IsEmailErrorVisible || IsPasswordErrorVisible || IsPhoneErrorVisible || IsConfirmPasswordErrorVisible) return;

            IsButtonEnabled = false;
            ButtonText = "Guardando...";

            var responseData = await authService.RegisterAsync(new RegisterInput
            {
                Email = Email,
                Name = Name,
                LastName = LastName,
                Phone = Phone,
                Password = Password
            });
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
            else if (message == "USER_ALREADY_EXISTS")
            {
                IsInvalidCredentialsVisible = true;
            }

            IsButtonEnabled = true;
            ButtonText = "Guardar";
        }
    }
}
