namespace UcabGo.App.Api.Services.UserRating.Inputs
{
    public class RatingInput
    {
        public int RideId { get; set; }
        public int EvaluatorId { get; set; }
        public int EvaluatedId { get; set; }
        public string EvaluatorType { get; set; }
        public int Stars { get; set; }
    }
}
