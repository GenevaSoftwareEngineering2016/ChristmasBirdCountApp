// (c) 2016 Geneva College Senior Software Project Team
using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using System.IO;
using Android.Views;
using Android.Content;

namespace ChristmasBirdCountApp
{
    [Activity(Label = "Bird Counter", MainLauncher = true, Icon = "@drawable/audubon_society2")]
    public class MainActivity : Activity
    {
        private List<BirdCount> birdList;
        private ListView mListView;

        public static Stream FilePath { get; private set; }
        public int SelectedID = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            // Set focus on Text Box
            FindViewById<EditText>(Resource.Id.txtname).RequestFocus();

            // Initialize Button Variables
            Button btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            Button btnClear = FindViewById<Button>(Resource.Id.btnClear);
            Button btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            
            // Initialize ListView
            mListView = FindViewById<ListView>(Resource.Id.myListView);

            // Start Button Click Events
            btnAdd.Click += delegate
            {
                string txtName = FindViewById<EditText>(Resource.Id.txtname).Text;
                if (txtName != "")
                {
                    birdList.Insert(0, new BirdCount() { Name = txtName, Count = 0 });
                    mListView.Adapter = new row_adapter(this, birdList);
                    FindViewById<EditText>(Resource.Id.txtname).Text = "";
                }
                else
                {
                    Toast.MakeText(this, "Please enter a bird name", ToastLength.Short).Show();
                }
                
            };

            mListView.ItemClick += MListView_ItemClick;
            mListView.ItemLongClick += MListView_ItemLongClick;
            btnClear.Click += BtnClear_Click;
            btnSubmit.Click += BtnSubmit_Click;
			
            // End Button Click Events

            // Calls the btnAdd Click Event when Enter Key is Pressed
            FindViewById<EditText>(Resource.Id.txtname).EditorAction += (sender, e) => {
                if (e.Event.Action == KeyEventActions.Down && e.Event.KeyCode == Keycode.Enter)
                {
                    btnAdd.PerformClick();
                }
                else
                {
                    e.Handled = false;
                }
            };

        }

        protected override void OnStart()
        {
            base.OnStart();

            // Load Existing List of Birds from .csv File
            birdList = BirdListFile.LoadBirdListFromFile();

            // Initialize ListView Adapter
            mListView.Adapter = new row_adapter(this, birdList);

            // Register Event Handlers
        }

        protected override void OnStop()
        {
            // Save Existing List of Birds to .csv File
            BirdListFile.CreateBirdListFile(birdList);

            // Deregister Event Handlers
            base.OnStop();
        }

        private void MListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            // get selected bird info
            int id = e.Position;
            string birdName = mItems[id].Name;
            int birdCount = mItems[id].Count;

            //Bundle args = new Bundle();
            //args.PutInt("birdCount", birdCount);
            //args.PutString("birdName", birdName);

            //pull up the dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            PopUp PopDialog = new PopUp(id, birdName, birdCount);
            PopDialog.Show(transaction, "Dialog Fragment");

            //subscribing to events in popup class
            PopDialog.OnDelete += PopDialog_OnDelete;
            PopDialog.OnUpdate += PopDialog_OnUpdate;

            // OLD CODE: first way of deleting a row
            //var alert = new AlertDialog.Builder(this);
            //SelectedID = e.Position;
            //alert.SetPositiveButton("Remove", removeClicked);
            //alert.SetNeutralButton("Clear Count", clearCount);
            //alert.SetNegativeButton("Close", closeClicked);

            //alert.Create().Show();
        }

        private void PopDialog_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            mItems.RemoveAt(e.id);
            int count = Int32.Parse(e.birdCount);
            mItems.Insert(e.id, new BirdCount() { Name = e.birdName, Count = count });
            mListView.Adapter = new row_adapter(this, mItems);
        }

        private void PopDialog_OnDelete(object sender, OnDeleteEventArgs e)
        {
            mItems.RemoveAt(e.id);
            mListView = FindViewById<ListView>(Resource.Id.myListView);
            mListView.Adapter = new row_adapter(this, mItems);
        }

        //private void closeClicked(object sender, DialogClickEventArgs e)
        //{
            
        //}

        //private void clearCount(object sender, DialogClickEventArgs e)
        //{
        //    mItems[SelectedID].Count = 0;
        //    mListView = FindViewById<ListView>(Resource.Id.myListView);
        //    mListView.Adapter = new row_adapter(this, mItems);

        //    Toast.MakeText(this, mItems[SelectedID].Name + "'s Count has been set to \"0\"", ToastLength.Short).Show();
        //}

        //private void removeClicked(object sender, DialogClickEventArgs e)
        //{
        //    string deletedName = mItems[SelectedID].Name;

        //    mItems.RemoveAt(SelectedID);
        //    mListView = FindViewById<ListView>(Resource.Id.myListView);
        //    mListView.Adapter = new row_adapter(this, mItems);
           
        //    Toast.MakeText(this, deletedName + " has been removed", ToastLength.Short).Show();
            
        //}

        private void BtnClear_Click(object sender, System.EventArgs e)
        {
            birdList.Clear();
            mListView = FindViewById<ListView>(Resource.Id.myListView);
            mListView.Adapter = new row_adapter(this, birdList);
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            birdList[e.Position].Count++;

            mListView = FindViewById<ListView>(Resource.Id.myListView);
            mListView.Adapter = new row_adapter(this, birdList);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            // Save Existing List of Birds to .csv File
            BirdListFile.CreateBirdListFile(birdList);

            // Start New Intent to Open New Screen for Submit Form
            var intent = new Intent(this, typeof(EmailFormActivity));
            StartActivity(intent);

            SetContentView(Resource.Layout.EmailForm);
        }
    }
}

