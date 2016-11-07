using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace ChristmasBirdCountApp
{
    [Activity(Label = "Bird Counter", Icon = "@drawable/audubon_society2")]
    public class EmailFormActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EmailForm);
            Button btnReturn = FindViewById<Button>(Resource.Id.btnReturn);
            btnReturn.Click += BtnReturn_Click;

            // Create your application here
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);

            SetContentView(Resource.Layout.Main);
        }

        protected override void OnStart()
        {
            base.OnStart();
            // Register event handlers
        }

        protected override void OnStop()
        {
            // Deregister event handlers
            base.OnStop();
        }
    }
}