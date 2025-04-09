using Microsoft.Maui.Controls;
using mobileapp.Data;
using mobileapp.Models;

namespace mobileapp.Views.Pages;
[QueryProperty(nameof(Animal.Id), nameof(Animal.Id))]
public partial class AnimalItemPage : ContentPage
{
    private Animal animal;
    private string Title;
    public AnimalItemPage()
    {
        InitializeComponent();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Page.Title = Title;
    }
    public string Id
    {
        set
        {
            animal = AnimalRepository.GetAnimal(Guid.Parse(value));
            if (animal is not null)
            {
                AnimalItemCtrl.Name = animal.Name;
                AnimalItemCtrl.Category = animal.Category;
                AnimalItemCtrl.Image = animal.Image;
                AnimalItemCtrl.Description = animal.Description;
                Title = animal.Name;
            }
        }

    }

    private async void AnimalItemCtrl_OnUpdate(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(EditAnimalPage)}?Id={animal.Id}");
    }
}