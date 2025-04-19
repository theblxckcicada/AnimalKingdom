using AnimalKingdom.Mobile.MSALClient;
using Foundation;
using Microsoft.Identity.Client;
using UIKit;

namespace AnimalKingdom.Mobile;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // Initialize MSAL and platformConfig is set
        IAccount existinguser = Task.Run(PublicClientSingleton.Instance.MSALClientHelper.InitializePublicClientAppAsync).Result;

        return base.FinishedLaunching(application, launchOptions);
    }
}
