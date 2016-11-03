using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ChristmasBirdCountApp.Resources
{
    [Activity(Label = "EmailFormActivity")]
    public class EmailFormActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EmailForm);

            // Create your application here
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