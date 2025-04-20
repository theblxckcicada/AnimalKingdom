using AnimalKingdom.Mobile.MSALClient;
using AnimalKingdom.Mobile.Services;
using AnimalKingdom.Mobile.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Identity.Client;

namespace AnimalKingdom.Mobile.Views.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    public bool IsNotBusy => !IsBusy;
    [ObservableProperty]
    private string randomImage;
    [ObservableProperty]
    bool isMenuVisible;

    [ObservableProperty]
    bool isMenuNotVisible = true;
    readonly Random random = new();

    [ObservableProperty]
    private IAccount account;

    [ObservableProperty]
    private bool loggedIn;

    [ObservableProperty]
    private string accessToken;

    public bool NotLoggedIn => !LoggedIn;


    public BaseViewModel()
    {


    }
    partial void OnLoggedInChanged(bool value)
    {
        OnPropertyChanged(nameof(NotLoggedIn));
    }

    partial void OnIsBusyChanged(bool value)
    {
        OnPropertyChanged(nameof(IsNotBusy));
    }

    public async Task CheckAccountStatusAsync()
    {
        var cachedUserAccount = await  RunBusyAsync(()=> PublicClientSingleton.Instance.MSALClientHelper.FetchSignedInUserFromCache());
        LoggedIn = cachedUserAccount != null;

        if (cachedUserAccount is not null)
        {
            Account = cachedUserAccount;
        }
    }

    public async Task AquireAccessTokenAsync(AnimalService animalService)
    {
        _ = await PublicClientSingleton.Instance.AcquireTokenSilentAsync();

        // update access token 
        AccessToken = PublicClientSingleton.Instance.MSALClientHelper.AuthResult.AccessToken;
        animalService.SetAccessToken(AccessToken);
    }

    public void GetRandomImage()
    {
        RandomImage = $"animal_{(random.Next(1, 7))}.jpg";
    }
    protected async Task RunBusyAsync(Func<Task> action)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            await Task.Delay(100);
            await action();
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected async Task<T> RunBusyAsync<T>(Func<Task<T>> action)
    {
        IsBusy = true;
        try
        {
            await Task.Delay(100);
            return await action();
        }
        finally
        {
            IsBusy = false;
        }
    }
    protected static async Task<string> ConvertImageToBase64String(Stream stream)
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
        var currentPage = Shell.Current.CurrentPage;
        var targetRoute = nameof(AnimalListPage);

        // Avoid navigating if already on the target page
        if (currentPage?.GetType().Name == targetRoute)
            return;

        await RunBusyAsync(() => Shell.Current.GoToAsync(targetRoute));
    }

    [RelayCommand]
    private async Task NavigateToHomePageAsync()
    {
        var currentPage = Shell.Current.CurrentPage;
        var targetRoute = nameof(HomePage);

        // Avoid navigating if already on the target page
        if (currentPage?.GetType().Name == targetRoute)
            return;
        await RunBusyAsync(() => Shell.Current.GoToAsync($"//{nameof(HomePage)}"));
    }
    [RelayCommand]
    private async Task NavigateToAddAnimalPageAsync()
    {
        var currentPage = Shell.Current.CurrentPage;
        var targetRoute = nameof(AddAnimalPage);

        // Avoid navigating if already on the target page
        if (currentPage?.GetType().Name == targetRoute)
            return;
        await RunBusyAsync(() => Shell.Current.GoToAsync(nameof(AddAnimalPage)));
    }

    [RelayCommand]
    private async Task AdminLoginAsync()
    {
        await RunBusyAsync(async () =>
        {
            try
            {
                await PublicClientSingleton.Instance.AcquireTokenSilentAsync();

                // Only navigate if login succeeds
                await Shell.Current.GoToAsync(nameof(AnimalListPage));
            }
            catch (MsalUiRequiredException)
            {
                // Silent login failed; optionally prompt user or show message
                await Shell.Current.DisplayAlert("Login Required", "Please login interactively.", "OK");
            }
            catch (Exception ex)
            {
                // Log or handle unexpected errors
                await Shell.Current.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
        });
    }

    [RelayCommand]
    private async Task AdminLogoutAsync()
    {
        await RunBusyAsync(async () =>
        {
            try
            {
                await PublicClientSingleton.Instance.SignOutAsync();

                // Optional: clear state, account, tokens, etc.
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to sign out: {ex.Message}", "OK");
            }

            // Navigate away regardless of logout success
            await Shell.Current.GoToAsync(nameof(AnimalListPage));
        });
    }


}