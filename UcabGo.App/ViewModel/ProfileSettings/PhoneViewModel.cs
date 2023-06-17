using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UcabGo.App.Api.Services.User;
using UcabGo.App.Services;
using UcabGo.App.Utils;

namespace UcabGo.App.ViewModel
{
    public partial class PhoneViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string phone;

        [ObservableProperty]
        private string buttonText;

        [ObservableProperty]
        private bool isButtonEnabled;

        [ObservableProperty]
        private bool isErrorVisible;

        private readonly IUserApi phoneApi;

        public PhoneViewModel(ISettingsService settingsService, INavigationService navigation, IUserApi phoneApi) : base(settingsService, navigation)
        {
            this.phoneApi = phoneApi;

            OnAppearing();
        }

        public override void OnAppearing()
        {
            Phone = settings.User.Phone;
            ButtonText = "Guardar";
            IsButtonEnabled = true;
            IsErrorVisible = false;
        }


        [RelayCommand]
        async Task ChangePhone()
        {
            //Hide all errors
            IsErrorVisible = false;

            //Validate fields
            if (!Phone.IsValidPhone())
            {
                IsErrorVisible = true;
                return;
            }

            ButtonText = "Guardando...";
            IsButtonEnabled = false;

            Phone = Phone.Trim();

            //Call api
            var apiResponse = await phoneApi.ChangePhoneAsync(Phone);
            if (apiResponse.Message == "PHONE_UPDATED")
            {
                var user = settings.User;
                user.Phone = Phone;
                settings.User = user;
                await navigation.GoBackAsync();
            }
            else
            {
                //Show error
                //...
            }


            ButtonText = "Guardar";
            IsButtonEnabled = true;
        }
    }
}
