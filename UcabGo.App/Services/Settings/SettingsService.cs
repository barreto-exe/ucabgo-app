using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UcabGo.App.Api.Models;

namespace UcabGo.App.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        //Default values for settings
        private readonly string AccessTokenDefault = string.Empty;
        private readonly User UserDefault = null;

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

        //Methods for settings
        public void SetValue(string key, object value)
        {
            if(value == null || string.IsNullOrWhiteSpace(value.ToString()))
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
