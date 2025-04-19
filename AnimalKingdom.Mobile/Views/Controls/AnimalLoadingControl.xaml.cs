namespace AnimalKingdom.Mobile.Views.Controls;

public partial class AnimalLoadingControl : ContentView
{
    public AnimalLoadingControl()
    {
        InitializeComponent();
        StartAnimations();
    }

    private void StartAnimations()
    {
        // Main spinner rotation
        var mainRotation = new Animation(v => MainSpinner.Rotation = v, 0, 360, Easing.Linear);
        mainRotation.Commit(this, "mainRotation", length: 2000, repeat: () => true);


        // Core pulse animation
        var corePulse = new Animation {
            { 0, 0.5, new Animation(v => Core.Scale = v, 1, 1.2, Easing.SinInOut) },
            { 0.5, 1, new Animation(v => Core.Scale = v, 1.2, 1, Easing.SinInOut) }
        };
        corePulse.Commit(this, "corePulse", length: 1500, repeat: () => true);

        // Gradient border pulse
        var borderPulse = new Animation {
            { 0, 0.5, new Animation(v => MainSpinner.Scale = v, 1, 1.05, Easing.SinInOut) },
            { 0.5, 1, new Animation(v => MainSpinner.Scale = v, 1.05, 1, Easing.SinInOut) }
        };
        borderPulse.Commit(this, "borderPulse", length: 1500, repeat: () => true);
    }


}
