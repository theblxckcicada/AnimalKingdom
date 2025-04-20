// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Android.App;
using Android.Content;
using Microsoft.Identity.Client;

namespace AnimalKingdom.Mobile.Platforms.Android
{
    [Activity(Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
        DataHost = "auth",
        DataScheme = "msala2c25013-1db5-4526-889e-198caf1c9cd4")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}
