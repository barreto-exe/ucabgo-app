using UcabGo.App.Api.Tools;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.User
{
    public class UserApi : BaseRestJsonApi, IUserApi
    {
        public UserApi(ISettingsService settingsService) : base(settingsService)
        {
        }

        public async Task<ApiResponse<Models.User>> ChangePhoneAsync(string phone)
        {
            var input = new
            {
                Phone = phone
            };
            return await PutAsync<Models.User>(ApiRoutes.CHANGE_PHONE, input);
        }

        public async Task<ApiResponse<Models.User>> ChangeWalkingDistanceAsync(int walkingDistance)
        {
            var input = new
            {
                WalkingDistance = walkingDistance
            };
            return await PutAsync<Models.User>(ApiRoutes.WALKING_DISTANCE, input);
        }
    }
}
