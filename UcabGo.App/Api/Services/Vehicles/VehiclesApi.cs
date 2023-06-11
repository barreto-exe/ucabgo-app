using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Models;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.Vehicles
{
    public class VehiclesApi : BaseRestJsonApi, IVehiclesApi
    {
        public VehiclesApi(ISettingsService settingsService) : base(settingsService)
        {
        }


        public Task<List<Vehicle>> GetVehicles()
        {
            throw new NotImplementedException();
        }
        public Task<Vehicle> GetVehicle(int id)
        {
            throw new NotImplementedException();
        }
        public Task<Vehicle> AddVehicle(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }
        public Task<Vehicle> UpdateVehicle(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }
        public Task<Vehicle> DeleteVehicle(int id)
        {
            throw new NotImplementedException();
        }
    }
}
