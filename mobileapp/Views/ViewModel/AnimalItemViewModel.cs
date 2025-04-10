using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mobileapp.Data;
using mobileapp.Models;
using mobileapp.Views.Pages;

namespace mobileapp.Views.ViewModel;


[QueryProperty(nameof(Animal.Id), nameof(Animal.Id))]
public partial class AnimalItemViewModel : BaseViewModel
{
    [ObservableProperty]
    private Animal animal;

    [ObservableProperty]
    private string title;
    private string id;
    public AnimalItemViewModel()
    {

    }


    public string Id
    {
        get => id;
        set
        {
            if (id == value)
                return;

            id = value;
            LoadAnimal(value);
        }
    }

    private void LoadAnimal(string id)
    {
        if (Guid.TryParse(id, out var guid))
        {
            Animal = AnimalRepository.GetAnimal(guid);
            Title = Animal?.Name ?? string.Empty;
        }
    }

    [RelayCommand]
    private async Task NavigateToEditAnimalPageAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(EditAnimalPage)}?Id={Animal.Id}");
    }

    [RelayCommand]
    private async Task NavigateToHomePageAsync()
    {
        await Shell.Current.GoToAsync($"//{nameof(AnimalPage)}");
    }
}