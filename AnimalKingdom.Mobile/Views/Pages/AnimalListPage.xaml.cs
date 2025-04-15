using AnimalKingdom.Mobile.Views.ViewModel;

namespace AnimalKingdom.Mobile.Views.Pages;

public partial class AnimalListPage : ContentPage
{
    private readonly AnimalListViewModel _viewModel;
    private CancellationTokenSource _debounceCts;
    public AnimalListPage(AnimalListViewModel viewModel)
    {
         InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AnimalListViewModel viewModel)
        {
            viewModel.GetRandomImage();
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

            if (_viewModel?.AnimalSearchTextChangedCommand?.CanExecute(e.NewTextValue) == true)
            {
                _viewModel.AnimalSearchTextChangedCommand.Execute(e.NewTextValue);
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
        if (BindingContext is AnimalListViewModel viewModel)
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
    }
    private async void Update_Layout_Clicked(object sender, EventArgs e)
    {
        await Update_LayoutAsync(sender, e);

    }
}