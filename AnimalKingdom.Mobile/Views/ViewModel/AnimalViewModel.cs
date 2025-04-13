using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AnimalKingdom.Shared.Models;
using AnimalKingdom.Mobile.Views.Pages;
using AnimalKingdom.Mobile.Repository;

namespace AnimalKingdom.Mobile.Views.ViewModel;


[QueryProperty(nameof(Animal.Id), nameof(Animal.Id))]
public partial class AnimalViewModel(AnimalRepository animalRepository) : BaseViewModel
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

            // Set category list regardless of animal loading
            categoryList = [.. Enum.GetValues<AnimalCategory>()];

            // If we couldn't load an animal, exit gracefully
            if (animal is null)
                return;

            // Update ViewModel properties
            Animal = animal;
            Image = animal.Image;
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

            // Reset the stream position to beginning (since ImageSource.FromStream might have read it)
            stream.Position = 0;

            // Convert stream to byte array
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            // Convert to base64 string
            string base64String = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";

            // set the image to the field
            Image = base64String;

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
                Animal.Image = Image;

                await animalRepository.UpdateAnimal(Animal);
            }
            else
            {
                // Create new animal
                Animal newAnimal = new()
                {
                    Id = Guid.NewGuid(),
                    Name = Name,
                    Image = Image,
                    Description = Description,
                    Category = Category,
                };

                await animalRepository.AddAnimal(newAnimal);
            }

            // Navigate back to the list page
            await Shell.Current.GoToAsync($"//{nameof(AnimalListPage)}");
        });
    }


    [RelayCommand]
    private async Task NavigateToEditAnimalPageAsync()
    {
        await RunBusyAsync(async () => await Shell.Current.GoToAsync($"{nameof(EditAnimalPage)}?Id={Animal.Id}"));
    }

    [RelayCommand]
    private async Task NavigateToHomePageAsync()
    {
        await RunBusyAsync(async () => await Shell.Current.GoToAsync($"//{nameof(AnimalListPage)}"));
    }
}