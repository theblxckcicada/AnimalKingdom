using Microsoft.Maui.Controls;
using mobileapp.Data;
using mobileapp.Models;
using mobileapp.Views.ViewModel;

namespace mobileapp.Views.Pages;

public partial class EditAnimalPage : ContentPage
{

    public EditAnimalPage(AnimalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}