using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.Vehicles
{
    public class VehiclesApi : BaseRestJsonApi, IVehiclesApi
    {
        public VehiclesApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<IEnumerable<Vehicle>>> GetVehicles()
        {
            return await GetAsync<IEnumerable<Vehicle>>(ApiRoutes.USER_VEHICLES);
        }
        public async Task<ApiResponse<Vehicle>> AddVehicle(Vehicle vehicle)
        {
            return await PostAsync<Vehicle>(ApiRoutes.USER_VEHICLES, vehicle);
        }
        public async Task<ApiResponse<Vehicle>> UpdateVehicle(Vehicle vehicle)
        {
            return await PutAsync<Vehicle>(ApiRoutes.USER_VEHICLES, vehicle);
        }
        public async Task<ApiResponse<Vehicle>> DeleteVehicle(int id)
        {
            return await DeleteAsync<Vehicle>($"{ApiRoutes.USER_VEHICLES}/{id}");
        }
    }
}
