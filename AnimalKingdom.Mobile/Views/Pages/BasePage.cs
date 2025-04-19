using AnimalKingdom.Mobile.Views.ViewModel;

namespace AnimalKingdom.Mobile.Views.Pages;

public class BasePage<TViewModel> : ContentPage where TViewModel : BaseViewModel
{
    protected TViewModel? ViewModel => BindingContext as TViewModel;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (ViewModel != null)
            await ViewModel.CheckAccountStatusAsync();
    }
}
