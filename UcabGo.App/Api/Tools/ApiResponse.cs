namespace UcabGo.App.Api.Tools
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
