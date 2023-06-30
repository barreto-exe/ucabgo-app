using Maui.GoogleMaps;
using Map = Maui.GoogleMaps.Map;

namespace UcabGo.App.Utils
{
    public static class MapHelper
    {
        public readonly static Position CampusCameraPosition = new(8.299886d, -62.712464d);
        public readonly static Position CampusPinPosition = new(8.29727428d, -62.71308436d);


        static MapHelper()
        {
        }

        /// <summary>
        /// Obtains the current location of the device. If it can't, it will return the last known location. If it can't, it will return null.
        /// </summary>
        public static async Task<Location> GetCurrentLocation()
        {
            var location =
                await Geolocation.GetLocationAsync() ??
                await Geolocation.GetLastKnownLocationAsync();

            if (location == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo obtener la ubicación actual", "Aceptar");
            }
            return location;
        }
        public static void SetPinOnMap(this Map map, Pin pin)
        {
            map.Pins.Clear();
            if (pin != null)
            {
                map.Pins.Add(pin);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromKilometers(0.5)));
            }

            /* Unmerged change from project 'UcabGo.App (net7.0-ios)'
            Before:
                    }

                    public static Pin GetHomePin(Models.Location home)
            After:
                    }

                    public static Pin GetHomePin(Models.Location home)
            */
        }

        public static Pin GetHomePin(Models.Location home)
        {
            var pin = new Pin
            {
                Type = PinType.Place,
                Label = "Casa",
                Address = home.Zone,
                Position = new Position(home.Latitude, home.Longitude),
                Rotation = 0f,
                Tag = "id_home",
                IsVisible = true,
                IsDraggable = true,
                //Icon = BitmapDescriptorFactory.FromBundle("home.png"),
            };

            return pin;
        }
        public static Pin GetCampusPin()
        {
            var pin = new Pin
            {
                Type = PinType.Place,
                Label = "UCAB Guayana",
                Address = "Puerto Ordaz, Bolívar, Venezuela",
                Position = CampusPinPosition,
                Rotation = 0f,
                Tag = "id_ucab",
                IsVisible = true
                //Icon = BitmapDescriptorFactory.FromBundle("campus.png"),
            };

            return pin;
        }
        public static Pin GetCurrentPositionPin(Position position)
        {
            var pin = new Pin
            {
                Type = PinType.Place,
                Label = "Ubicación actual",
                Position = position,
                Rotation = 0f,
                Tag = "id_current",
                IsVisible = true
            };

            return pin;
        }
        public static Pin GetSelectedPositionPin(Position position)
        {
            var pin = new Pin
            {
                Type = PinType.Place,
                Label = "Ubicación seleccionada",
                Position = position,
                Rotation = 0f,
                Tag = "id_selected",
                IsVisible = true
            };

            return pin;
        }


        private static Polygon BuildCampusPolygon()
        {
            var polygon = new Polygon();
            polygon.Positions.Add(new Position(8.295203d, -62.712272d));
            polygon.Positions.Add(new Position(8.297396d, -62.709854d));
            polygon.Positions.Add(new Position(8.298067d, -62.709864d));
            polygon.Positions.Add(new Position(8.298469d, -62.709638d));
            polygon.Positions.Add(new Position(8.298696d, -62.709724d));
            polygon.Positions.Add(new Position(8.299402d, -62.710098d));
            polygon.Positions.Add(new Position(8.300754d, -62.710833d));
            polygon.Positions.Add(new Position(8.301193d, -62.709328d));
            polygon.Positions.Add(new Position(8.302388d, -62.709542d));
            polygon.Positions.Add(new Position(8.301965d, -62.710951d));
            polygon.Positions.Add(new Position(8.301633d, -62.711809d));
            polygon.Positions.Add(new Position(8.301132d, -62.712760d));
            polygon.Positions.Add(new Position(8.300564d, -62.712958d));
            polygon.Positions.Add(new Position(8.299417d, -62.713092d));
            polygon.Positions.Add(new Position(8.299067d, -62.713178d));
            polygon.Positions.Add(new Position(8.296013d, -62.713944d));

            Color color = Colors.Green;
            polygon.StrokeColor = color;
            polygon.StrokeWidth = 3f;

            polygon.FillColor = Color.FromRgba(0, 97, 37, 0.1);

            polygon.IsClickable = true;

            return polygon;
        }
        public static void DrawCampus(this Map map, bool allowPinInside = false)
        {
            var polygon = BuildCampusPolygon();

            if (!allowPinInside)
            {
                polygon.Clicked += async (sender, e) =>
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No puedes seleccionar el campus como ubicación.", "Aceptar");
                    map.Pins.Clear();
                };
            }

            map.Polygons.Add(polygon);
        }
        public static void DrawCampus(this Map map, Func<Task> action)
        {
            var polygon = BuildCampusPolygon();

            polygon.Clicked += async (sender, e) =>
            {
                await action();
            };

            map.Polygons.Add(polygon);
        }

        public static void MoveCameraToCampus(this Map map)
        {
            map.MoveToRegion(
                MapSpan.FromCenterAndRadius(CampusCameraPosition,
                Distance.FromKilometers(0.5)));
        }
    }
}
