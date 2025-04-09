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
	}

	public void LoadAnimals()
	{
		var animals = new ObservableCollection<Animal>(AnimalRepository.GetAnimals());
        AnimalListCollection.ItemsSource = animals;
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

}