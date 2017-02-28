// (c) 2017 Geneva College Senior Software Project Team
using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;


namespace ChristmasBirdCountApp.Email
{
    [Activity(Label = "Bird Counter", Icon = "@drawable/audubon_society2", Theme = "@style/CustomActionBarTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, ScreenOrientation = ScreenOrientation.Portrait)]
    public class FeederEmailFormActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}