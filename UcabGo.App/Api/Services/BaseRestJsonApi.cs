using Newtonsoft.Json;
using System.Net;
using System.Text;
using UcabGo.App.Api.Tools;

namespace UcabGo.App.Api.Services
{
    public abstract class BaseRestJsonApi
    {
        readonly HttpClient client;
        readonly JsonSerializerSettings serializerSettings;

        public BaseRestJsonApi()
        {
            client = new HttpClient();
            serializerSettings = new()
            {
                DateFormatString = "yyyy-MM-dd hh:mm:ss",
                Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US"),
            };
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
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(content, serializerSettings);
                }
                else
                {
                    ToastMessage(response.StatusCode);
                    return default;
                }

            }
            catch (HttpRequestException ex)
            {
                ToastMessage();
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
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(responseString, serializerSettings);
                }
                else
                {
                    ToastMessage(response.StatusCode);
                    return default;
                }
            }
            catch (HttpRequestException ex)
            {
                ToastMessage();
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
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(responseString, serializerSettings);
                }
                else
                {
                    ToastMessage(response.StatusCode);
                    return default;
                }
            }
            catch (HttpRequestException ex)
            {
                ToastMessage();
                return default;
            }
        }

        protected async Task<ApiResponse<T>> DeleteAsync<T>(string url)
        {
            try
            {
                var response = await client.DeleteAsync(new Uri(url));
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(responseString, serializerSettings);
                }
                else
                {
                    ToastMessage(response.StatusCode);
                    return default;
                }
            }
            catch (HttpRequestException ex)
            {
                ToastMessage();
                return default;
            }
        }

        private static void ToastMessage()
        {
            //Toast error message: "No se pudo conectar con el servidor"
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
    }
}
