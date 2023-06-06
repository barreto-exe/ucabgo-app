namespace UcabGo.App.Api.Tools
{
    public static class ApiRoutes
    {
        public static string BASE_URL = "";
        public static string LOGIN { get => BASE_URL + "auth/login"; }
        public static string REGISTER { get => BASE_URL + "auth/register"; }
        public static string CHANGE_PASSWORD { get => BASE_URL + "auth/changepassword"; }
    }
}
