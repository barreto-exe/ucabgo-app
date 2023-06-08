using UcabGo.App.Api.Models;
using UcabGo.App.Api.Tools;

namespace UcabGo.App.Api.Services.Phone
{
    public interface IPhoneApi
    {
        Task<ApiResponse<User>> ChangePhoneAsync(string phone);
    }
}
