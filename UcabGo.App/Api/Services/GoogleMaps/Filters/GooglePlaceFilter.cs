using UcabGo.App.Utils;

namespace UcabGo.App.Api.Services.GoogleMaps;

public class GooglePlaceFilter
{
    public string Query { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? Radius { get; set; }
    public string Language { get; set; } = "es";
    public string Key { get; set; } = EnviromentVariables.GetValue("GoogleMapsApiKey");
}
