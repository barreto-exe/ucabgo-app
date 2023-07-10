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
        public string AliasEmoji
        {
            get
            {
                if (Alias.ToLower().Contains("casa")) return "🏠 " + Alias;
                if (Alias.ToLower().Contains("trabajo")) return "🏢 " + Alias;
                if (Alias.ToLower().Contains("universidad")) return "🏫 " + Alias;
                if (Alias.ToLower().Contains("ucab")) return "🔰 " + Alias;

                return "📍 " + Alias;
            }
        }
        public string DestinationText
        {
            get
            {
                if (Alias.ToLower().Contains("ucab") == true)
                {
                    return "UCAB Guayana";
                }
                else
                {
                    return Zone;
                }
            }
        }
    }
}
