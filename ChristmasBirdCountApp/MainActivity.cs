// (c) 2016 Geneva College Senior Software Project Team
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
        private List<BirdCount> masterBirdList;     // Most up-to-date list of all birds; Used by search function to add birds to "workingBirdList"
        private List<BirdCount> workingBirdList;    // List of all birds with counts 0+; This list is submitted with email report to Compiler
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
                    workingBirdList.Insert(0, new BirdCount() { Name = txtName, Count = 0 });
                    mListView.Adapter = new row_adapter(this, workingBirdList);
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

            // Load the "Master" Bird List
            Context appContext = this.ApplicationContext;
            masterBirdList = BirdListFile.LoadMasterBirdList(appContext);

            // Load Existing "Working" List of Birds from .csv File
            workingBirdList = BirdListFile.LoadWorkingBirdListFromFile();

            // Update the "Working" bird list from the "Master" bird list
            //workingBirdList = BirdListFile.UpdateWorkingBirdListFromMaster(masterBirdList, workingBirdList);

            // Initialize ListView Adapter
            mListView.Adapter = new row_adapter(this, workingBirdList);

            // Register Event Handlers
        }

        protected override void OnStop()
        {
            // Save Existing List of Birds to .csv File
            BirdListFile.CreateWorkingBirdListFile(masterBirdList, workingBirdList);

            // Deregister Event Handlers
            base.OnStop();
        }

        private void MListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            // get selected bird info
            int id = e.Position;
            string birdName = workingBirdList[id].Name;
            int birdCount = workingBirdList[id].Count;

            //pull up the dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            PopUpAdd PopAdd = new PopUpAdd(id, birdName, birdCount);
            PopAdd.Show(transaction, "Dialog Fragment");

            //Subscribe to events in PopUpAdd class
            PopAdd.OnAdd += PopAdd_OnAdd;
            PopAdd.OnEdit += PopAdd_OnEdit;

            //PopUp PopDialog = new PopUp(id, birdName, birdCount);
            //PopDialog.Show(transaction, "Dialog Fragment");

            ////subscribing to events in popup class
            //PopDialog.OnDelete += PopDialog_OnDelete;
            //PopDialog.OnUpdate += PopDialog_OnUpdate;
            
        }

        private void PopAdd_OnEdit(object sender, OnEditEventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            PopUp PopDialog = new PopUp(e.id, e.birdName, e.birdCount);
            PopDialog.Show(transaction, "Dialog Fragment");

            //subscribing to events in popup class
            PopDialog.OnDelete += PopDialog_OnDelete;
            PopDialog.OnUpdate += PopDialog_OnUpdate;
        }

        private void PopAdd_OnAdd(object sender, OnAddEventArgs e)
        {
            birdList.RemoveAt(e.id);
            int totalCount;
            int addBirds;
            if (e.addNumber == "")
            {
                addBirds = 0;
            }
            else
            {
                addBirds = Int32.Parse(e.addNumber);
            }

            //add count to existing bird count
            totalCount = e.birdCount + addBirds;

            birdList.Insert(e.id, new BirdCount() { Name = e.birdName, Count = totalCount });
            mListView.Adapter = new row_adapter(this, birdList);
        }

        private void PopDialog_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            workingBirdList.RemoveAt(e.id);
            int count = 0;
            if (e.birdCount == "")
            {
                count = 0;
            }
            else
            {
                count = Int32.Parse(e.birdCount);
            }
            
            workingBirdList.Insert(e.id, new BirdCount() { Name = e.birdName, Count = count });
            mListView.Adapter = new row_adapter(this, workingBirdList);
        }

        private void PopDialog_OnDelete(object sender, OnDeleteEventArgs e)
        {
            workingBirdList.RemoveAt(e.id);
            mListView = FindViewById<ListView>(Resource.Id.myListView);
            mListView.Adapter = new row_adapter(this, workingBirdList);
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
            workingBirdList.Clear();
            mListView = FindViewById<ListView>(Resource.Id.myListView);
            mListView.Adapter = new row_adapter(this, workingBirdList);
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            workingBirdList[e.Position].Count++;

            mListView = FindViewById<ListView>(Resource.Id.myListView);
            mListView.Adapter = new row_adapter(this, workingBirdList);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            // Save Existing List of Birds to .csv File
            BirdListFile.CreateWorkingBirdListFile(masterBirdList, workingBirdList);

            // Start New Intent to Open New Screen for Submit Form
            var intent = new Intent(this, typeof(EmailFormActivity));
            StartActivity(intent);

            SetContentView(Resource.Layout.EmailForm);
        }
    }
}

