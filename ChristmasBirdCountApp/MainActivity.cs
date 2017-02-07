// (c) 2016 Geneva College Senior Software Project Team
using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using System.IO;
using Android.Content;
using Android.Content.PM;
using ChristmasBirdCountApp.Email;

namespace ChristmasBirdCountApp
{
    [Activity(Label = "Bird Counter", MainLauncher = true, Icon = "@drawable/whiteBird", Theme = "@style/CustomActionBarTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        private List<BirdCount> masterBirdList;     // Most up-to-date list of all birds; Used by search function to add birds to "workingBirdList"
        private List<BirdCount> workingBirdList;    // List of all birds with counts 0+; This list is submitted with email report to Compiler
        private List<BirdCount> filteredBirdList;
        //private Button btnAddBird;
        //private Button btnClear;
        //private Button btnSubmit;
        private ImageButton ibClearFilter;
        LinearLayout llClear;
        LinearLayout llAdd;
        LinearLayout llSubmit;
        private ListView userBirdListView;
        private EditText birdNameFilter;

        public static Stream FilePath { get; private set; }
        public static int totalSpeciesSeen;     // The total number of various bird species seen (number of birds added to "workingBirdList")
        public static int totalBirdsSeen;       // The total number of all birds seen (across all species in "workingBirdList")
        public int SelectedID = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetCustomView(Resource.Layout.actionBar);
            ActionBar.SetDisplayShowCustomEnabled(true);

            SetContentView(Resource.Layout.Main);

            // Initialize Button Variables
            //Button btnClear = FindViewById<Button>(Resource.Id.btnClear);
            //Button btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            //btnAddBird = FindViewById<Button>(Resource.Id.btnAddBirdMain);
            ibClearFilter = FindViewById<ImageButton>(Resource.Id.ibClearFilter);

            llClear = FindViewById<LinearLayout>(Resource.Id.llClear);
            llAdd = FindViewById<LinearLayout>(Resource.Id.llAdd);
            llSubmit = FindViewById<LinearLayout>(Resource.Id.llSubmit);
            

            // Initialize Filter (Search) Box
            birdNameFilter = FindViewById<EditText>(Resource.Id.txtNameFilter);

            // Initialize ListView
            userBirdListView = FindViewById<ListView>(Resource.Id.myListView);

            // Start Button Click Events
            birdNameFilter.TextChanged += BirdNameFilter_OnTextChanged;
            userBirdListView.ItemClick += MListView_ItemClick;
            userBirdListView.ItemLongClick += MListView_ItemLongClick;
            //btnClear.Click += BtnClear_Click;
            //btnSubmit.Click += BtnSubmit_Click;
            llClear.Click += BtnClear_Click;
            llSubmit.Click += BtnSubmit_Click;

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
            filteredBirdList = workingBirdList;

            // Update the "Working" bird list from the "Master" bird list
            //workingBirdList = BirdListFile.UpdateWorkingBirdListFromMaster(masterBirdList, workingBirdList);

            // Initialize ListView Adapter
            userBirdListView.Adapter = new row_adapter(this, filteredBirdList);

            // Register Event Handlers
            //btnAddBird.Click += AddBird_OnClick;
            ibClearFilter.Click += IBClearFilter_OnClick;
            llAdd.Click += AddBird_OnClick;

            if (workingBirdList.Count == 0)
            {
                //btnClear.Enabled = false;
                llClear.Enabled = false;
                llClear.SetBackgroundColor(Android.Graphics.Color.LightGray);
            }
            else
            {
                //btnClear.Enabled = true;
                llClear.Enabled = true;
                llClear.SetBackgroundResource(Resource.Drawable.selector);

            }
        }

        protected override void OnStop()
        {
            // Save Existing List of Birds to .csv File
            BirdListFile.CreateWorkingBirdListFile(masterBirdList, workingBirdList);

            // Deregister Event Handlers
            //btnAddBird.Click -= AddBird_OnClick;
            ibClearFilter.Click -= IBClearFilter_OnClick;
            llAdd.Click -= AddBird_OnClick;

            base.OnStop();
        }

        private void AddBird_OnClick(object sender, EventArgs e)
        {
            // Open the "Add Bird" PopUp
            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            AddBirdPopUp addBirdPopDialog = new AddBirdPopUp();
            addBirdPopDialog.AddBirdLists(masterBirdList, workingBirdList);
            addBirdPopDialog.Show(transaction, "Dialog Fragment");

            addBirdPopDialog.OnTap += AddBirdPopDialog_OnTap;
        }

        private void AddBirdPopDialog_OnTap(object sender, OnTapEventArgs e)
        {
            workingBirdList.Add(new BirdCount() { Name = e.birdName, Count = 0, InList = true});

            filteredBirdList = workingBirdList;     // Update the filtered bird list

            userBirdListView = FindViewById<ListView>(Resource.Id.myListView);
            userBirdListView.Adapter = new row_adapter(this, filteredBirdList);

            // Enable the "Clear" button if the list now has 1+ items in it.
            if (workingBirdList.Count == 0)
            {
                //btnClear.Enabled = false;
                llClear.Enabled = false;
                llClear.SetBackgroundColor(Android.Graphics.Color.LightGray);
            }
            else
            {
                //btnClear.Enabled = true;
                llClear.Enabled = true;
                llClear.SetBackgroundResource(Resource.Drawable.selector);
            }
        }

        private void BirdNameFilter_OnTextChanged(object sender, EventArgs e)
        {
            filteredBirdList = Search.FilterBirdCountList(birdNameFilter.Text, workingBirdList);
            userBirdListView.Adapter = new row_adapter(this, filteredBirdList);
        }

        private void MListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            // Get selected bird info
            int id = e.Position;
            string birdName = filteredBirdList[id].Name;
            int birdCount = filteredBirdList[id].Count;

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
            string birdName = filteredBirdList[e.id].Name;
            int birdIndex = 0;

            foreach (var bird in workingBirdList)
            {
                if (bird.Name == birdName)
                {
                    birdIndex = workingBirdList.IndexOf(bird);  // Get the index of the bird in the "workingBirdList," and remove the bird at that index.
                }
                else
                {
                    // The matching bird in the "workingBirdList" has not been found (yet).
                }
            }

            workingBirdList.RemoveAt(birdIndex);

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
            var totalCount = e.birdCount + addBirds;

            workingBirdList.Insert(birdIndex, new BirdCount() { Name = e.birdName, Count = totalCount, InList = true});

            filteredBirdList = Search.FilterBirdCountList(birdNameFilter.Text, workingBirdList);  // Update the filtered bird list

            userBirdListView.Adapter = new row_adapter(this, filteredBirdList);
        }

