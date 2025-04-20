using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AnimalKingdom.Mobile.Views.Pages;
using System.Collections.ObjectModel;
using AnimalKingdom.Shared.Models;
using AnimalKingdom.Mobile.Services;

namespace AnimalKingdom.Mobile.Views.ViewModel;

public partial class AnimalListViewModel : BaseViewModel
{
    [ObservableProperty]

    private ObservableCollection<Animal> animalCollection = [];


    private readonly AnimalService animalService;
    public AnimalListViewModel(AnimalService animalService)
    {
        this.animalService = animalService;
        animalService.AnimalsUpdated += OnAnimalsUpdated;
        _ = LoadAnimals();
    }


    private Task LoadAnimals(string searchText = "")
    {
        return RunBusyAsync(async () =>
        {

            var animals = string.IsNullOrEmpty(searchText)
                ? await animalService.GetAnimals()
                : await animalService.Search(searchText);

            AnimalCollection = new ObservableCollection<Animal>(animals);
        });
    }


    private void OnAnimalsUpdated()
    {
        LoadAnimals(); // or you could reload with a filter if you track searchText
    }

    [RelayCommand]
    private async Task AnimalSelectionChangedAsync(Animal animal)
    {
        if (animal is not null)
        {
            await Shell.Current.GoToAsync($"{nameof(AnimalItemPage)}?Id={animal.Id}");
        }


    }



    [RelayCommand]
    private void AnimalSearchTextChanged(string text)
    {
        LoadAnimals(text);
    }

    [RelayCommand]
    private async Task DeleteAnimalAsync(Animal animal)
    {
        await AquireAccessTokenAsync(animalService);
        await RunBusyAsync(async () =>
        {
            if (animal is not null)
            {
                bool confirm = await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete {animal.Name}?", "Yes", "No");
                if (confirm)
                {
                    await animalService.DeleteAnimal(animal.Id);
                }
            }
        });
    }
}