using UcabGo.App.Api.Models;

namespace UcabGo.App.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Initial { get => User?.Name[0].ToString().ToUpper(); }
        public DateTime TimeSolicited { get; set; }
        public DateTime? TimeAccepted { get; set; }
        public DateTime? TimeIgnored { get; set; }
        public DateTime? TimeCancelled { get; set; }
        public DateTime? TimeFinished { get; set; }

        public bool IsWaiting { get => TimeAccepted == null; }
        public bool IsAccepted { get => TimeAccepted != null; }
        public bool IsEnded { get => TimeIgnored != null || TimeCancelled != null || TimeFinished != null; }
        public bool IsActive { get => TimeIgnored == null && TimeCancelled == null && TimeFinished == null; }
    }
}
