using UcabGo.App.Api.Services.UserRating.Dtos;
using UcabGo.App.Api.Services.UserRating.Inputs;
using UcabGo.App.Api.Tools;

namespace UcabGo.App.Api.Services.UserRating
{
    public interface IUserRatingApi
    {
        Task<ApiResponse<RatingDto>> PostRating(RatingInput input);
        Task<ApiResponse<double>> GetAverageRating();
    }
}
