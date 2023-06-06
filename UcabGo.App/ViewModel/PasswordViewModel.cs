using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using UcabGo.App.Api.Interfaces;
using UcabGo.App.Services;

namespace UcabGo.App.ViewModel
{
    public partial class PasswordViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string currentPassword;

        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private bool isCurrentPasswordErrorVisible;

        [ObservableProperty]
        private bool isNewPasswordErrorVisible;

        [ObservableProperty]
        private bool isConfirmPasswordErrorVisible;

        [ObservableProperty]
        private string buttonText;

        [ObservableProperty]
        private bool isButtonEnabled;

        readonly IAuthApi authService;

        public PasswordViewModel(ISettingsService settingsService, INavigationService navigation, IAuthApi authService) : base(settingsService, navigation)
        {
            this.authService = authService;

            OnAppearing();
        }

        public void OnAppearing()
        {
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
            IsCurrentPasswordErrorVisible = false;
            IsNewPasswordErrorVisible = false;
            IsConfirmPasswordErrorVisible = false;
            ButtonText = "Guardar";
            IsButtonEnabled = true;
        }

        [RelayCommand]
        async Task ChangePassword()
        {
            //Hide all errors
            IsCurrentPasswordErrorVisible = false;
            IsNewPasswordErrorVisible = false;
            IsConfirmPasswordErrorVisible = false;

            if (!IsValidPassword(NewPassword))
            {
                IsNewPasswordErrorVisible = true;
                return;
            }
            if (NewPassword != ConfirmPassword)
            {
                IsConfirmPasswordErrorVisible = true;
                return;
            }

            ButtonText = "Guardando...";
            IsButtonEnabled = false;

            var passChanged = await authService.ChangePasswordAsync(CurrentPassword, NewPassword);
            if (passChanged.Message == "PASSWORD_CHANGED")
            {
                await navigation.GoBackAsync();
            }
            else
            {
                IsCurrentPasswordErrorVisible = true;
            }

            ButtonText = "Guardar";
            IsButtonEnabled = true;
        }

        bool IsValidPassword(string password)
        {
            var pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
