using UcabGo.App.Api.Tools;

namespace UcabGo.App.Api.Services.User
{
    public interface IUserApi
    {
        Task<ApiResponse<Models.User>> ChangePersonalInfoAsync(string name, string lastName, string phone);
        Task<ApiResponse<Models.User>> ChangeWalkingDistanceAsync(int walkingDistance);
        Task<ApiResponse<Models.User>> UpdateProfilePicture(MultipartFormDataContent input);
    }
}
