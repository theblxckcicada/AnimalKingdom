using AnimalKingdom.Mobile.Views.Pages;
using AnimalKingdom.Mobile.Views.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AnimalKingdom.Mobile.Views.ViewModel;

public partial class BaseViewModel : ObservableObject
{
	[ObservableProperty]
	private bool isBusy;

    [ObservableProperty]
    private bool isNotBusy;
    [ObservableProperty]
    private string randomImage;
    [ObservableProperty]
    bool isMenuVisible;

    [ObservableProperty]
    bool isMenuNotVisible =true;
    readonly Random random = new();
    public BaseViewModel()
	{
		
	}
    public void GetRandomImage()
    {
        RandomImage = $"animal_{(random.Next(1,7))}.jpg";
    }
    protected async Task RunBusyAsync(Func<Task> action)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            IsNotBusy = false;
            await action();
        }
        finally
        {
            IsBusy = false;
            IsNotBusy = true;
        }
    }
    protected async Task<string> ConvertImageToBase64String(Stream stream)
    {         // Only reset position if seeking is supported
        if (stream.CanSeek)
        { stream.Position = 0; }

        // Convert stream to byte array
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        byte[] imageBytes = memoryStream.ToArray();

        // Convert to base64 string
        return $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";

    }
    [RelayCommand]
    private void MenuBarSelection()
    {
        //Shell.Current.FlyoutIsPresented = true;
        IsMenuVisible = !IsMenuVisible;
        IsMenuNotVisible = !IsMenuVisible;
    }

    [RelayCommand]
    private async Task NavigateToAnimalListAsync()
    {
        await RunBusyAsync(async () => await Shell.Current.GoToAsync($"{nameof(AnimalListPage)}"));
    }
    [RelayCommand]
    private async Task NavigateToHomePageAsync()
    {
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
    [RelayCommand]
    private async Task NavigateToAddAnimalPageAsync()
    {
        await RunBusyAsync(async () => await Shell.Current.GoToAsync(nameof(AddAnimalPage)));
    }

}