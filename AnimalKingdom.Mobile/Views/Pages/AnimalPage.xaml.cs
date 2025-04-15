using AnimalKingdom.Mobile.Views.ViewModel;

namespace AnimalKingdom.Mobile.Views.Pages;

public partial class AnimalItemPage : ContentPage
{
    private readonly AnimalViewModel _viewModel;
    public AnimalItemPage(AnimalViewModel viewModel)
    {
        InitializeComponent();
     
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }


}