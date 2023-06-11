namespace UcabGo.App.Api.Tools
{
    public static class ApiRoutes
    {
        public static string BASE_URL = "";
        public static string LOGIN { get => BASE_URL + "auth/login"; }
        public static string REGISTER { get => BASE_URL + "auth/register"; }
        public static string CHANGE_PASSWORD { get => BASE_URL + "auth/changepassword"; }
        public static string CHANGE_PHONE { get => BASE_URL + "user/phone"; }
        public static string USER_VEHICLES { get => BASE_URL + "user/vehicles"; }
        public static string WALKING_DISTANCE { get => BASE_URL + "user/walking-distance"; }
        public static string SOS_CONTACTS { get => BASE_URL + "user/sos-contacts"; }
    }
}
