using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using AnimalKingdom.Mobile.Views.ViewModel;
using AnimalKingdom.Mobile.Repository;
using AnimalKingdom.Mobile.Views.Controls;

namespace AnimalKingdom.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
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
		builder.Services.AddSingleton<AnimalRepository>();
		builder.Services.AddSingleton<HttpClient>();

		// Add View Models
		builder.Services.AddSingleton<AnimalListViewModel>();
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddTransient<AnimalViewModel>();
		


#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
