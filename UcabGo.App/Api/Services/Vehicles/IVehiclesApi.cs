using UcabGo.App.Api.Tools;
using UcabGo.App.Models;

namespace UcabGo.App.Api.Services.Vehicles
{
    public interface IVehiclesApi
    {
        Task<ApiResponse<List<Vehicle>>> GetVehicles();
        Task<ApiResponse<Vehicle>> AddVehicle(Vehicle vehicle);
        Task<ApiResponse<Vehicle>> UpdateVehicle(Vehicle vehicle);
        Task<ApiResponse<Vehicle>> DeleteVehicle(int id);
    }
}
