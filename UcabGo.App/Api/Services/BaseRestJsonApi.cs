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
        readonly INavigationService navigationService;

        public BaseRestJsonApi(ISettingsService settingsService, INavigationService navigationService)
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

            this.navigationService = navigationService;
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
            catch
            {
                await ToastNoInternetMessage();
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
            catch
            {
                await ToastNoInternetMessage();
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
            catch 
            {
                await ToastNoInternetMessage();
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
            catch 
            {
                await ToastNoInternetMessage();
                return default;
            }
        }
        private async Task<ApiResponse<T>> GeneralResponse<T>(HttpResponseMessage response)
        {
            string responseString = await response.Content.ReadAsStringAsync();

            if (responseString.Contains("INVALID_INPUT"))
            {
                await ToastInvalidInput();
                return new ApiResponse<T>()
                {
                    Data = default,
                    Message = "INVALID_INPUT",
                };
            }
            else if (!response.IsSuccessStatusCode)
            {
                await ToastMessage(response.StatusCode);
            }


            return JsonConvert.DeserializeObject<ApiResponse<T>>(responseString, serializerSettings);
        }

        private async Task ToastNoInternetMessage()
        {
            var page = App.Current.MainPage;
            await page.DisplayAlert("Error", "Parece que no tienes conexión a internet.", "Ok");

            settingsService.AccessToken = string.Empty;
            await navigationService.RestartSession();
        }
        private async Task ToastInvalidInput()
        {
            var page = App.Current.MainPage;
            await page.DisplayAlert("Error", "Los datos ingresados no son válidos", "Ok");
        }
        private async Task ToastMessage(HttpStatusCode code)
        {
            var page = App.Current.MainPage;

            switch (code)
            {
                case HttpStatusCode.Unauthorized:
                    await page.DisplayAlert(
                        "Error", 
                        "Tu sesión ha caducado", 
                        "Ok");

                    settingsService.AccessToken = string.Empty;
                    await navigationService.RestartSession();
                    break;

                case HttpStatusCode.Forbidden:
                    await page.DisplayAlert(
                        "Error", 
                        "No tienes permiso para realizar esta acción", 
                        "Ok");
                    break;

                case HttpStatusCode.NotFound:
                    await page.DisplayAlert(
                        "Error", 
                        "No se encontró el recurso solicitado", 
                        "Ok");
                    break;

                case HttpStatusCode.InternalServerError:
                    await page.DisplayAlert(
                        "Error", 
                        "Ha ocurrido un error en el servidor. No es tu culpa, intentaremos solucionarlo en el menor tiempo posible.",
                        "Ok");

                    break;

                case
                HttpStatusCode.ServiceUnavailable:
                    await page.DisplayAlert(
                        "Error", 
                        "El servidor no está disponible en este momento. Intenta más tarde.", 
                        "Ok");
                    break;

                default:
                    await page.DisplayAlert(
                        "Error", 
                        "Ha ocurrido un error inesperado. No es tu culpa, intentaremos solucionarlo en el menor tiempo posible.", 
                        "Ok");
                    break;
            }
        }
    }
}
