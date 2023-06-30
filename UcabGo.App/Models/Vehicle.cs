using UcabGo.App.Api.Models;

namespace UcabGo.App.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
        public string Color { get; set; }
        public User Owner { get; set; }
        public string Name { get => $"{Plate} - {Brand} {Model}"; }
    }
}
