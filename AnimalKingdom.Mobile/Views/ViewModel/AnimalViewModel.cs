using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AnimalKingdom.Shared.Models;
using AnimalKingdom.Mobile.Views.Pages;
using AnimalKingdom.Mobile.Repository;

namespace AnimalKingdom.Mobile.Views.ViewModel;


[QueryProperty(nameof(Animal.Id), nameof(Animal.Id))]
public partial class AnimalViewModel : BaseViewModel
{
    [ObservableProperty]
    private Animal animal;

    [ObservableProperty]
    private string pageTitle;
    private string id;

    [ObservableProperty]
    private string name;
    [ObservableProperty]
    private string description;
    [ObservableProperty]
    private string image;

    [ObservableProperty]
    private AnimalCategory category;

    [ObservableProperty]
    private List<AnimalCategory> categoryList;

    AnimalRepository animalRepository;

    public AnimalViewModel(AnimalRepository animalRepository)
    {
        // Set category list regardless of animal loading
        categoryList = [.. Enum.GetValues<AnimalCategory>()];
        this.animalRepository = animalRepository;
  

    }

    public string Id
    {
        get => id;
        set
        {
            if (id == value)
                return;

            id = value;
            _ = LoadAnimalAsync(id); // fire and forget safely
        }
    }

    private async Task LoadAnimalAsync(string id)
    {
        await RunBusyAsync(async () =>
        {
            Animal? animal = null;

            if (Guid.TryParse(id, out var guid))
            {
                // Use existing animal if already loaded
                if (Animal is not null && Animal.Id == guid)
                {
                    animal = Animal;
                }
                else
                {
                    animal = await animalRepository.GetAnimal(guid);
                }
            }



            // If we couldn't load an animal, exit gracefully
            if (animal is null)
            {
                Category = AnimalCategory.Herbivore;
                Image = await ConvertImageToBase64String(await FileSystem.OpenAppPackageFileAsync("default_animal.jpg"));
                return;
            }

            // Update ViewModel properties
            Animal = animal;
            Image = animal.Image ?? await ConvertImageToBase64String(await FileSystem.OpenAppPackageFileAsync("default_animal.jpg"));
            Description = animal.Description;
            Category = animal.Category;
            Name = animal.Name;
        });

    }


    [RelayCommand]
    private async Task LoadImageAsync()
    {
        FileResult result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images
        });

        if (result == null) return;
        if (result != null)
        {

            using var stream = await result.OpenReadAsync();
            // set the image to the field
            Image = await BaseViewModel.ConvertImageToBase64String(stream);

        }
    }
 

    [RelayCommand]
    private async Task SaveAnimalToListAsync()
    {
        await RunBusyAsync(async () =>
        {
            if (Animal is not null)
            {
                // Update existing animal
                Animal.Name = Name;
                Animal.Description = Description;
                Animal.Category = Category;
                Animal.Image = Image ?? await BaseViewModel.ConvertImageToBase64String(await FileSystem.OpenAppPackageFileAsync("default_animal.jpg"));

                await animalRepository.UpdateAnimal(Animal);
            }
            else
            {
                // Create new animal
                Animal newAnimal = new()
                {
                    Id = Guid.NewGuid(),
                    Name = Name,
                    Image = Image ?? await BaseViewModel.ConvertImageToBase64String(await FileSystem.OpenAppPackageFileAsync("default_animal.jpg")),
                    Description = Description,
                    Category = Category,
                };

                await animalRepository.AddAnimal(newAnimal);
            }

            // Navigate back to the list page
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}/{nameof(AnimalListPage)}");
        });
    }


    [RelayCommand]
    private async Task NavigateToEditAnimalPageAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(EditAnimalPage)}?Id={Animal.Id}");
    }


}