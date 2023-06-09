using Newtonsoft.Json;
using System.Net;
using UcabGo.App.Api.Tools;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.User
{
    public class UserApi : BaseRestJsonApi, IUserApi
    {
        public UserApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<Models.User>> ChangePersonalInfoAsync(string name, string lastName, string phone)
        {
            var input = new
            {
                Name = name,
                LastName = lastName,
                Phone = phone
            };
            return await PutAsync<Models.User>(ApiRoutes.USER, input);
        }

        public async Task<ApiResponse<Models.User>> ChangeWalkingDistanceAsync(int walkingDistance)
        {
            var input = new
            {
                WalkingDistance = walkingDistance
            };
            return await PutAsync<Models.User>(ApiRoutes.WALKING_DISTANCE, input);
        }

        public async Task<ApiResponse<Models.User>> UpdateProfilePicture(MultipartFormDataContent input)
        {
            RefreshToken();
            var response = await Client.PutAsync(ApiRoutes.USER_PICTURE, input);
            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Request was successful
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponse<Models.User>>(content);
                return result;
            }
            else
            {
                throw new Exception("Error updating profile picture");
            }
        }
    }
}
