#if !WINDOWS
    using Maui.GoogleMaps.Hosting;
#endif
using UcabGo.App.Api.Interfaces;
using UcabGo.App.Api.Services;
using UcabGo.App.Api.Services.SosContacts;
using UcabGo.App.Api.Services.User;
using UcabGo.App.Api.Services.Vehicles;
using UcabGo.App.Api.Tools;
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

        //Setting the API URL
        ApiRoutes.BASE_URL = EnviromentVariables.GetValue("ApiUrl");

        return builder.Build();
    }
}
