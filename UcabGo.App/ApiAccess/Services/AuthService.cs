using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UcabGo.App.ApiAccess.Models;
using UcabGo.App.ApiAccess.Tools;

namespace UcabGo.App.ApiAccess.Services
{
    public class AuthService
    {
        HttpClient client;
        JsonSerializerSettings serializerSettings;

        public AuthService()
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

            var responseHttp = await client.PostAsync(new Uri(Routes.LOGIN), content);

            var responseString = await responseHttp.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ApiResponse<Login>>(responseString, serializerSettings);

            return response.Data;
        }
    }
}
