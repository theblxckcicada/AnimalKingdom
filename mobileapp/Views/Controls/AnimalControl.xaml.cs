
using Microsoft.Maui.Controls;
using mobileapp.Models;

namespace mobileapp.Views.Controls;

public partial class AnimalControl : ContentView
{
	public event EventHandler<string> OnError;
	public event EventHandler<EventArgs> OnSave;
	public event EventHandler<EventArgs> OnCancel;
	public event EventHandler<EventArgs> OnDelete;
	public event EventHandler<EventArgs> OnUpdate;


	public AnimalControl()
	{
		InitializeComponent();
		AnimalCategoryList.ItemsSource =CategoryList;

    }

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
		get
		{
			return AnimalDescription.Text;
		}
		set
		{
			AnimalDescription.Text = value;
		}
	}
	public string Image
	{
		get => AnimalImage.Source.ToString()!.Replace("File: ","");

		set => AnimalImage.Source = value ;
	}

    public AnimalCategory Category
    {
        get
        {
            if (AnimalCategoryList.SelectedIndex == -1)
                return default; // or throw an exception if you prefer

            return Enum.Parse<AnimalCategory>(AnimalCategoryList.SelectedItem.ToString());
        }
        set => AnimalCategoryList.SelectedItem = value.ToString();
    }

    public static List<string> CategoryList => [.. Enum.GetNames<AnimalCategory>()];
    public void BtnSaveAnimal_Clicked(object sender, EventArgs e)
	{
		OnSave?.Invoke(sender, e);
	}
	public void BtnDeleteAnimal_Clicked(object sender, EventArgs e)
	{
		OnDelete?.Invoke(sender, e);
	}
	public void BtnCancel_Clicked(object sender, EventArgs e)
	{
		OnCancel?.Invoke(sender, e);
	}

	private void BtnUpdate_Clicked(object sender, EventArgs e)
	{
		OnUpdate?.Invoke(sender, e);
	}

    private async void BtnPickImage_Clicked(object sender, EventArgs e)
    {
        FileResult result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images
        });

        if (result == null) return;
        if (result != null)
        {

            using var stream = await result.OpenReadAsync();
            // Set the image source
            AnimalImage.Source = ImageSource.FromStream(() => stream);

            // Reset the stream position to beginning (since ImageSource.FromStream might have read it)
            stream.Position = 0;

            // Convert stream to byte array
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            // Convert to base64 string
            string base64String = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";

            AnimalImage.Source = base64String;

        }

    }

    private void Category_SelectedIndexChanged(object sender, EventArgs e)
    {
		Category = Enum.Parse<AnimalCategory>(AnimalCategoryList.SelectedItem.ToString());
    }
}