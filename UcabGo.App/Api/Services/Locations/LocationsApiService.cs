using UcabGo.App.Api.Tools;
using UcabGo.App.Services;
using Location = UcabGo.App.Models.Location;

namespace UcabGo.App.Api.Services.Locations
{
    public class LocationsApiService : BaseRestJsonApi, ILocationsApiService
    {
        public LocationsApiService(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<Location>> GetUserHome()
        {
            return await GetAsync<Location>(ApiRoutes.HOME);
        }

        public async Task<ApiResponse<IEnumerable<Location>>> GetUserLocations()
        {
            return await GetAsync<IEnumerable<Location>>(ApiRoutes.LOCATIONS);
        }

        public async Task<ApiResponse<Location>> PostUserHome(Location location)
        {
            return await PostAsync<Location>(ApiRoutes.HOME, location);
        }
    }
}
