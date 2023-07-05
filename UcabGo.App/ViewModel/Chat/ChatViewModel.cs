using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.Chat;
using UcabGo.App.Api.Services.SignalR;
using UcabGo.App.Models;
using UcabGo.App.Services;

namespace UcabGo.App.ViewModel
{
    [QueryProperty(nameof(RideId), "rideId")]
    public partial class ChatViewModel : ViewModelBase
    {
        readonly IHubConnectionFactory hubConnectionFactory;
        readonly IChatApi chatApi;

        [ObservableProperty]
        int rideId;

        [ObservableProperty]
        string messageText;

        [ObservableProperty]
        ObservableCollection<ChatMessage> messages;

        public ScrollView ScrollView { get; set; }

        public ChatViewModel(ISettingsService settingsService, INavigationService navigation, IHubConnectionFactory hubConnectionFactory, IChatApi chatApi) : base(settingsService, navigation)
        {
            this.hubConnectionFactory = hubConnectionFactory;
            this.chatApi = chatApi;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await RefreshMessages();
        }

        [RelayCommand]
        public async Task SendMessage()
        {
            MessageText = MessageText.Trim();
            if (string.IsNullOrEmpty(MessageText))
            {
                return;
            }

            var response = await chatApi.SendMessage(RideId, MessageText);
            if (response?.Message == "MESSAGE_SENT")
            {
                MessageText = string.Empty;
                await RefreshMessages();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo enviar el mensaje.", "Aceptar");
            }
        }

        async Task RefreshMessages()
        {
            var response = await chatApi.GetMessages(RideId);
            if(response?.Message == "MESSAGES_FOUND")
            {
                Messages = new(response.Data);

                await ScrollView.ScrollToAsync(0, ScrollView.Content.Height, true);
                ScrollView.Focus();
            }
            else if (response?.Message == "CHAT_NOT_FOUND")
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El chat no existe.", "Aceptar");

                await navigation.GoBackAsync();
            }
        }
    }
}
