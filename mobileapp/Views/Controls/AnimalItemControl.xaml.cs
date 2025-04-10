using Microsoft.Maui.Controls;
using mobileapp.Models;

namespace mobileapp.Views.Controls;

public partial class AnimalItemControl : ContentView
{
    public event EventHandler<EventArgs> OnUpdate;
    public event EventHandler<EventArgs> OnCancel;
    public AnimalItemControl() => InitializeComponent();

    public string Name
    {
        get
        {
            return AnimalName.Text;
        }
        set { AnimalName.Text = value; }
    }

    public string Description
    {
        get => AnimalDescription.Text;
        set
        {
            AnimalDescription.Text = value;
        }
    }
    public string Image
    {
        get => AnimalImage.Source.ToString()!;

        set => AnimalImage.Source = value != null ? ImageSource.FromFile(value) : null;
    }

    public AnimalCategory Category
    {
        get => Enum.Parse<AnimalCategory>(AnimalCategory.Text);
        set { AnimalCategory.Text = value.ToString(); }
    }


    private void BtnUpdate_Clicked(object sender, EventArgs e)
    {
        OnUpdate?.Invoke(sender, e);
    }
    private void BtnCancel_Clicked(object sender, EventArgs e)
    {
        OnCancel?.Invoke(sender, e);
    }

}