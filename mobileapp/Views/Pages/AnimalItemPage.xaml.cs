using Microsoft.Maui.Controls;
using mobileapp.Data;
using mobileapp.Models;
using mobileapp.Views.ViewModel;

namespace mobileapp.Views.Pages;

public partial class AnimalItemPage : ContentPage
{
    private readonly AnimalItemViewModel _viewModel;
    public AnimalItemPage(AnimalItemViewModel viewModel)
    {
        InitializeComponent();
     
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }


}