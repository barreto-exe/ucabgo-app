namespace UcabGo.App.Models
{
    public class SosContact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Initial { get => Name[0].ToString().ToUpper(); }
    }
}
