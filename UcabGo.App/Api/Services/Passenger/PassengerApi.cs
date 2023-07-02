using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Api.Services.PassengerService;
using UcabGo.App.Api.Services.PassengerService.Inputs;
using UcabGo.App.Api.Tools;
using UcabGo.App.Services;
using PassengerModel = UcabGo.App.Models.Passenger;

namespace UcabGo.App.Api.Services.Passenger
{
    public class PassengerApi : BaseRestJsonApi, IPassengerApi
    {
        public PassengerApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }

        public async Task<ApiResponse<PassengerModel>> AskForRide(PassengerInput input)
        {
            return await PostAsync<PassengerModel>(ApiRoutes.PASSENGER + "/rides/ask", input);
        }

        public async Task<ApiResponse<PassengerModel>> CancelRide(int rideId)
        {
            return await PostAsync<PassengerModel>(ApiRoutes.PASSENGER + "/rides/cancel", new { rideId });
        }

        public async Task<ApiResponse<PassengerModel>> FinishRide(int rideId)
        {
            return await PostAsync<PassengerModel>(ApiRoutes.PASSENGER + "/rides/finish", new { rideId });
        }
    }
}
