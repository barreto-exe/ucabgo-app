using UcabGo.App.Models;
using UserModel = UcabGo.App.Api.Models.User;

namespace UcabGo.App.Api.Services.UserRating.Dtos
{
    public class RatingDto
    {
        public int Id { get; set; }
        public Ride Ride { get; set; }
        public UserModel Evaluated { get; set; }
        public UserModel Evaluator { get; set; }
        public int Stars { get; set; }
        public DateTime EvaluationDate { get; set; }
    }
}
