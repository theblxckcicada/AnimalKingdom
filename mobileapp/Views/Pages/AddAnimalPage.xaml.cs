using Microsoft.Maui.Controls;
using mobileapp.Data;
using mobileapp.Models;

namespace mobileapp.Views.Pages;

public partial class AddAnimalPage : ContentPage
{
	public AddAnimalPage()
	{
		InitializeComponent();
	}

    private async void AnimalCtrl_OnCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void AnimalCtrl_OnSave(object sender, EventArgs e)
    {
        // Add a new Animal to the list 
        Animal animal = new()
        {
            Id = Guid.NewGuid(),
            Name = AnimalCtrl.Name,
            Image = AnimalCtrl.Image,
            Description = AnimalCtrl.Description,
            Category = AnimalCtrl.Category,
        };
        AnimalRepository.AddAnimal(animal);

        await Shell.Current.GoToAsync("..");

    }
}