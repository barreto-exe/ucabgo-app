using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using UcabGo.App.Services;
using UcabGo.App.Utils;

namespace UcabGo.App.Api.Services.Rides
{
    public class RidesApi : BaseRestJsonApi, IRidesApi
    {
        public RidesApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<IEnumerable<RideMatching>>> GetMatchingRides(UcabGo.App.Models.Location finalDestination, int walkingDistance, bool goingToCampus)
        {
            var currentLocation = await MapHelper.GetCurrentLocation();

            var filter = new
            {
                InitialLatitude = currentLocation.Latitude,
                InitialLongitude = currentLocation.Longitude,
                FinalLatitude = finalDestination.Latitude,
                FinalLongitude = finalDestination.Longitude,
                WalkingDistance = walkingDistance,
                GoingToCampus = goingToCampus
            };
            return await GetAsync<IEnumerable<RideMatching>>(ApiRoutes.RIDES_MATCHING, filter);
        }
    }
}
