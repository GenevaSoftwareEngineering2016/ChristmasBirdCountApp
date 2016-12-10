// (c) 2016 Geneva College Senior Software Project Team
using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using System.IO;
using Android.Content;
using ChristmasBirdCountApp.Email;

namespace ChristmasBirdCountApp
{
    [Activity(Label = "Bird Counter", MainLauncher = true, Icon = "@drawable/audubon_society2")]
    public class MainActivity : Activity
    {
        private List<BirdCount> masterBirdList;     // Most up-to-date list of all birds; Used by search function to add birds to "workingBirdList"
        private List<BirdCount> workingBirdList;    // List of all birds with counts 0+; This list is submitted with email report to Compiler
        private List<BirdCount> filteredBirdList;
        private Button btnAddBird;
        private ListView mListView;
        private EditText birdNameFilter;

        public static Stream FilePath { get; private set; }
        public int SelectedID = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            // Initialize Button Variables
            Button btnClear = FindViewById<Button>(Resource.Id.btnClear);
            Button btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnAddBird = FindViewById<Button>(Resource.Id.btnAddBirdMain);

            // Initialize Filter (Search) Box
            birdNameFilter = FindViewById<EditText>(Resource.Id.txtNameFilter);

            // Initialize ListView
            mListView = FindViewById<ListView>(Resource.Id.myListView);

            // Start Button Click Events
            birdNameFilter.TextChanged += BirdNameFilter_OnTextChanged;
            mListView.ItemClick += MListView_ItemClick;
            mListView.ItemLongClick += MListView_ItemLongClick;
            btnClear.Click += BtnClear_Click;
            btnSubmit.Click += BtnSubmit_Click;

            birdNameFilter.ClearFocus();    // Do not focus on text field for filter by default.

            // End Button Click Events
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
            btnAddBird.Click += AddBird_OnClick;

            // USE TEST 'workingBirdList' ENTRIES FOR TESTING SEARCH
            workingBirdList.Insert(0, new BirdCount() { Name = "Dove", Count = 0 });
            workingBirdList.Insert(0, new BirdCount() { Name = "Bluejay", Count = 0 });
            workingBirdList.Insert(0, new BirdCount() { Name = "Robin", Count = 0 });
            workingBirdList.Insert(0, new BirdCount() { Name = "Bald Eagle", Count = 0 });
            workingBirdList.Insert(0, new BirdCount() { Name = "Pidgeon", Count = 0 });
            workingBirdList.Insert(0, new BirdCount() { Name = "Canadian Goose", Count = 0 });
            workingBirdList.Insert(0, new BirdCount() { Name = "American Goose", Count = 0 });

            // END TESTING
        }

        protected override void OnStop()
        {
            // Save Existing List of Birds to .csv File
            BirdListFile.CreateWorkingBirdListFile(masterBirdList, workingBirdList);

            // Deregister Event Handlers
            btnAddBird.Click -= AddBird_OnClick;
            base.OnStop();
        }

        private void AddBird_OnClick(object sender, EventArgs e)
        {
            // Open the "Add Bird" PopUp
            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            AddBirdPopUp addBirdPopDialog = new AddBirdPopUp();
            addBirdPopDialog.AddBirdLists(masterBirdList, workingBirdList);
            addBirdPopDialog.Show(transaction, "Dialog Fragment");
        }

        private void BirdNameFilter_OnTextChanged(object sender, EventArgs e)
        {
            filteredBirdList = Search.FilterBirdCountList(birdNameFilter.Text, workingBirdList);
            mListView.Adapter = new row_adapter(this, filteredBirdList);
        }

        private void MListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            // Get selected bird info
            int id = e.Position;
            string birdName = workingBirdList[id].Name;
            int birdCount = workingBirdList[id].Count;

            // Pull up the dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            PopUpAdd PopAdd = new PopUpAdd(id, birdName, birdCount);
            PopAdd.Show(transaction, "Dialog Fragment");

            // Subscribe to events in PopUpAdd class
            PopAdd.OnAdd += PopAdd_OnAdd;
            PopAdd.OnEdit += PopAdd_OnEdit;
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
            workingBirdList.RemoveAt(e.id);
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

            workingBirdList.Insert(e.id, new BirdCount() { Name = e.birdName, Count = totalCount });
            mListView.Adapter = new row_adapter(this, workingBirdList);
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

