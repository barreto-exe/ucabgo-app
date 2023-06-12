using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using UcabGo.App.Api.Tools;
using UcabGo.App.Services;

namespace UcabGo.App.Api.Services
{
    public abstract class BaseRestJsonApi
    {
        readonly HttpClient client;
        readonly JsonSerializerSettings serializerSettings;
        readonly ISettingsService settingsService;

        public BaseRestJsonApi(ISettingsService settingsService)
        {
            client = new HttpClient();
            serializerSettings = new()
            {
                DateFormatString = "yyyy-MM-dd hh:mm:ss",
                Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US"),
            };
            this.settingsService = settingsService;

            //Set token to client if token is not null
            var token = settingsService.AccessToken;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected async Task<ApiResponse<T>> GetAsync<T>(string url, object query = null)
        {
            //Add query parameters to url with object reflection
            if (query != null)
            {
                var properties = query.GetType().GetProperties();
                var queryString = new StringBuilder();
                queryString.Append('?');
                foreach (var property in properties)
                {
                    queryString.Append($"{property.Name}={property.GetValue(query)}&");
                }
                url += queryString.ToString().TrimEnd('&');
            }

            try
            {
                var response = await client.GetAsync(url);
                return await GeneralResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                ToastNoInternetMessage();
                return default;
            }
        }
        protected async Task<ApiResponse<T>> PostAsync<T>(string url, object data)
        {
            var json = JsonConvert.SerializeObject(data, serializerSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync(new Uri(url), content);
                return await GeneralResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                ToastNoInternetMessage();
                return default;
            }
        }
        protected async Task<ApiResponse<T>> PutAsync<T>(string url, object data)
        {
            var json = JsonConvert.SerializeObject(data, serializerSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PutAsync(new Uri(url), content);
                return await GeneralResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                ToastNoInternetMessage();
                return default;
            }
        }
        protected async Task<ApiResponse<T>> DeleteAsync<T>(string url)
        {
            try
            {
                var response = await client.DeleteAsync(new Uri(url));
                return await GeneralResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                ToastNoInternetMessage();
                return default;
            }
        }

        private static void ToastNoInternetMessage()
        {
            //Toast error message: "No se pudo conectar con el servidor"
        }
        private static void ToastInvalidInput()
        {

        }
        private static void ToastMessage(HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.Unauthorized:
                    //Toast error message: "No tienes permiso para realizar esta acción"
                    break;

                case HttpStatusCode.Forbidden:
                    //Toast error message: "No tienes permiso para realizar esta acción"
                    break;

                case HttpStatusCode.NotFound:
                    //Toast error message: "No se encontró el recurso"
                    break;

                case HttpStatusCode.InternalServerError:
                    //Toast error message: "Error interno del servidor"
                    break;

                case HttpStatusCode.ServiceUnavailable:
                    //Toast error message: "Servicio no disponible"
                    break;

                default:
                    //Toast error message: "Error desconocido"
                    break;
            }
        }
        private async Task<ApiResponse<T>> GeneralResponse<T>(HttpResponseMessage response)
        {
            string responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ToastMessage(response.StatusCode);
            }

            if(responseString.Contains("INVALID_INPUT"))
            {
                ToastInvalidInput();
                return new ApiResponse<T>()
                {
                    Data = default,
                    Message = "INVALID_INPUT",
                };
            }

            return JsonConvert.DeserializeObject<ApiResponse<T>>(responseString, serializerSettings);
        }
    }
}
