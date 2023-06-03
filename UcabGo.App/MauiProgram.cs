using System.Reflection;
using UcabGo.App.ApiAccess.Interfaces;
using UcabGo.App.ApiAccess.Services;
using UcabGo.App.ApiAccess.Tools;
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

		//DI ViewModels
		builder.Services.AddSingleton<LoginViewModel>();
		
		//DI Services
		builder.Services.AddSingleton<AuthService>();


		//Setting the API URL
        using var stream = FileSystem.OpenAppPackageFileAsync("API.txt").Result;
        using var reader = new StreamReader(stream);
        var apiUrl = reader.ReadToEnd();
		Routes.BASE_URL = apiUrl;

        return builder.Build();
	}
}
