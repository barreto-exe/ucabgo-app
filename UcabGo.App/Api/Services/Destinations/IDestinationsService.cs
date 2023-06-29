using UcabGo.App.Api.Tools;
using Location = UcabGo.App.Models.Location;

namespace UcabGo.App.Api.Services.Destinations
{
    public interface IDestinationsService
    {
        Task<ApiResponse<IEnumerable<Location>>> GetDestinations();
        Task<ApiResponse<Location>> AddDestination(Location location);
        Task<ApiResponse<Location>> DeleteDestination(Location location);
    }
}
