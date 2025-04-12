using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using AnimalKingdom.Mobile.Views.ViewModel;
using AnimalKingdom.Mobile.Repository;

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
			});

		// Add Dependency injection 
        builder.Services.AddSingleton(FilePicker.Default);
		builder.Services.AddSingleton<AnimalRepository>();
		builder.Services.AddSingleton<HttpClient>();

		// Add View Models
		builder.Services.AddSingleton<AnimalListViewModel>();
		builder.Services.AddTransient<AnimalViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
