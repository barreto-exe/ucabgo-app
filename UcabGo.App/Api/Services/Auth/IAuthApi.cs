using UcabGo.App.Api.Models;
using UcabGo.App.Api.Tools;
using UcabGo.App.Models.Inputs;

namespace UcabGo.App.Api.Interfaces
{
    public interface IAuthApi
    {
        Task<ApiResponse<Login>> LoginAsync(string email, string password);
        Task<ApiResponse<Login>> RegisterAsync(RegisterInput input);
        Task<ApiResponse<object>> ChangePasswordAsync(string oldPassword, string newPassword);
    }
}
