using UcabGo.App.Api.Interfaces;
using UcabGo.App.Api.Models;
using UcabGo.App.Api.Tools;

namespace UcabGo.App.Api.Services
{
    public class AuthApi : BaseRestJsonApi, IAuthApi
    {
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
