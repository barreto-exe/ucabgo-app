using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.Chat
{
    public class ChatApi : BaseRestJsonApi, IChatApi
    {
        public ChatApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<IEnumerable<ChatMessage>>> GetMessages(int rideId)
        {
            return await GetAsync<IEnumerable<ChatMessage>>($"{ApiRoutes.RIDES}/{rideId}/chat");
        }

        public async Task<ApiResponse<ChatMessage>> SendMessage(int rideId, string content)
        {
            return await PostAsync<ChatMessage>($"{ApiRoutes.RIDES}/{rideId}/chat", new { content });
        }
    }
}
