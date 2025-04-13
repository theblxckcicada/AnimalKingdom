using CommunityToolkit.Mvvm.ComponentModel;

namespace AnimalKingdom.Mobile.Views.ViewModel;

public partial class BaseViewModel : ObservableObject
{
	[ObservableProperty]
	private bool isBusy;

    [ObservableProperty]
    private bool isNotBusy;

    public BaseViewModel()
	{
		
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



}