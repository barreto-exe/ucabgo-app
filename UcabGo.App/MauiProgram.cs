using System.Reflection;
using UcabGo.App.ApiAccess.Tools;

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

		//Setting the API URL
        using var stream = FileSystem.OpenAppPackageFileAsync("API.txt").Result;
        using var reader = new StreamReader(stream);
        var apiUrl = reader.ReadToEnd();
		Routes.BASE_URL = apiUrl;

        return builder.Build();
	}
}
