using Microsoft.Maui.Controls;
using AnimalKingdom.Mobile.Data;
using AnimalKingdom.Mobile.Models;
using AnimalKingdom.Mobile.Views.ViewModel;

namespace AnimalKingdom.Mobile.Views.Pages;

public partial class EditAnimalPage : ContentPage
{

    public EditAnimalPage(AnimalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}