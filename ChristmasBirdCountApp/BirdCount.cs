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

namespace ChristmasBirdCountApp
{
    class BirdCount
    {
        public string name { get; set; }
        public string count { get; set; }


        public override string ToString()
        {
            return name; 
        }
    }
}