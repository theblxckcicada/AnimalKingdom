
using AnimalKingdom.Mobile.Views.Pages;

namespace AnimalKingdom.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register routes 
		Routing.RegisterRoute(nameof(AddAnimalPage),typeof(AddAnimalPage));
		Routing.RegisterRoute(nameof(EditAnimalPage),typeof(EditAnimalPage));
		Routing.RegisterRoute(nameof(AnimalListPage),typeof(AnimalListPage));
		Routing.RegisterRoute(nameof(AnimalItemPage),typeof(AnimalItemPage));
	}
}
