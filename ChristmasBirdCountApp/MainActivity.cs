// (c) 2016 Geneva College Senior Software Project Team
using Android.App;
using Android.Widget;
using Android.OS;

namespace ChristmasBirdCountApp
{
    [Activity(Label = "ChristmasBirdCountApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

