
using mobileapp.Views.Pages;

namespace mobileapp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register routes 
		Routing.RegisterRoute(nameof(AddAnimalPage),typeof(AddAnimalPage));
		Routing.RegisterRoute(nameof(EditAnimalPage),typeof(EditAnimalPage));
		Routing.RegisterRoute(nameof(AnimalPage),typeof(AnimalPage));
		Routing.RegisterRoute(nameof(AnimalItemPage),typeof(AnimalItemPage));
	}
}
