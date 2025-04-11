using Microsoft.Maui.Controls;
using mobileapp.Data;
using mobileapp.Models;
using mobileapp.Views.ViewModel;

namespace mobileapp.Views.Pages;

public partial class AddAnimalPage : ContentPage
{
	public AddAnimalPage(AnimalViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}