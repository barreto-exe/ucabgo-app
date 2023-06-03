using Newtonsoft.Json;
using System.Text;
using UcabGo.App.Api.Interfaces;
using UcabGo.App.Api.Models;
using UcabGo.App.Api.Tools;

namespace UcabGo.App.Api.Services
{
    public class AuthApi : IAuthApi
    {
        HttpClient client;
        JsonSerializerSettings serializerSettings;

        public AuthApi()
        {
            client = new HttpClient();
            serializerSettings = new()
            {
                DateFormatString = "yyyy-MM-dd hh:mm:ss",
                Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US"),
            };
        }

        public async Task<Login> LoginAsync(string email, string password)
        {
            var login = new
            {
                Email = email,
                Password = password
            };
            var json = JsonConvert.SerializeObject(login, serializerSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var responseHttp = await client.PostAsync(new Uri(ApiRoutes.LOGIN), content);

            var responseString = await responseHttp.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ApiResponse<Login>>(responseString, serializerSettings);

            return response.Data;
        }
    }
}
