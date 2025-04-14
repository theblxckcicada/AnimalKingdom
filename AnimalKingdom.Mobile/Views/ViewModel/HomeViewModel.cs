using AnimalKingdom.Mobile.Views.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AnimalKingdom.Mobile.Views.ViewModel;

public partial class HomeViewModel : BaseViewModel
{
 
    public HomeViewModel()
    {
        GetRandomImage();
    }




}