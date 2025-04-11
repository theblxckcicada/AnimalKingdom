using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mobileapp.Data;
using mobileapp.Models;
using mobileapp.Views.Pages;

namespace mobileapp.Views.ViewModel;


[QueryProperty(nameof(Animal.Id), nameof(Animal.Id))]
public partial class AnimalViewModel : BaseViewModel
{
    [ObservableProperty]
    private Animal animal;

    [ObservableProperty]
    private string pageTitle;
    private string id;

    	[ObservableProperty]
	public string name;
	[ObservableProperty]
	public string description;
    [ObservableProperty]
	public string image;

    [ObservableProperty]
    public AnimalCategory category;

	public List<AnimalCategory> CategoryList => [.. Enum.GetValues<AnimalCategory>()];
    public AnimalViewModel()
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
            Image = Animal.Image;
            Description = Animal.Description;
            Category = Animal.Category;
            Name = Animal.Name;
            PageTitle = Animal?.Name ?? string.Empty;
        }
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
        if (Animal is not null)
        {
            Animal.Name = Name;
            Animal.Description = Description;
            Animal.Category = Category;
            Animal.Image = Image;

            // update the animal 
            AnimalRepository.UpdateAnimal(Animal);
        }
        else
        {
            // create a new animal 
            Animal animal = new()
            {
                Id = Guid.NewGuid(),
                Name = Name,
                Image = Image,
                Description = Description,
                Category = Category,
            };
            AnimalRepository.AddAnimal(animal);
        }
        await Shell.Current.GoToAsync($"//{nameof(AnimalListPage)}");

    }

    [RelayCommand]
    private async Task NavigateToEditAnimalPageAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(EditAnimalPage)}?Id={Animal.Id}");
    }

    [RelayCommand]
    private async Task NavigateToHomePageAsync()
    {
        await Shell.Current.GoToAsync($"//{nameof(AnimalListPage)}");
    }
}