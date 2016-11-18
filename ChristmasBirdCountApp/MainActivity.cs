﻿// (c) 2016 Geneva College Senior Software Project Team
using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using System.IO;
using Android.Views;
using Android.Content;
using ChristmasBirdCountApp.Email;

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
            var alert = new AlertDialog.Builder(this);
            SelectedID = e.Position;
            alert.SetPositiveButton("Remove", Remove_Click);
            alert.SetNeutralButton("Clear Count", ClearCount_Click);
            alert.SetNegativeButton("Close", Close_Click);

            alert.Create().Show();
        }

        private void Close_Click(object sender, DialogClickEventArgs e)
        {
            // Closes Long-Click Modal
        }

        private void ClearCount_Click(object sender, DialogClickEventArgs e)
        {
            birdList[SelectedID].Count = 0;
            mListView = FindViewById<ListView>(Resource.Id.myListView);
            mListView.Adapter = new row_adapter(this, birdList);

            Toast.MakeText(this, birdList[SelectedID].Name + "'s Count has been set to \"0\"", ToastLength.Short).Show();
        }

        private void Remove_Click(object sender, DialogClickEventArgs e)
        {
            string deletedName = birdList[SelectedID].Name;

            birdList.RemoveAt(SelectedID);
            mListView = FindViewById<ListView>(Resource.Id.myListView);
            mListView.Adapter = new row_adapter(this, birdList);
           
            Toast.MakeText(this, deletedName + " has been removed", ToastLength.Short).Show();    
        }

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

