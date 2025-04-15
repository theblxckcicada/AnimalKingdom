using AndroidX.Lifecycle;
using AnimalKingdom.Mobile.Views.ViewModel;
using CommunityToolkit.Maui.Animations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AnimalKingdom.Mobile.Views.Pages;

public partial class HomePage : ContentPage
{

    private Animation fadeInAnimation;
    HomeViewModel viewModel;
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }


    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is HomeViewModel viewModel)
        {
            viewModel.GetRandomImage();
            GridLabel.Opacity = 0;
            GridLabel.TranslationY = 200;

            fadeInAnimation = new Animation
            {
                { 0, 1, new Animation(v => GridLabel.TranslationY = v, 0, 0, Easing.CubicOut) },
                { 0, 1, new Animation(v => GridLabel.Opacity = v, 0, 1, Easing.CubicIn) }
            };

            fadeInAnimation.Commit(this, "FadeInFromBottom", length: 1000, easing: Easing.Linear);


        }
    }

    private async Task Update_LayoutAsync(object sender, EventArgs e)
    {
        if (BindingContext is HomeViewModel viewModel)
        {
            var content = (viewModel.IsMenuNotVisible
                ? GridContent.TranslateTo(250, 0, 800, Easing.Linear)
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
        await Update_LayoutAsync(sender,e);

    }
}