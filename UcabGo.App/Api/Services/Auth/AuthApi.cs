using UcabGo.App.Api.Interfaces;
using UcabGo.App.Api.Models;
using UcabGo.App.Api.Tools;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services
{
    public class AuthApi : BaseRestJsonApi, IAuthApi
    {
        public AuthApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<object>> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            var input = new
            {
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            return await PutAsync<object>(ApiRoutes.CHANGE_PASSWORD, input);
        }

        public async Task<ApiResponse<Login>> LoginAsync(string email, string password)
        {
            var login = new
            {
                Email = email,
                Password = password
            };

            return await PostAsync<Login>(ApiRoutes.LOGIN, login);
        }
    }
}
