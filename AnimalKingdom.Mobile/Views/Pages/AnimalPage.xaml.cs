using AnimalKingdom.Mobile.Views.ViewModel;

namespace AnimalKingdom.Mobile.Views.Pages;

public partial class AnimalItemPage : ContentPage
{
    private readonly AnimalViewModel viewModel;
    public AnimalItemPage(AnimalViewModel _viewModel)
    {
        InitializeComponent();
     
        viewModel = _viewModel;
        BindingContext = _viewModel;
        _ = viewModel.CheckAccountStatusAsync();
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.CheckAccountStatusAsync();
    }

    }