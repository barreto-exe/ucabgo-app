using Maui.GoogleMaps.Hosting;
using UcabGo.App.Api.Interfaces;
using UcabGo.App.Api.Services;
using UcabGo.App.Api.Services.Chat;
using UcabGo.App.Api.Services.Destinations;
using UcabGo.App.Api.Services.Driver;
using UcabGo.App.Api.Services.GoogleMaps;
using UcabGo.App.Api.Services.Locations;
using UcabGo.App.Api.Services.Passenger;
using UcabGo.App.Api.Services.PassengerService;
using UcabGo.App.Api.Services.Rides;
using UcabGo.App.Api.Services.SignalR;
using UcabGo.App.Api.Services.SosContacts;
using UcabGo.App.Api.Services.User;
using UcabGo.App.Api.Services.Vehicles;
using UcabGo.App.Services;
using UcabGo.App.Services.Navigation;
using UcabGo.App.Services.Settings;
using UcabGo.App.Shells;
using UcabGo.App.Utils;
using UcabGo.App.ViewModel;
using UcabGo.App.Views;

namespace UcabGo.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
#if ANDROID
            .UseGoogleMaps()
#elif IOS
            .UseGoogleMaps(EnviromentVariables.GetValue("GoogleMapsApiKey"))
#endif
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "IconFont");
            });

        //DI API Services
        builder.Services.AddSingleton<IAuthApi, AuthApi>();
        builder.Services.AddSingleton<IUserApi, UserApi>();
        builder.Services.AddSingleton<ISosContactsApi, SosContactsApi>();
        builder.Services.AddSingleton<IVehiclesApi, VehiclesApi>();
        builder.Services.AddSingleton<IGoogleMapsApi, GoogleMapsApi>();
        builder.Services.AddSingleton<ILocationsApiService, LocationsApiService>();
        builder.Services.AddSingleton<IDestinationsService, DestinationsService>();
        builder.Services.AddSingleton<IDriverApi, DriverApi>();
        builder.Services.AddSingleton<IRidesApi, RidesApi>();
        builder.Services.AddSingleton<IPassengerApi, PassengerApi>();
        builder.Services.AddSingleton<IChatApi, ChatApi>();

        builder.Services.AddSingleton<IHubConnectionFactory, HubConnectionFactory>();


        //DI Services
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();

        //DI Shells
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<SessionShell>();

        //DI ViewModels
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<RoleSelectionViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddSingleton<PasswordViewModel>();
        builder.Services.AddSingleton<PhoneViewModel>();
        builder.Services.AddSingleton<SosContactsViewModel>();
        builder.Services.AddSingleton<SosContactsAddViewModel>();
        builder.Services.AddSingleton<VehiclesViewModel>();
        builder.Services.AddSingleton<VehiclesAddViewModel>();
        builder.Services.AddSingleton<WalkingDistanceViewModel>();
        builder.Services.AddSingleton<MapViewModel>();
        builder.Services.AddSingleton<DestinationsListViewModel>();
        builder.Services.AddSingleton<DestinationAddViewModel>();
        builder.Services.AddSingleton<ActiveRiderViewModel>();
        builder.Services.AddSingleton<SelectDestinationViewModel>();
        builder.Services.AddSingleton<RidesAvailableViewModel>();
        builder.Services.AddSingleton<ActivePassengerViewModel>();
        builder.Services.AddSingleton<ChatViewModel>();

        //DI Views
        builder.Services.AddSingleton<LoginView>();
        builder.Services.AddSingleton<RoleSelectionView>();
        builder.Services.AddSingleton<ProfileView>();
        builder.Services.AddSingleton<HelpView>();
        builder.Services.AddSingleton<AboutView>();
        builder.Services.AddSingleton<TermsView>();
        builder.Services.AddSingleton<PasswordView>();
        builder.Services.AddSingleton<PhoneView>();
        builder.Services.AddSingleton<SosContactsView>();
        builder.Services.AddSingleton<SosContactAddView>();
        builder.Services.AddSingleton<VehiclesView>();
        builder.Services.AddSingleton<VehiclesAddView>();
        builder.Services.AddSingleton<WalkingDistanceView>();
        builder.Services.AddSingleton<MapView>();
        builder.Services.AddSingleton<DestinationsListView>();
        builder.Services.AddSingleton<DestinationAddView>();
        builder.Services.AddSingleton<ActiveRiderView>();
        builder.Services.AddSingleton<SelectDestinationView>();
        builder.Services.AddSingleton<RidesAvailableView>();
        builder.Services.AddSingleton<ActivePassengerView>();
        builder.Services.AddSingleton<ChatView>();


        //Removes the underline from the Entry
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
        {
#if ANDROID
            h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
#if WINDOWS
            h.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0,0,0,0);
#endif
        });

        //Removes the underline from the Picker control
        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
        {
#if ANDROID
            h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
#if WINDOWS
            h.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0,0,0,0);
#endif
        });

        return builder.Build();
    }
}
