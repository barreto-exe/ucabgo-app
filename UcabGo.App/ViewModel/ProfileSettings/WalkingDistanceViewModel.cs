using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UcabGo.App.Api.Services.User;
using UcabGo.App.Services;

namespace UcabGo.App.ViewModel
{
    public partial class WalkingDistanceViewModel : ViewModelBase
    {
        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        bool isEmpty;

        [ObservableProperty]
        private string buttonText;

        [ObservableProperty]
        private bool isButtonEnabled;

        [ObservableProperty]
        int walkingDistance;

        readonly IUserApi userApi;

        public WalkingDistanceViewModel(ISettingsService settingsService, INavigationService navigation, IUserApi userApi) : base(settingsService, navigation)
        {
            this.userApi = userApi;
        }

        public override void OnAppearing()
        {
            IsEmpty = false;
            IsButtonEnabled = false;
            ButtonText = "Guardar";

            WalkingDistance = Convert.ToInt32(settings.User.WalkingDistance);
        }

        [RelayCommand]
        async Task ValueUpdated()
        {
            IsButtonEnabled = true;
        }

        [RelayCommand]
        async Task Save()
        {
            IsButtonEnabled = false;
            ButtonText = "Guardando...";

            var response = await userApi.ChangeWalkingDistanceAsync(WalkingDistance);
            if (response?.Message == "WALKING_DISTANCE_UPDATED")
            {
                var user = settings.User;
                user.WalkingDistance = WalkingDistance;
                settings.User = user;

                await Application.Current.MainPage.DisplayAlert("Éxito", "La distancia de caminata se ha actualizado correctamente.", "Aceptar");

                await navigation.GoBackAsync();
            }
            else
            {
                WalkingDistance = Convert.ToInt32(settings.User.WalkingDistance);
            }

            IsButtonEnabled = true;
            ButtonText = "Guardar";
        }
    }
}
