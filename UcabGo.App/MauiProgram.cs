using UcabGo.App.Api.Interfaces;
using UcabGo.App.Api.Services;
using UcabGo.App.Api.Tools;
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
            });

        //DI Views
        builder.Services.AddSingleton<LoginView>();
        builder.Services.AddSingleton<RoleSelectionView>();

        //DI ViewModels
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<RoleSelectionViewModel>();

        //DI Services
        builder.Services.AddSingleton<IAuthApi, AuthApi>();


        //Setting the API URL
        using var stream = FileSystem.OpenAppPackageFileAsync("API.txt").Result;
        using var reader = new StreamReader(stream);
        var apiUrl = reader.ReadToEnd();
        ApiRoutes.BASE_URL = apiUrl;

        return builder.Build();
    }
}
