using UcabGo.App.Services;

namespace UcabGo.App.Api.Services.Rides
{
    public class RidesApi : BaseRestJsonApi, IRidesApi
    {
        public RidesApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
        {
        }
    }
}
