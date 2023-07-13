using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using UcabGo.App.Api.Models;
using UcabGo.App.Api.Services.UserRating;
using UcabGo.App.Api.Services.UserRating.Inputs;
using UcabGo.App.Models;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    [QueryProperty(nameof(UsersJson), "usersJson")]
    [QueryProperty(nameof(RideId), "rideId")]
    [QueryProperty(nameof(IsDriver), "isDriver")]
    public partial class RateUserViewModel : ViewModelBase
    {
        readonly IUserRatingApi userRatingApi;

        [ObservableProperty]
        int rideId;

        [ObservableProperty]
        bool isDriver;

        [ObservableProperty]
        ObservableCollection<UserRating> ratings;

        [ObservableProperty]
        bool isButtonEnabled;

        [ObservableProperty]
        string buttonText;

        [ObservableProperty]
        string usersJson;

        [ObservableProperty]
        int rateTest;

        private IEnumerable<User> users;

        public RateUserViewModel(ISettingsService settingsService, INavigationService navigation, IUserRatingApi userRatingApi) : base(settingsService, navigation)
        {
            this.userRatingApi = userRatingApi;

            users = new List<User>();
            ratings = new();
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            IsButtonEnabled = true;
            ButtonText = "Calificar";

            //UsersJson = "[{\"Id\":4,\"Email\":\"alfonzo1@ucab.edu.ve\",\"Name\":\"Jesús\",\"LastName\":\"Alfonzo\",\"CompleteName\":\"Jesús Alfonzo\",\"Phone\":\"04128339241\",\"WalkingDistance\":536.0,\"ProfilePicture\":\"https://i.imgur.com/tP7TNRu.png\",\"Initial\":\"J\"},{\"Id\":4,\"Email\":\"alfonzo1@ucab.edu.ve\",\"Name\":\"Jesús\",\"LastName\":\"Alfonzo\",\"CompleteName\":\"Jesús Alfonzo\",\"Phone\":\"04128339241\",\"WalkingDistance\":536.0,\"ProfilePicture\":\"https://i.imgur.com/tP7TNRu.png\",\"Initial\":\"J\"}]\r\n";
            //IsDriver = true;

            users = JsonConvert.DeserializeObject<IEnumerable<User>>(UsersJson);
            if (!users.Any())
            {
                await navigation.GoBackAsync();
                return;
            }

            Ratings = new(from u in users
                          select new UserRating
                          {
                              User = u,
                              Rating = 0
                          });
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        [RelayCommand]
        public async Task Send()
        {
            if(Ratings.Any(r => r.Rating == 0))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe calificar a todos los usuarios", "Aceptar");
                return;
            }


            IsButtonEnabled = false;
            ButtonText = "Calificando...";

            List<Task> tasks = new();

            foreach(var rating in Ratings)
            {
                tasks.Add(userRatingApi.PostRating(new RatingInput
                {
                    RideId = RideId,
                    EvaluatorId = settings.User.Id,
                    EvaluatedId = rating.User.Id,
                    EvaluatorType = IsDriver ? "D" : "P",
                    Stars = Convert.ToInt32(Math.Ceiling(rating.Rating)),
                }));
            }

            await Task.WhenAll(tasks);

            await navigation.NavigateToAsync("//" + nameof(RoleSelectionView));
            
            IsButtonEnabled = true;
            ButtonText = "Calificar";
        }

        [RelayCommand]
        async Task UpdateRatings()
        {
            //Parse all ratings to int
            foreach(var rating in Ratings)
            {
                rating.Rating = Convert.ToInt32(Math.Ceiling(rating.Rating));
            }

            Ratings = new(Ratings);
        }
    }
}
