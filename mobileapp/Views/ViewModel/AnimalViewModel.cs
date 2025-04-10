using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mobileapp.Data;
using mobileapp.Models;
using mobileapp.Views.Pages;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace mobileapp.Views.ViewModel;

public partial class AnimalViewModel : ContentView
{

    public ObservableCollection<Animal> AnimalCollection { get; } = [];
    public AnimalViewModel()
    {
        LoadAnimals();
    }


    public void LoadAnimals(string searchText = "")
    {

        List<Animal> animals = string.IsNullOrEmpty(searchText) ? AnimalRepository.GetAnimals() : AnimalRepository.Search(searchText);

        // Clear Collection First
        AnimalCollection.Clear();
        foreach (var animal in animals)
        {
            AnimalCollection.Add(animal);
        }

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