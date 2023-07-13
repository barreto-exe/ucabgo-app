using UcabGo.App.Api.Services.UserRating.Dtos;
using UcabGo.App.Api.Services.UserRating.Inputs;
using UcabGo.App.Api.Tools;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.UserRating
{
    public class UserRatingApi : BaseRestJsonApi, IUserRatingApi
    {
        public UserRatingApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<double>> GetAverageRating()
        {
            return await GetAsync<double>(ApiRoutes.STARS_AVERAGE);
        }

        public async Task<ApiResponse<RatingDto>> PostRating(RatingInput input)
        {
            var result = await PostAsync<RatingDto>(ApiRoutes.STARS, input);
            return result;
        }
    }
}
