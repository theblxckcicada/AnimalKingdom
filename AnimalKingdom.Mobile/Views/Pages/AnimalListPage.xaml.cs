using AnimalKingdom.Mobile.Views.ViewModel;
using Microsoft.Maui.Controls;

namespace AnimalKingdom.Mobile.Views.Pages;

public partial class AnimalListPage : ContentPage
{
    private readonly AnimalListViewModel viewModel;
    private CancellationTokenSource _debounceCts;
    private string searchText;
    public AnimalListPage(AnimalListViewModel _viewModel)
    {
         InitializeComponent();
        viewModel = _viewModel;
        BindingContext = _viewModel;
       
    }


    protected async  override void OnAppearing()
    {
            base.OnAppearing();
            await viewModel.CheckAccountStatusAsync();
            viewModel.GetRandomImage();
            // update the collection view and RowDefinitions for admin and visitors

            AnimalCollectionView.SelectionMode = viewModel.LoggedIn ? SelectionMode.None: SelectionMode.Single;
            if (viewModel.LoggedIn)
            {
                GridContent.RowDefinitions =
                    [
                        new RowDefinition { Height = new GridLength(50) },
                        new RowDefinition { Height = new GridLength(50) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition { Height = new GridLength(80) }
                    ];
            }
            else
            {
                GridContent.RowDefinitions =
                    [
                        new RowDefinition { Height = new GridLength(50) },
                        new RowDefinition { Height = new GridLength(50) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
               
                    ];
            }

        
        if (viewModel?.AnimalSearchTextChangedCommand?.CanExecute(searchText) == true)
        {
            viewModel.AnimalSearchTextChangedCommand.Execute(searchText);
        }
    }

    private async void AnimalSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _debounceCts?.Cancel(); // Cancel any ongoing delay
        _debounceCts = new CancellationTokenSource();
        var token = _debounceCts.Token;

        try
        {
            await Task.Delay(300, token); // Wait 300ms, or cancel if new input comes in

            if (viewModel?.AnimalSearchTextChangedCommand?.CanExecute(e.NewTextValue) == true)
            {
                viewModel.AnimalSearchTextChangedCommand.Execute(e.NewTextValue);
                this.searchText = e.NewTextValue;
            }
        }
        catch (TaskCanceledException)
        {
            // Ignore the exception, since it's expected during rapid typing
        }
    }


    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var collectionView = (CollectionView)sender;
        collectionView.SelectedItem = null;
    }
    private async Task Update_LayoutAsync(object sender, EventArgs e)
    {

            var content = (viewModel.IsMenuNotVisible
                ? GridContent.TranslateTo(250, 150, 800, Easing.Linear)
                : GridContent.TranslateTo(0, 0, 800, Easing.Linear));

            GridMenu.TranslationX = -300;
            var menu = (viewModel.IsMenuNotVisible
               ? GridMenu.TranslateTo(0, 0, 800, Easing.Linear)
               : GridMenu.TranslateTo(-300, 0, 800, Easing.Linear));

            await Task.WhenAll(content, menu);
            if (viewModel?.MenuBarSelectionCommand?.CanExecute(e) == true)
            {
                viewModel.MenuBarSelectionCommand.Execute(e);
            }
        
    }
    private async void Update_Layout_Clicked(object sender, EventArgs e)
    {
        await Update_LayoutAsync(sender, e);

    }
}