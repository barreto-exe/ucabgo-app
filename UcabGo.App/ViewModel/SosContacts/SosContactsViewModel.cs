using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.SosContacts;
using UcabGo.App.Models;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class SosContactsViewModel : ViewModelBase
    {
        private readonly ISosContactsApi sosContactsApi;

        [ObservableProperty]
        ObservableCollection<SosContact> sosContacts;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        bool isEmpty;


        public SosContactsViewModel(ISettingsService settingsService, INavigationService navigation, ISosContactsApi sosContactsApi) : base(settingsService, navigation)
        {
            sosContacts = new ObservableCollection<SosContact>();
            this.sosContactsApi = sosContactsApi;
        }

        public override async void OnAppearing()
        {
            IsEmpty = false;
            await Refresh();
        }

        [RelayCommand]
        async Task Refresh()
        {
            SosContacts.Clear();
            IsRefreshing = true;

            var contacts = await sosContactsApi.GetSosContacts();
            if (contacts?.Message == "SOSCONTACTS_FOUND")
            {
                IsEmpty = !contacts.Data.Any();

                foreach (var contact in contacts.Data)
                {
                    SosContacts.Add(contact);
                }

                settings.SosContacts = contacts.Data;
            }

            IsRefreshing = false;
        }

        [RelayCommand]
        async Task AddContact()
        {
            await navigation.NavigateToAsync<SosContactAddView>();
        }

        [RelayCommand]
        async Task DeleteContact(SosContact contact)
        {
            var apiResponse = await sosContactsApi.DeleteSosContact(contact.Id);
            if (apiResponse.Message == "SOSCONTACT_DELETED")
            {
                SosContacts.Remove(contact);

                IsEmpty = SosContacts.Count == 0;
            }
        }
        [RelayCommand]
        async Task UpdateContact(SosContact contact)
        {
            //contact into a dictionary
            var parameters = new Dictionary<string, object>
            {
                { "contact", contact }
            };

            await navigation.NavigateToAsync<SosContactAddView>(parameters);
        }

        //call contact
        [RelayCommand]
        async Task CallContact(SosContact contact)
        {
            if (PhoneDialer.Default.IsSupported)
            {
                PhoneDialer.Default.Open(contact.Phone);
            }
        }
    }

}
