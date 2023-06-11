using UcabGo.App.Api.Models;
using UcabGo.App.Api.Tools;

namespace UcabGo.App.Api.Services.User
{
    public interface IUserApi
    {
        Task<ApiResponse<Models.User>> ChangePhoneAsync(string phone);
        Task<ApiResponse<Models.User>> ChangeWalkingDistanceAsync(int walkingDistance);
    }
}
