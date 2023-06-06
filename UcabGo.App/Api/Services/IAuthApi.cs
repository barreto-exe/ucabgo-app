using UcabGo.App.Api.Models;
using UcabGo.App.Api.Tools;

namespace UcabGo.App.Api.Interfaces
{
    public interface IAuthApi
    {
        Task<ApiResponse<Login>> LoginAsync(string email, string password);
    }
}
