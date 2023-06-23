namespace UcabGo.App.Api.Services.Driver
{
    public class RideCreateInput
    {
        public int Vehicle { get; set; }
        public int Destination { get; set; }
        public int SeatQuantity { get; set; }
        public double LatitudeOrigin { get; set; }
        public double LongitudeOrigin { get; set; }
    }
}
