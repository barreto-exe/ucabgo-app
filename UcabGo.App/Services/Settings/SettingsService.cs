using Newtonsoft.Json;
using UcabGo.App.Api.Models;
using Location = UcabGo.App.Models.Location;

namespace UcabGo.App.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        //Default values for settings
        private readonly string AccessTokenDefault = string.Empty;
        private readonly User UserDefault = null;
        private readonly Location HomeDefault = null;
        private readonly Location CampusDefault = null;

        //Getters and setters for settings
        public string AccessToken
        {
            get => GetValueOrDefault(nameof(AccessToken), AccessTokenDefault);
            set => SetValue(nameof(AccessToken), value);
        }
        public User User
        {
            get => GetValueOrDefault(nameof(User), UserDefault);
            set => SetValue(nameof(User), value);
        }
        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(AccessToken);
        public Location Home
        {
            get => GetValueOrDefault(nameof(Home), HomeDefault);
            set => SetValue(nameof(Home), value);
        }
        public Location Campus
        {
            get => GetValueOrDefault(nameof(Campus), CampusDefault);
            set => SetValue(nameof(Campus), value);
        }

        //Methods for settings
        public void SetValue(string key, object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                Preferences.Remove(key);
            }
            else
            {
                var json = JsonConvert.SerializeObject(value);
                Preferences.Set(key, json);
            }
        }
        public T GetValueOrDefault<T>(string key, T defaultValue)
        {
            var json = Preferences.Get(key, string.Empty);
            return json == string.Empty ? defaultValue : JsonConvert.DeserializeObject<T>(json);
        }
    }
}
