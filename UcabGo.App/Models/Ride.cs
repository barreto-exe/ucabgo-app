using UcabGo.App.Api.Models;

namespace UcabGo.App.Models
{
    public class Ride
    {
        public Ride()
        {
            Vehicle = new();
            Destination = new();
            Passengers = new List<Passenger>();
        }

        public int Id { get; set; }
        public User Driver { get; set; }
        public Vehicle Vehicle { get; set; }
        public Location Destination { get; set; }
        public int SeatQuantity { get; set; }
        public int AvailableSeats { get; set; }
        public string AvailableSeatsText
        {
            get => AvailableSeats switch
            {
                0 => "No hay asientos disponibles.",
                1 => "1 asiento disponible.",
                _ => $"{AvailableSeats} asientos disponibles.",
            };
        }
        public float LatitudeOrigin { get; set; }
        public float LongitudeOrigin { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime? TimeStarted { get; set; }
        public DateTime? TimeEnded { get; set; }
        public DateTime? TimeCanceled { get; set; }
        public bool IsStarted
        {
            get => TimeStarted.HasValue && !TimeEnded.HasValue && !TimeCanceled.HasValue;
        }
        public bool IsEnded
        {
            get => TimeEnded.HasValue || TimeCanceled.HasValue;
        }
        public IEnumerable<Passenger> Passengers { get; set; }
    }
}
