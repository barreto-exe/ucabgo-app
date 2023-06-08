using UcabGo.App.Api.Models;
using UcabGo.App.Api.Tools;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.Phone
{
    public class PhoneApi : BaseRestJsonApi, IPhoneApi
    {
        public PhoneApi(ISettingsService settingsService) : base(settingsService)
        {
        }

        public async Task<ApiResponse<User>> ChangePhoneAsync(string phone)
        {
            var input = new
            {
                Phone = phone
            };
            return await PutAsync<User>(ApiRoutes.CHANGE_PHONE, input);
        }
    }
}
