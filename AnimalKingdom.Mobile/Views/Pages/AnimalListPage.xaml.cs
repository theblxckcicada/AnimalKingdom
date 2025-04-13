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
}