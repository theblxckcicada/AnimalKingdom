using mobileapp.Data;
using System.Collections.ObjectModel;
using mobileapp.Models;
namespace mobileapp.Views.Pages;

public partial class AnimalPage : ContentPage
{
    public AnimalPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadAnimals();
        // Search the animal
        if (!string.IsNullOrEmpty(AnimalSearchBar.Text))
        {
            var animals = AnimalRepository.Search(AnimalSearchBar.Text);
            LoadAnimals(animals);
        }
      
    }

    public void LoadAnimals(List<Animal> animals = null)
    {
        var animalList = new ObservableCollection<Animal>(animals ?? AnimalRepository.GetAnimals());
        AnimalListCollection.ItemsSource = animalList;
    }

    private async void AnimalListCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (AnimalListCollection.SelectedItem is not null)
        {
            await Shell.Current.GoToAsync($"{nameof(AnimalItemPage)}?Id={((Animal)AnimalListCollection.SelectedItem).Id}");
            AnimalListCollection.SelectedItem = null;
        }
    }

    private async void BtnAddAnimal_ClickedAsync(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddAnimalPage));
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Search the animal
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            LoadAnimals();
            return;
        }
        var animals = AnimalRepository.Search(e.NewTextValue);
        LoadAnimals(animals);

    }
}