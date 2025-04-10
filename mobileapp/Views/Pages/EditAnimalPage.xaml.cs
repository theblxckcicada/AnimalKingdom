using Microsoft.Maui.Controls;
using mobileapp.Data;
using mobileapp.Models;

namespace mobileapp.Views.Pages;
[QueryProperty(nameof(Animal.Id), nameof(Animal.Id))]
public partial class EditAnimalPage : ContentPage
{
    private Animal animal;
    private string Title;
    public EditAnimalPage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Page.Title = $"Edit {Title}";
    }
    public string Id
    {
        set
        {
            animal = AnimalRepository.GetAnimal(Guid.Parse(value));
            if (animal is not null)
            {
                AnimalEditCtrl.Name = animal.Name;
                AnimalEditCtrl.Category = animal.Category;
                AnimalEditCtrl.Image = animal.Image;
                AnimalEditCtrl.Description = animal.Description;
            }
        }

    }

    private async void AnimalEditCtrl_OnSave(object sender, EventArgs e)
    {
        // Update a new Animal to the list 
        if (animal is not null)
        {
            animal.Name = AnimalEditCtrl.Name;
            animal.Image = AnimalEditCtrl.Image;
            animal.Description = AnimalEditCtrl.Description;
            animal.Category = AnimalEditCtrl.Category;
            AnimalRepository.UpdateAnimal(animal);
        }
        await Shell.Current.GoToAsync($"//{nameof(AnimalPage)}");
    }

    private void AnimalEditCtrl_OnError(object sender, string e)
    {

    }

    private async void AnimalEditCtrl_OnCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(AnimalPage)}");
    }

    private async void AnimalEditCtrl_OnDelete(object sender, EventArgs e)
    {
        // delete the animal 
        if(animal is not null)
        {
            AnimalRepository.DeleteAnimal(animal.Id);
        }
        await Shell.Current.GoToAsync($"//{nameof(AnimalPage)}");
    }
}