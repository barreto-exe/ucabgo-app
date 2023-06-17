namespace UcabGo.App.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Zone { get; set; }
        public string Detail { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsHome { get; set; }
    }
}
