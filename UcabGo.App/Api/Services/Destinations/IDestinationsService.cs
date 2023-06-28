using UcabGo.App.Api.Tools;
using Location = UcabGo.App.Models.Location;

namespace UcabGo.App.Api.Services.Destinations
{
    public interface IDestinationsService
    {
        Task<ApiResponse<IEnumerable<Location>>> GetDriverDestinations();
        Task<ApiResponse<Location>> AddDriverDestination(Location location);
        Task<ApiResponse<Location>> DeleteDriverDestination(Location location);
    }
}
