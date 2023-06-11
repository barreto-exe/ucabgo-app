using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Api.Services.User;
using UcabGo.App.Services;

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
        }

        public bool IsValidPhone(string phone)
        {
            return true;
        }

        [RelayCommand]
        async Task ChangePhone()
        {
            //Hide all errors
            //...

            //Validate fields
            if (!IsValidPhone(Phone))
            {
                //Show error
                //...

                return;
            }


            ButtonText = "Guardando...";
            IsButtonEnabled = false;

            //Call api
            var apiResponse = await phoneApi.ChangePhoneAsync(Phone);
            if(apiResponse.Message == "PHONE_UPDATED")
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
