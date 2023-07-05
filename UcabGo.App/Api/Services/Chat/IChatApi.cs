using UcabGo.App.Api.Tools;
using UcabGo.App.Models;

namespace UcabGo.App.Api.Services.Chat
{
    public interface IChatApi
    {
        Task<ApiResponse<IEnumerable<ChatMessage>>> GetMessages(int rideId);
        Task<ApiResponse<ChatMessage>> SendMessage(int rideId, string content);
    }
}
