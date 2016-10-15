// (c) 2016 Geneva College Senior Software Project Team
using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace ChristmasBirdCountApp
{
    [Activity(Label = "ChristmasBirdCountApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private List<BirdCount> mItems;
        private ListView mListView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            mListView = FindViewById<ListView>(Resource.Id.myListView);

            mItems = new List<BirdCount>();

            //ArrayAdapter<BirdCount> adapter = new ArrayAdapter<BirdCount>(this, Android.Resource.Layout.SimpleListItem1, mItems);

            //mListView.Adapter = adapter;
            mListView.Adapter = new row_adapter(this, mItems);

            btnAdd.Click += delegate {
                //mItems.Add(txtName);
                string txtName = FindViewById<EditText>(Resource.Id.txtname).Text;
                //adapter.Insert(new BirdCount() { name = txtName, count = "0" }, 0);
                mItems.Add(new BirdCount() { name = txtName, count = "0" });
                //((ArrayAdapter)mListView.Adapter).NotifyDataSetChanged();
                FindViewById<EditText>(Resource.Id.txtname).Text = "";
            };



            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

