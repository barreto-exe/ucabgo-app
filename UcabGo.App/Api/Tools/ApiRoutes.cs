using UcabGo.App.Utils;

namespace UcabGo.App.Api.Tools
{
    public static class ApiRoutes
    {
        public static readonly string BASE_URL = EnviromentVariables.GetValue("ApiUrl");
        public static readonly string GOOGLE_MAPS_PLACES_URL = "https://maps.googleapis.com/maps/api/place/textsearch/json";
        public static readonly string GOOGLE_MAPS_GEOCODE_URL = "https://maps.googleapis.com/maps/api/geocode/json";
        public static string LOGIN { get => BASE_URL + "auth/login"; }
        public static string REGISTER { get => BASE_URL + "auth/register"; }
        public static string CHANGE_PASSWORD { get => BASE_URL + "auth/changepassword"; }
        public static string CHANGE_PHONE { get => BASE_URL + "user/phone"; }
        public static string USER_VEHICLES { get => BASE_URL + "user/vehicles"; }
        public static string WALKING_DISTANCE { get => BASE_URL + "user/walking-distance"; }
        public static string SOS_CONTACTS { get => BASE_URL + "user/sos-contacts"; }
        public static string DEFAULT_LOCATIONS { get => BASE_URL + "user/default-locations"; }
        public static string HOME { get => BASE_URL + "user/home"; }
        public static string DESTINATIONS { get => BASE_URL + "user/destinations"; }
        public static string DRIVER { get => BASE_URL + "driver"; }
        public static string RIDES_MATCHING { get => BASE_URL + "rides/matching"; }
    }
}
