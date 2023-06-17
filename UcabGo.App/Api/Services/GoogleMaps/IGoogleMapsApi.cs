namespace UcabGo.App.Api.Services.GoogleMaps;

public interface IGoogleMapsApi
{
    Task<IEnumerable<PlaceDto>> GetPlaces(GooglePlaceFilter filter);
    Task<GeocodeDto> GetGeocode(double latitude, double longitude);
}
