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
                AnimalCtrl.Name = animal.Name;
                AnimalCtrl.Category = animal.Category;
                AnimalCtrl.Image = animal.Image;
                AnimalCtrl.Description = animal.Description;
            }
        }

    }

    private async void AnimalCtrl_OnSave(object sender, EventArgs e)
    {
        // Update a new Animal to the list 
        if (animal is not null)
        {
            animal.Name = AnimalCtrl.Name;
            animal.Image = AnimalCtrl.Image;
            animal.Description = AnimalCtrl.Description;
            animal.Category = AnimalCtrl.Category;
            AnimalRepository.UpdateAnimal(animal);
        }
        await Shell.Current.GoToAsync($"//{nameof(AnimalPage)}");
    }

    private void AnimalCtrl_OnError(object sender, string e)
    {

    }

    private async void AnimalCtrl_OnCancel(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(AnimalPage)}");
    }

    private async void AnimalCtrl_OnDelete(object sender, EventArgs e)
    {
        // delete the animal 
        if(animal is not null)
        {
            AnimalRepository.DeleteAnimal(animal.Id);
        }
        await Shell.Current.GoToAsync($"//{nameof(AnimalPage)}");
    }
}