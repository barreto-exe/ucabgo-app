using UcabGo.App.Api.Interfaces;
using UcabGo.App.Api.Services;
using UcabGo.App.Api.Services.Phone;
using UcabGo.App.Api.Tools;
using UcabGo.App.Services;
using UcabGo.App.Services.Navigation;
using UcabGo.App.Services.Settings;
using UcabGo.App.Shells;
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
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "IconFont");
            });

        //DI API Services
        builder.Services.AddSingleton<IAuthApi, AuthApi>();
        builder.Services.AddSingleton<IPhoneApi, PhoneApi>();

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


        //DI Views
        builder.Services.AddSingleton<LoginView>();
        builder.Services.AddSingleton<RoleSelectionView>();
        builder.Services.AddSingleton<ProfileView>();
        builder.Services.AddSingleton<HelpView>();
        builder.Services.AddSingleton<AboutView>();
        builder.Services.AddSingleton<TermsView>();
        builder.Services.AddScoped<PasswordView>();
        builder.Services.AddScoped<PhoneView>();

        
        //Setting the API URL
        using var stream = FileSystem.OpenAppPackageFileAsync("API.txt").Result;
        using var reader = new StreamReader(stream);
        var apiUrl = reader.ReadToEnd();
        ApiRoutes.BASE_URL = apiUrl;

        return builder.Build();
    }
}
