using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UcabGo.App.Api.Services.SosContacts;
using UcabGo.App.Models;
using UcabGo.App.Services;
using UcabGo.App.Utils;

namespace UcabGo.App.ViewModel
{
    [QueryProperty(nameof(Contact), "contact")]
    public partial class SosContactsAddViewModel : ViewModelBase
    {
        [ObservableProperty]
        string nameEntry;

        [ObservableProperty]
        string phoneEntry;

        [ObservableProperty]
        string buttonText;

        [ObservableProperty]
        bool isButtonEnabled;

        [ObservableProperty]
        SosContact contact;

        [ObservableProperty]
        private bool isErrorVisible;

        bool isEditing => Contact != null;

        readonly ISosContactsApi sosContactsApi;

        public SosContactsAddViewModel(ISettingsService settingsService, INavigationService navigation, ISosContactsApi sosContactsApi) : base(settingsService, navigation)
        {
            this.sosContactsApi = sosContactsApi;
        }

        public override void OnAppearing()
        {
            ButtonText = "Guardar";
            IsButtonEnabled = true;
            IsErrorVisible = false;

            if (isEditing)
            {
                NameEntry = Contact.Name;
                PhoneEntry = Contact.Phone;
            }
            else
            {
                NameEntry = string.Empty;
                PhoneEntry = string.Empty;
            }
        }

        [RelayCommand]
        async Task Save()
        {
            IsErrorVisible = false;

            if (!PhoneEntry.IsValidPhone())
            {
                IsErrorVisible = true;
                return;
            }

            ButtonText = "Guardando...";
            IsButtonEnabled = false;

            PhoneEntry = PhoneEntry.Trim();

            if (isEditing)
            {
                await UpdateContact();
            }
            else
            {
                await AddContact();
            }


            ButtonText = "Guardar";
            IsButtonEnabled = true;
        }

        private async Task AddContact()
        {
            var sosContact = new SosContact
            {
                Name = NameEntry,
                Phone = PhoneEntry
            };
            var apiResponse = await sosContactsApi.AddSosContact(sosContact);
            if (apiResponse.Message == "SOSCONTACT_CREATED")
            {
                await navigation.GoBackAsync();
            }
        }

        private async Task UpdateContact()
        {
            var sosContact = new SosContact
            {
                Id = Contact.Id,
                Name = NameEntry,
                Phone = PhoneEntry
            };
            var apiResponse = await sosContactsApi.UpdateSosContact(sosContact);
            if (apiResponse.Message == "SOSCONTACT_UPDATED")
            {
                await navigation.GoBackAsync();
            }
            else if (apiResponse.Message == "SOSCONTACT_NOT_FOUND")
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No se pudo encontrar el contacto de emergencia",
                    "Ok");
                await navigation.GoBackAsync();
            }
        }

        [RelayCommand]
        async Task Cancel()
        {
            await navigation.GoBackAsync();
        }
    }
}
