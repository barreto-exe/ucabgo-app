using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using Location = UcabGo.App.Models.Location;

namespace UcabGo.App.Api.Services.Rides
{
    public interface IRidesApi
    {
        Task<ApiResponse<IEnumerable<RideMatching>>> GetMatchingRides(Location finalDestination, int walkingDistance, bool goingToCampus);
    }
}
