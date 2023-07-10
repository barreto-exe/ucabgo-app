using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using UcabGo.App.Api.Services.User;
using UcabGo.App.Services;
using UcabGo.App.Utils;

namespace UcabGo.App.ViewModel
{
    public partial class PhoneViewModel : ViewModelBase
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
        bool isErrorVisible;

        readonly IUserApi userApi;

        public PhoneViewModel(ISettingsService settingsService, INavigationService navigation, IUserApi phoneApi) : base(settingsService, navigation)
        {
            this.userApi = phoneApi;

            OnAppearing();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Phone = settings.User.Phone;
            Name = settings.User.Name;
            LastName = settings.User.LastName;

            ButtonText = "Guardar";
            IsButtonEnabled = true;
            IsErrorVisible = false;
        }


        [RelayCommand]
        async Task Save()
        {
            //Validate all fields phone and names are not empty
            if (string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(LastName))
            {
                return;
            }

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

            Phone = Phone.FormatPhone();

            //Call api
            var apiResponse = await userApi.ChangePersonalInfoAsync(Name, LastName, Phone);
            if (apiResponse.Message == "USER_UPDATED")
            {
                var user = settings.User;
                user.Name = Name;
                user.LastName = LastName;
                user.Phone = Phone;
                settings.User = user;

                await Application.Current.MainPage.DisplayAlert("Éxito", "Información personal actualizada.", "Aceptar");

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
