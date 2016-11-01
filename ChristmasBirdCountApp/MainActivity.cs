// (c) 2016 Geneva College Senior Software Project Team
using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using System.IO;

namespace ChristmasBirdCountApp
{
    [Activity(Label = "Bird Counter", MainLauncher = true, Icon = "@drawable/audubon_society2")]
    public class MainActivity : Activity
    {
        private List<BirdCount> mItems;
        private ListView mListView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            FindViewById<EditText>(Resource.Id.txtname).RequestFocus();
            Button btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            Button btnClear = FindViewById<Button>(Resource.Id.btnClear);
            Button btnSave = FindViewById<Button>(Resource.Id.btnSave);
            mListView = FindViewById<ListView>(Resource.Id.myListView);

            //mItems = new List<BirdCount>();

            mItems = new List<BirdCount>
            {
                new BirdCount { Name = "Robin", Count = 8 },
                new BirdCount { Name = "Blue Jay", Count = 10 },
                new BirdCount { Name = "Cardinal", Count = 12 }
            };

            //ArrayAdapter<BirdCount> adapter = new ArrayAdapter<BirdCount>(this, Android.Resource.Layout.SimpleListItem1, mItems);

            //mListView.Adapter = adapter;
            mListView.Adapter = new row_adapter(this, mItems);

            btnAdd.Click += delegate
            {
                string txtName = FindViewById<EditText>(Resource.Id.txtname).Text;
                if (txtName != "")
                {
                    mItems.Insert(0, new BirdCount() { Name = txtName, Count = 0 });
                    mListView.Adapter = new row_adapter(this, mItems);
                    FindViewById<EditText>(Resource.Id.txtname).Text = "";
                }
                else
                {
                    Toast.MakeText(this, "Please enter a bird name", ToastLength.Short).Show();
                }
                
            };

            mListView.ItemClick += MListView_ItemClick;
            btnClear.Click += BtnClear_Click;
            btnSave.Click += BtnSave_Click;

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }

        private void BtnSave_Click(object sender, System.EventArgs e)
        {
            //    string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/filetemp.txt");
            //    FileStream fs = null;
            //    bool bExists = false;
            //    if (File.Exists(path))
            //    {
            //        fs = new FileStream(path, FileMode.Open);
            //        bExists = true;
            //    }
            //    else
            //    {
            //        fs = new FileStream(path, FileMode.Create);
            //    }
            //    StreamWriter sw = new StreamWriter(fs);
            //if (bExists)
            //{
            //    sw.Write("¬" + t.Result.Text);
            //}
            //else
            //{
            //    sw.Write(t.Result.Text);
            //}

            //    foreach (var item in mItems)
            //    {
            //        sw.Write(item.Name);
            //    }

            //    fs.Close();
        }

        private void BtnClear_Click(object sender, System.EventArgs e)
        {
            mItems.Clear();
            mListView = FindViewById<ListView>(Resource.Id.myListView);
            mListView.Adapter = new row_adapter(this, mItems);
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            mItems[e.Position].Count++;

            mListView = FindViewById<ListView>(Resource.Id.myListView);
            mListView.Adapter = new row_adapter(this, mItems);
        }
    }
}

