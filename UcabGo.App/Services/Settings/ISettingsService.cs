using UcabGo.App.Api.Models;

namespace UcabGo.App.Services
{
    public interface ISettingsService
    {
        string AccessToken { get; set; }
        User User { get; set; }

        protected T GetValueOrDefault<T>(string key, T defaultValue);
        protected void SetValue(string key, object value);
    }
}
