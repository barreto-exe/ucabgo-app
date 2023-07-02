namespace UcabGo.App.Api.Services.PassengerService.Inputs
{
    public class PassengerInput
    {
        public int Ride { get; set; }
        public int FinalLocation { get; set; }
        public double LatitudeOrigin { get; set; }
        public double LongitudeOrigin { get; set; }
    }
}