using UcabGo.App.Api.Tools;
using Location = UcabGo.App.Models.Location;

namespace UcabGo.App.Api.Services.Locations
{
    public interface ILocationsApiService
    {
        Task<ApiResponse<Location>> PostUserHome(Location location);
        Task<ApiResponse<Location>> GetUserHome();
        Task<ApiResponse<IEnumerable<Location>>> GetUserDefaultLocations();
    }
}
