using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UcabGo.App.Api.Services.Vehicles;
using UcabGo.App.Models;
using UcabGo.App.Services;

namespace UcabGo.App.ViewModel
{
    [QueryProperty(nameof(VehicleParameter), "vehicle")]
    public partial class VehiclesAddViewModel : ViewModelBase
    {
        [ObservableProperty]
        string brandEntry;

        [ObservableProperty]
        string modelEntry;

        [ObservableProperty]
        string plateEntry;

        [ObservableProperty]
        string colorEntry;

        [ObservableProperty]
        string buttonText;

        [ObservableProperty]
        bool isButtonEnabled;

        [ObservableProperty]
        Vehicle vehicleParameter;

        bool isEditing => VehicleParameter != null;

        readonly IVehiclesApi vehiclesApi;

        public VehiclesAddViewModel(ISettingsService settingsService, INavigationService navigation, IVehiclesApi vehiclesApi) : base(settingsService, navigation)
        {
            this.vehiclesApi = vehiclesApi;
        }

        public override void OnAppearing()
        {
            ButtonText = "Guardar";
            IsButtonEnabled = true;

            if (isEditing)
            {
                BrandEntry = VehicleParameter.Brand;
                ModelEntry = VehicleParameter.Model;
                PlateEntry = VehicleParameter.Plate;
                ColorEntry = VehicleParameter.Color;
            }
            else
            {
                BrandEntry = string.Empty;
                ModelEntry = string.Empty;
                PlateEntry = string.Empty;
                ColorEntry = string.Empty;
            }
        }

        [RelayCommand]
        async Task Save()
        {
            bool isValidInput =
                !string.IsNullOrEmpty(BrandEntry) &&
                !string.IsNullOrEmpty(ModelEntry) &&
                !string.IsNullOrEmpty(PlateEntry) &&
                !string.IsNullOrEmpty(ColorEntry);
            if (!isValidInput)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe llenar todos los campos", "Aceptar");
                return;
            }

            ButtonText = "Guardando...";
            IsButtonEnabled = false;

            if (isEditing)
            {
                await UpdateVehicle();
            }
            else
            {
                await AddVehicle();
            }


            ButtonText = "Guardar";
            IsButtonEnabled = true;
        }

        private async Task AddVehicle()
        {
            var apiResponse = await vehiclesApi.AddVehicle(new Vehicle
            {
                Brand = BrandEntry,
                Model = ModelEntry,
                Plate = PlateEntry,
                Color = ColorEntry,
            });

            if (apiResponse.Message == "VEHICLE_CREATED")
            {
                await navigation.GoBackAsync();
            }
        }

        private async Task UpdateVehicle()
        {
            var apiResponse = await vehiclesApi.UpdateVehicle(new Vehicle
            {
                Id = VehicleParameter.Id,
                Brand = BrandEntry,
                Model = ModelEntry,
                Plate = PlateEntry,
                Color = ColorEntry,
            });
            if (apiResponse.Message == "VEHICLE_UPDATED")
            {
                await navigation.GoBackAsync();
            }
            else if (apiResponse.Message == "VEHICLE_NOT_FOUND")
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El vehículo no existe", "Aceptar");
            }
        }

        [RelayCommand]
        async Task Cancel()
        {
            await navigation.GoBackAsync();
        }

        public bool IsValidPhone(string phone)
        {
            return true;
        }
    }
}
