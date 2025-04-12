using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AnimalKingdom.Mobile.Views.Pages;
using System.Collections.ObjectModel;
using AnimalKingdom.Shared.Models;
using AnimalKingdom.Mobile.Repository;
using System.Threading.Tasks;

namespace AnimalKingdom.Mobile.Views.ViewModel;

public partial class AnimalListViewModel : BaseViewModel
{
    [ObservableProperty]

    private ObservableCollection<Animal> animalCollection = [];

    private AnimalRepository animalRepository;
    public AnimalListViewModel(AnimalRepository animalRepository)
    {
        this.animalRepository = animalRepository;
        animalRepository.AnimalsUpdated += OnAnimalsUpdated;

        LoadAnimals();
    }


    private async Task LoadAnimals(string searchText = "")
    {

       AnimalCollection = [.. string.IsNullOrEmpty(searchText) ?await  animalRepository.GetAnimals() : await animalRepository.Search(searchText)];

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