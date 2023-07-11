using UcabGo.App.Api.Services.Driver.Inputs;
using UcabGo.App.Api.Services.Rides.Dtos;
using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using PassengerModel = UcabGo.App.Models.Passenger;

namespace UcabGo.App.Api.Services.Driver
{
    public interface IDriverApi
    {
        Task<ApiResponse<IEnumerable<Ride>>> GetRides(bool onlyAvailable);
        Task<ApiResponse<Ride>> CreateRide(RideCreateInput rideInput);
        Task<ApiResponse<Ride>> StartRide(int id);
        Task<ApiResponse<Ride>> CompleteRide(int id);
        Task<ApiResponse<Ride>> CancelRide(int id);
        Task<ApiResponse<IEnumerable<PassengerModel>>> GetPassengers(int rideId);
        Task<ApiResponse<Ride>> AcceptPassenger(int rideId, int passengerId);
        Task<ApiResponse<Ride>> IgnorePassenger(int rideId, int passengerId);
        Task<ApiResponse<Ride>> CancelPassenger(int rideId, int passengerId);
        Task<ApiResponse<CooldownDto>> GetCooldownTime();
    }
}
