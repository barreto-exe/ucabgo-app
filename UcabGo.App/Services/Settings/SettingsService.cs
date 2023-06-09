using Newtonsoft.Json;
using UcabGo.App.Api.Models;
using UcabGo.App.Models;
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
        private readonly IEnumerable<Vehicle> VehiclesDefault = null;
        private readonly IEnumerable<SosContact> SosContactsDefault = null;

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
        public IEnumerable<Vehicle> Vehicles
        {
            get => GetValueOrDefault(nameof(Vehicles), VehiclesDefault);
            set => SetValue(nameof(Vehicles), value);
        }
        public IEnumerable<SosContact> SosContacts
        {
            get => GetValueOrDefault(nameof(SosContacts), SosContactsDefault);
            set => SetValue(nameof(SosContacts), value);
        }
        public bool ReloadImage
        {
            get => GetValueOrDefault(nameof(ReloadImage), false);
            set => SetValue(nameof(ReloadImage), value);
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
