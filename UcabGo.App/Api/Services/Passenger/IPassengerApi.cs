using UcabGo.App.Api.Services.PassengerService.Inputs;
using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using PassengerModel = UcabGo.App.Models.Passenger;

namespace UcabGo.App.Api.Services.PassengerService
{
    public interface IPassengerApi
    {
        Task<ApiResponse<PassengerModel>> AskForRide(PassengerInput input);
        Task<ApiResponse<IEnumerable<Ride>>> GetRides(bool onlyAvailable);
        Task<ApiResponse<PassengerModel>> CancelRide(int rideId);
        Task<ApiResponse<PassengerModel>> FinishRide(int rideId);
    }
}
