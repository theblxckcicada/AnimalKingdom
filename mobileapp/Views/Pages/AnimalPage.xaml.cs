using mobileapp.Data;
using System.Collections.ObjectModel;
using mobileapp.Models;
using CommunityToolkit.Mvvm.Input;
using mobileapp.Views.ViewModel;

namespace mobileapp.Views.Pages;

public partial class AnimalPage : ContentPage
{
    private readonly AnimalViewModel _viewModel;
    public AnimalPage(AnimalViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void AnimalSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(_viewModel?.AnimalSearchTextChangedCommand?.CanExecute(e.NewTextValue) == true)
        {
            _viewModel.AnimalSearchTextChangedCommand.Execute(e.NewTextValue);
        }
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var collectionView = (CollectionView)sender;
        collectionView.SelectedItem = null;
    }
}