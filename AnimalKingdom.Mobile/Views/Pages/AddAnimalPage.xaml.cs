using AnimalKingdom.Mobile.Views.ViewModel;

namespace AnimalKingdom.Mobile.Views.Pages;

public partial class AddAnimalPage : ContentPage
{
    private readonly AnimalViewModel viewModel;
    public AddAnimalPage(AnimalViewModel _viewModel)
    {
        InitializeComponent();
        viewModel = _viewModel;
        BindingContext = _viewModel;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.CheckAccountStatusAsync();
    }
}