using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Services.Chat;
using UcabGo.App.Api.Services.SignalR;
using UcabGo.App.Models;
using UcabGo.App.Services;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Core.Platform;
using UcabGo.App.Api.Tools;
using System.Diagnostics;

namespace UcabGo.App.ViewModel
{
    [QueryProperty(nameof(RideId), "rideId")]
    public partial class ChatViewModel : ViewModelBase
    {
        readonly IChatApi chatApi;

        
        readonly IHubConnectionFactory hubConnectionFactory;
        HubConnection hubConnection;
        CancellationTokenSource tokenSource;

        [ObservableProperty]
        int rideId;

        [ObservableProperty]
        string messageText;

        [ObservableProperty]
        ObservableCollection<ChatMessage> messages;

        [ObservableProperty]
        bool isLoading;

        [ObservableProperty]
        ObservableCollection<ChatMessageOption> messageOptions;
        
        public CollectionView CollectionView { get; set; }


        public ChatViewModel(ISettingsService settingsService, INavigationService navigation, IHubConnectionFactory hubConnectionFactory, IChatApi chatApi) : base(settingsService, navigation)
        {
            this.chatApi = chatApi;

            this.hubConnectionFactory = hubConnectionFactory;

            this.messageOptions = new(ChatMessageOption.GetChatMessageOptions());
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            Messages = new();

            IsLoading = true;

            await RefreshMessages();

            IsLoading = false;

            await RunHubConnection();
        }

        private async Task RunHubConnection()
        {
            hubConnection = hubConnectionFactory.GetHubConnection(ApiRoutes.CHAT_HUB);
            hubConnection.On<int>(ApiRoutes.CHAT_RECEIVE_MESSAGE, async (rideId) =>
            {
                if (rideId == RideId)
                {
                    await RefreshMessages();
                }
            });

            tokenSource = new();
            try
            {
                if (hubConnection.State == HubConnectionState.Disconnected)
                {
                    await hubConnection.StartAsync(tokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(hubConnection.State.ToString() + ex.Message);
                await RunHubConnection();
            }
        }

        public override async void OnDisappearing()
        {
            base.OnDisappearing();

            tokenSource.Cancel();
            await hubConnection.StopAsync();
        }

        [RelayCommand]
        public async Task SelectMessage(ChatMessageOption option)
        {
            var options = option.FinalPortions
                .Select(x => $"📨 {x}")
                .ToArray();

            var finalPortion = await Application.Current.MainPage
                .DisplayActionSheet("Completa el mensaje", "Cancelar", null, options);

            if (finalPortion != "Cancelar" && !string.IsNullOrWhiteSpace(finalPortion))
            {
                finalPortion = finalPortion.Replace("📨", "").Trim();
                
                if(option.FirstPortion == string.Empty)
                {
                    await SendMessage(finalPortion);
                }
                else
                {
                    //Replace emoji and set to lower only two first characters
                    char firstChar = finalPortion[0].ToString().ToLower()[0];
                    char secondChar = finalPortion[1].ToString().ToLower()[0];
                    finalPortion = $"{firstChar}{secondChar}{finalPortion[2..]}";

                    await SendMessage($"{option.FirstPortion} {finalPortion}");
                }
            }
        }

        public async Task SendMessage(string message)
        {
            var response = await chatApi.SendMessage(RideId, message);
            if (response?.Message == "MESSAGE_SENT")
            {
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
                //Add only messages that are not already in the list 
                foreach (var message in response.Data)
                {
                    if (!Messages.Any(m => m.Id == message.Id))
                    {
                        Messages.Add(message);
                    }
                }

                int index = Messages.Count - 1;
                if (index > 0) CollectionView.ScrollTo(index, animate: true);
                
                //CollectionView.Focus();
            }
            else if (response?.Message == "CHAT_NOT_FOUND")
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El chat no existe.", "Aceptar");

                await navigation.GoBackAsync();
            }
        }
    }
}
