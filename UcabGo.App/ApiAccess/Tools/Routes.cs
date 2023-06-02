namespace UcabGo.App.ApiAccess.Tools
{
    public static class Routes
    {
        public static string BASE_URL = "";
        public static string LOGIN { get => BASE_URL + "auth/login"; }
        public static string REGISTER { get => BASE_URL + "auth/register"; }
    }
}
