
using AnimalKingdom.Mobile.Views.Pages;

namespace AnimalKingdom.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        // Register routes 
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(AddAnimalPage),typeof(AddAnimalPage));
		Routing.RegisterRoute(nameof(EditAnimalPage),typeof(EditAnimalPage));
		Routing.RegisterRoute(nameof(AnimalListPage),typeof(AnimalListPage));
		Routing.RegisterRoute(nameof(AnimalItemPage),typeof(AnimalItemPage));
	}
    //protected override async void OnNavigated(ShellNavigatedEventArgs args)
    //{
    //    base.OnNavigated(args);

    //    if (CurrentItem?.CurrentItem?.BindingContext is BaseViewModel viewModel)
    //    {
    //        _ = viewModel.CheckAccountStatusAsync();
    //    }
    //}

}
