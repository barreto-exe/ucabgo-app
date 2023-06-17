using Newtonsoft.Json;
using UcabGo.App.Api.Tools;
using UcabGo.App.Services;
using UcabGo.App.Utils;

namespace UcabGo.App.Api.Services.GoogleMaps;

public class GoogleMapsApi : BaseRestJsonApi, IGoogleMapsApi
{
    public GoogleMapsApi(ISettingsService settingsService, INavigationService navigationService) : base(settingsService, navigationService)
    {
    }

    public async Task<GeocodeDto> GetGeocode(double latitude, double longitude)
    {
        dynamic input = new
        {
            latlng = $"{latitude},{longitude}",
            language = "es",
            key = EnviromentVariables.GetValue("GoogleMapsApiKey"),
        };
        dynamic response = await GeneralGetAsync(ApiRoutes.GOOGLE_MAPS_GEOCODE_URL, input);

        if(response.ToString().Contains("REQUEST_DENIED"))
        {
            return null;
        }

        var excludedComponents = new List<string>
        {
            "country",
            "administrative_area_level_1",
            "administrative_area_level_2",
            "locality",
            "postal_code"
        };

        List<dynamic> allAddressComponents = new();
        foreach (var result in response.results)
        {
            foreach (var component in result.address_components)
            {
                allAddressComponents.Add(component);
            }
        }

        var resultComponents = new List<string>();
        foreach (var component in allAddressComponents)
        {
            string json = component.types.ToString();
            string[] types = JsonConvert.DeserializeObject<string[]>(json);

            string name = component.long_name.ToString();

            if (types.Any(t => excludedComponents.Contains(t)) || 
                name.Length <= 3 || 
                name.Contains('+')) continue;

            resultComponents.Add(component.long_name.ToString());
        }

        return new()
        {
            Components = resultComponents.Distinct().OrderBy(x => x).ToList(),
        };
    }

    public async Task<IEnumerable<PlaceDto>> GetPlaces(GooglePlaceFilter filter)
    {
        dynamic response = await GeneralGetAsync(ApiRoutes.GOOGLE_MAPS_PLACES_URL, filter);

        List<PlaceDto> places = new();
        foreach (var result in response.results)
        {
            PlaceDto place = new()
            {
                Name = result.name,
                Address = result.formatted_address,
                Latitude = result.geometry.location.lat,
                Longitude = result.geometry.location.lng
            };
            places.Add(place);
        }

        return places;
    }
}
