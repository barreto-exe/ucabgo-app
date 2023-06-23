using UcabGo.App.Api.Tools;
using UcabGo.App.Models;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.Driver
{
    public class DriverApi : BaseRestJsonApi, IDriverApi
    {
        public DriverApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<List<Ride>>> GetRides(bool onlyAvailable)
        {
            var query = new
            {
                OnlyAvailable = onlyAvailable,
            };

            return await GetAsync<List<Ride>>($"{ApiRoutes.DRIVER}/rides", query);
        }
        public async Task<ApiResponse<Ride>> CreateRide(RideCreateInput rideInput)
        {
            return await PostAsync<Ride>($"{ApiRoutes.DRIVER}/rides/create", rideInput);
        }
        public async Task<ApiResponse<Ride>> StartRide(int id)
        {
            var input = new
            {
                Id = id,
            };
            return await PutAsync<Ride>($"{ApiRoutes.DRIVER}/rides/start", input);
        }
        public async Task<ApiResponse<Ride>> CompleteRide(int id)
        {
            var input = new
            {
                Id = id,
            };
            return await PutAsync<Ride>($"{ApiRoutes.DRIVER}/rides/complete", input);
        }
        public Task<ApiResponse<Ride>> CancelRide(int id)
        {
            var input = new
            {
                Id = id,
            };
            return PutAsync<Ride>($"{ApiRoutes.DRIVER}/rides/cancel", input);
        }
        public async Task<ApiResponse<List<Passenger>>> GetPassengers(int rideId)
        {
            return await GetAsync<List<Passenger>>($"{ApiRoutes.DRIVER}/{rideId}/passengers");
        }
        public async Task<ApiResponse<Ride>> AcceptPassenger(int rideId, int passengerId)
        {
            return await PutAsync<Ride>($"{ApiRoutes.DRIVER}/{rideId}/passengers/{passengerId}/accept");
        }
        public Task<ApiResponse<Ride>> IgnorePassenger(int rideId, int passengerId)
        {
            return PutAsync<Ride>($"{ApiRoutes.DRIVER}/{rideId}/passengers/{passengerId}/ignore");
        }
        public Task<ApiResponse<Ride>> CancelPassenger(int rideId, int passengerId)
        {
            return PutAsync<Ride>($"{ApiRoutes.DRIVER}/{rideId}/passengers/{passengerId}/cancel");
        }
    }
}
