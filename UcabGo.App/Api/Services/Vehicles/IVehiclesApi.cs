using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Models;

namespace UcabGo.App.Api.Services.Vehicles
{
    public interface IVehiclesApi
    {
        Task<List<Vehicle>> GetVehicles();
        Task<Vehicle> GetVehicle(int id);
        Task<Vehicle> AddVehicle(Vehicle vehicle);
        Task<Vehicle> UpdateVehicle(Vehicle vehicle);
        Task<Vehicle> DeleteVehicle(int id);
    }
}
