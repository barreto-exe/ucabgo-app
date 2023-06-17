using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Api.Tools;
using UcabGo.App.Services;
using Location = UcabGo.App.Models.Location;

namespace UcabGo.App.Api.Services.Destinations
{
    public class DestinationsService : BaseRestJsonApi, IDestinationsService
    {
        public DestinationsService(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<IEnumerable<Location>>> GetDriverDestinations()
        {
            return await GetAsync<IEnumerable<Location>>(ApiRoutes.DESTINATIONS);
        }
        public async Task<ApiResponse<Location>> AddDriverDestination(Location location)
        {
            return await PostAsync<Location>(ApiRoutes.DESTINATIONS, location);
        }
        public async Task<ApiResponse<Location>> UpdateDriverDestination(Location location)
        {
            return await PutAsync<Location>(ApiRoutes.DESTINATIONS, location);
        }
        public async Task<ApiResponse<Location>> DeleteDriverDestination(Location location)
        {
            return await DeleteAsync<Location>(ApiRoutes.DESTINATIONS + "/" + location.Id);
        }
    }
}
