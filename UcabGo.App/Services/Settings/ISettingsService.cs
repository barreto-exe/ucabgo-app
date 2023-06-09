﻿using UcabGo.App.Api.Models;
using UcabGo.App.Models;
using Location = UcabGo.App.Models.Location;

namespace UcabGo.App.Services
{
    public interface ISettingsService
    {
        string AccessToken { get; set; }
        User User { get; set; }
        bool IsLoggedIn { get; }
        Location Home { get; set; }
        Location Campus { get; set; }
        IEnumerable<Vehicle> Vehicles { get; set; }
        IEnumerable<SosContact> SosContacts { get; set; }
        bool ReloadImage { get; set; }

        protected T GetValueOrDefault<T>(string key, T defaultValue);
        protected void SetValue(string key, object value);
    }
}