        private void PopDialog_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            string birdName = filteredBirdList[e.id].Name;
            int birdIndex = 0;

            foreach (var bird in workingBirdList)
            {
                if (bird.Name == birdName)
                {
                    birdIndex = workingBirdList.IndexOf(bird);  // Get the index of the bird in the "workingBirdList," and remove the bird at that index.
                }
                else
                {
                    // The matching bird in the "workingBirdList" has not been found (yet).
                }
            }

            //workingBirdList.RemoveAt(e.id);       // Cannot use "e.id" on "workingBirdList," because the user may be doing a filtered search
            workingBirdList.RemoveAt(birdIndex);

            int count = 0;
            if (e.birdCount == "")
            {
                count = 0;
            }
            else
            {
                count = Int32.Parse(e.birdCount);
            }
            
            workingBirdList.Insert(birdIndex, new BirdCount() { Name = e.birdName, Count = count, InList = true});

            filteredBirdList = Search.FilterBirdCountList(birdNameFilter.Text, workingBirdList);  // Update the filtered bird list

            userBirdListView.Adapter = new row_adapter(this, filteredBirdList);
        }

        private void PopDialog_OnDelete(object sender, OnDeleteEventArgs e)
        {
            string birdName = filteredBirdList[e.id].Name;
            int birdIndex = 0;

            foreach (var bird in workingBirdList)
            {
                if (bird.Name == birdName)
                {
                    birdIndex = workingBirdList.IndexOf(bird);  // Get the index of the bird in the "workingBirdList," and remove the bird at that index.
                }
                else
                {
                    // The matching bird in the "workingBirdList" has not been found (yet).
                }
            }

            //workingBirdList.RemoveAt(e.id);       // Cannot use "e.id" on "workingBirdList," because the user may be doing a filtered search
            workingBirdList.RemoveAt(birdIndex);

            userBirdListView = FindViewById<ListView>(Resource.Id.myListView);

            filteredBirdList = Search.FilterBirdCountList(birdNameFilter.Text, workingBirdList);  // Update the filtered bird list

            userBirdListView.Adapter = new row_adapter(this, filteredBirdList);

            // Disable the "Clear" button if the list now has no items in it.
            if (workingBirdList.Count == 0)
            {
                //btnClear.Enabled = false;
                llClear.Enabled = false;
                llClear.SetBackgroundColor(Android.Graphics.Color.LightGray);
            }
            else
            {
                //btnClear.Enabled = true;
                llClear.Enabled = true;
                llClear.SetBackgroundResource(Resource.Drawable.selector);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            // Pull up the dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();

            ClearAllPopUp ClearAll = new ClearAllPopUp();
            ClearAll.Show(transaction, "Dialog Fragment");

            // Subscribe to events in PopUpAdd class
            ClearAll.OnClear += ClearAll_OnClear;
        }

        private void ClearAll_OnClear(object sender, OnClearAllEventArgs e)
        {
            workingBirdList.Clear();

            // Also need to clear the variables for totalSpeciesSeen and totalBirdsSeen, so that all birds go back to having a 0 count.
            totalSpeciesSeen = 0;
            totalBirdsSeen = 0;

            userBirdListView = FindViewById<ListView>(Resource.Id.myListView);

            birdNameFilter.Text = "";       // Reset the bird name filter
            //btnClear.Enabled = false;       // Disable the "Clear" button because the list no longer has any items in it.
            llClear.Enabled = false;       // Disable the "Clear" button because the list no longer has any items in it.
            llClear.SetBackgroundColor(Android.Graphics.Color.LightGray);

            filteredBirdList = Search.FilterBirdCountList(birdNameFilter.Text, workingBirdList);  // Update the filtered bird list

            userBirdListView.Adapter = new row_adapter(this, filteredBirdList);
        }

        private void IBClearFilter_OnClick(object sender, EventArgs e)
        {
            birdNameFilter.Text = "";
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string birdName = filteredBirdList[e.Position].Name;

            foreach (var bird in workingBirdList)
            {
                if (bird.Name == birdName)
                {
                    int birdIndex = workingBirdList.IndexOf(bird);  // Get the index of the bird in the "workingBirdList," and increment the count for that bird.
                    workingBirdList[birdIndex].Count++;
                }
                else
                {
                    // The matching bird in the "workingBirdList" has not been found (yet).
                }
            }

            userBirdListView = FindViewById<ListView>(Resource.Id.myListView);

            filteredBirdList = Search.FilterBirdCountList(birdNameFilter.Text, workingBirdList);  // Update the filtered bird list

            userBirdListView.Adapter = new row_adapter(this, filteredBirdList);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            // Save Existing List of Birds to .csv File
            BirdListFile.CreateWorkingBirdListFile(masterBirdList, workingBirdList);

            // Calculate the total number of different bird species seen
            totalSpeciesSeen = workingBirdList.Count;

            // Calculate the total number of birds seen
            // 1) Reset the 'totalBirdsSeen' variable to begin a fresh total count
            totalBirdsSeen = 0;
            // 2) Add up the counts for all birds currently in the 'workingBirdList'
            foreach (var bird in workingBirdList)
            {
                totalBirdsSeen += bird.Count;
            }

            // Start New Intent to Open New Screen for Submit Form
            var intent = new Intent(this, typeof(EmailFormActivity));
            StartActivity(intent);

            //SetContentView(Resource.Layout.EmailForm);
        }
    }
}

