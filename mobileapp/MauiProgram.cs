using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using mobileapp.Views.ViewModel;

namespace mobileapp;

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

		// Add View Model as SingleTon
		builder.Services.AddSingleton<AnimalViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
