using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AnimalKingdom.Mobile.Data;
using AnimalKingdom.Mobile.Views.Pages;
using System.Collections.ObjectModel;
using AnimalKingdom.Shared.Models;

namespace AnimalKingdom.Mobile.Views.ViewModel;

public partial class AnimalListViewModel : BaseViewModel
{
    [ObservableProperty]

    private ObservableCollection<Animal> animalCollection = [];


    public AnimalListViewModel()
    {
        AnimalRepository.AnimalsUpdated += OnAnimalsUpdated;

        LoadAnimals();
    }


    private void LoadAnimals(string searchText = "")
    {

       AnimalCollection = new (string.IsNullOrEmpty(searchText) ? AnimalRepository.GetAnimals() : AnimalRepository.Search(searchText));

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
    private async Task NavigateToAddAnimalPageAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddAnimalPage));
    }

    [RelayCommand]
    private void AnimalSearchTextChanged(string text)
    {
        LoadAnimals(text);
    }
}