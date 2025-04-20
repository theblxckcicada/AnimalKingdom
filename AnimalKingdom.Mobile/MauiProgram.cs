using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using AnimalKingdom.Mobile.Views.ViewModel;
using AnimalKingdom.Mobile.Views.Controls;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using AnimalKingdom.Mobile.Services;

namespace AnimalKingdom.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

        var assembly = Assembly.GetExecutingAssembly();
        // Load appsettings.json
        string configFileName = "appsettings.development.json";
        string embeddedConfigfilename = $"{Assembly.GetCallingAssembly().GetName().Name}.{configFileName}";
        using var stream = assembly.GetManifestResourceStream(embeddedConfigfilename);

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();
        builder.Configuration.AddConfiguration(config);


        // load app settings configuration 
        builder
            .UseMauiApp<App>().UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

				// add icons
				fonts.AddFont("Brands-Regular-400.otf", "BrandsRegular");
                fonts.AddFont("Free-Regular-400.otf", "FreeRegular");
				fonts.AddFont("Free-Solid-900.otf", "FreeSolid");
			});



		// Add Dependency injection 
        builder.Services.AddSingleton(FilePicker.Default);
		builder.Services.AddSingleton<AnimalService>();
		builder.Services.AddSingleton<HttpClient>();

		// Add View Models
		builder.Services.AddSingleton<AnimalListViewModel>();
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddTransient<AnimalViewModel>();
		builder.Services.AddTransient<AnimalControl>();
		
#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
