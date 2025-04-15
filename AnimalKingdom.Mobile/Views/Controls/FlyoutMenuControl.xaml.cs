using AnimalKingdom.Mobile.Views.ViewModel;

namespace AnimalKingdom.Mobile.Views.Controls;

public partial class FlyoutMenuControl : ContentView
{
    public event EventHandler<EventArgs> FlyoutMenuClick;
	public FlyoutMenuControl()
	{
		InitializeComponent();
	}

    private  async void Update_Layout_Clicked(object sender, EventArgs e)
    {
        FlyoutMenuClick?.Invoke(sender, e);

    }
}