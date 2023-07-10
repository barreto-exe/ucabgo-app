using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UcabGo.App.Api.Services.User;
using UcabGo.App.Services;
using UcabGo.App.Views;

namespace UcabGo.App.ViewModel
{
    public partial class ProfileViewModel : ViewModelBase
    {
        [ObservableProperty]
        string username;
        [ObservableProperty]
        string email;
        [ObservableProperty]
        ImageSource pictureUrl;

        [ObservableProperty]
        bool isImageLoading;

        readonly IUserApi userApi;

        public ProfileViewModel(
            ISettingsService settingsService,
            INavigationService navigationService,
            IUserApi userApi) : base(settingsService, navigationService)
        {
            this.userApi = userApi;

            //Reload because the app is reopening
            settings.ReloadImage = true;
        }

        public override void OnAppearing()
        {
            Username = settings.User.Name + " " + settings.User.LastName;
            Email = settings.User.Email;
            IsImageLoading = false;

            if(settings.ReloadImage)
            {
                IsImageLoading = true;

                if (!string.IsNullOrEmpty(settings.User.ProfilePicture))
                {
                    PictureUrl = ImageSource.FromUri(new Uri(settings.User.ProfilePicture));
                }
                else if (string.IsNullOrEmpty(settings.User.ProfilePicture))
                {
                    PictureUrl = null;
                }

                IsImageLoading = false;
            }

            settings.ReloadImage = false;
        }

        [RelayCommand]
        async Task ChangePassword()
        {
            await navigation.NavigateToAsync<PasswordView>();
        }

        [RelayCommand]
        async Task ChangePhone()
        {
            await navigation.NavigateToAsync<PhoneView>();
        }
        [RelayCommand]
        async Task SosContacts()
        {
            await navigation.NavigateToAsync<SosContactsView>();
        }
        [RelayCommand]
        async Task Vehicles()
        {
            await navigation.NavigateToAsync<VehiclesView>();
        }
        [RelayCommand]
        async Task MyTrips()
        {
            //await navigation.NavigateToAsync<MyTripsView>();
        }

        [RelayCommand]
        async Task MyHouse()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                await navigation.NavigateToAsync<MapView>();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Es necesario que aceptes los permisos de ubicación para poder continuar", "Aceptar");
            }
        }

        [RelayCommand]
        async Task WalkingDistance()
        {
            await navigation.NavigateToAsync<WalkingDistanceView>();
        }

        [RelayCommand]
        async Task Logout()
        {
            settings.User = null;
            settings.AccessToken = null;

            await navigation.RestartSession();
        }

        [RelayCommand]
        async Task ChangeProfilePicture()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }

            if(status != PermissionStatus.Granted)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Es necesario que aceptes los permisos de almacenamiento para poder cambiar la foto de perfil.", "Aceptar");
                return;
            }

            IsImageLoading = true;

            var file = await MediaPicker.PickPhotoAsync();
            if (file != null)
            {
                var stream = await file.OpenReadAsync();
                var request = new MultipartFormDataContent
                {
                    { new StreamContent(stream), "Picture", file.FileName }
                };

                //Send trough formdata PUT request to API
                try
                {
                    var response = await userApi.UpdateProfilePicture(request);
                    if (response.Message == "PROFILE_PICTURE_UPDATED")
                    {
                        // Update profile picture
                        var user = settings.User;
                        user.ProfilePicture = response.Data.ProfilePicture;
                        settings.User = user;

                        PictureUrl = ImageSource.FromUri(new Uri(settings.User.ProfilePicture));
                    }
                    else if (response.Message == "INVALID_FILE")
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "El formato de la imagen debe ser jpg, jpeg o png.", "Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    // Error updating profile picture
                    await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un error al actualizar la imagen de perfil. Inténtelo de nuevo.", "Aceptar");
                }
            }

            IsImageLoading = false;
        }
    }
}
