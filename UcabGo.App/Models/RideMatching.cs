namespace UcabGo.App.Models
{
    public class RideMatching
    {
        public Ride Ride { get; set; }
        public double MatchingPercentage { get; set; }
        public string MatchingPercentageText
        {
            get
            {
                //Round to integer
                var percentage = (int)(MatchingPercentage * 100);
                return $"{percentage}%";
            }
        }
    }
}
