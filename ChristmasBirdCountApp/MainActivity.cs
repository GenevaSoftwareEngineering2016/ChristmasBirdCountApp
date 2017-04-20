// (c) 2016 Geneva College Senior Software Project Team
using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using ChristmasBirdCountApp.Email;

namespace ChristmasBirdCountApp
{
    [Activity(Label = "Birdubon", MainLauncher = true, Icon = "@drawable/tealBird", Theme = "@style/CustomActionBarTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        private List<BirdCount> masterBirdList;     // Most up-to-date list of all birds; Used by search function to add birds to "workingBirdList"
        private List<BirdCount> workingBirdList;    // List of all birds with counts 0+; This list is submitted with email report to Compiler
        private List<BirdCount> filteredBirdList;
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
            llClear.Click += BtnClear_Click;
            llSubmit.Click += BtnSubmit_Click;
            // End Button Click Events

            birdNameFilter.ClearFocus();    // Do not focus on text field for filter by default.
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

            // Initialize ListView Adapter
            userBirdListView.Adapter = new row_adapter(this, filteredBirdList);

            // Register Event Handlers
            ibClearFilter.Click += IBClearFilter_OnClick;
            llAdd.Click += AddBird_OnClick;

            if (workingBirdList.Count == 0)
            {
                llClear.Enabled = false;
                llClear.SetBackgroundColor(Android.Graphics.Color.LightGray);
            }
            else
            {
                llClear.Enabled = true;
                llClear.SetBackgroundResource(Resource.Drawable.selector);

            }
        }

        protected override void OnStop()
        {
            // Save Existing List of Birds to .csv File
            BirdListFile.CreateWorkingBirdListFile(masterBirdList, workingBirdList);

            // Deregister Event Handlers
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
            // Create new bird object and add it to the working list
            BirdCount newBird = new BirdCount(e.birdName, 0, true);
            workingBirdList.Add(newBird);

            // Clear the search filter and return the user to the (filtered, but no filter applied) working bird list
            birdNameFilter.Text = "";
            filteredBirdList = Search.FilterBirdCountList(birdNameFilter.Text, workingBirdList);    // Update the filtered bird list; IF filter was not cleared, list would still be filtered after bird is added.

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
            int countBeforeAdd = filteredBirdList[e.id].Count;
            int birdIndex = 0;

            // If the bird's count is already at the maximum value, we cannot increment or add to the count; Display an error message
            if (countBeforeAdd == Int32.MaxValue)
            {
                string alert = "Maximum count already reached.  Cannot add to count.";
                Toast.MakeText(this, alert, ToastLength.Short).Show();
                // END execution of this function, because we cannot add to the bird's count anyways
            }
            else
            {
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
                if (string.IsNullOrEmpty(e.addNumber))
                {
                    addBirds = 0;
                }
                else
                {
                    // Check that number entered is a valid 32-bit Int.  IF value parses to Int32, addBirds is set to the value.  IF NOT, the user gets an error message.
                    if (!Int32.TryParse(e.addNumber, out addBirds))
                    {
                        string alert = "Invalid number.  Check value entered.  Value may be too large.";
                        Toast.MakeText(this, alert, ToastLength.Short).Show();
                    }

                    // We need to see if the current count + the number to add exceeds the value that can be stored in an Int32.
                    Int64 valueToCheck = Convert.ToInt64(countBeforeAdd) + Convert.ToInt64(addBirds);

                    if (valueToCheck > Int32.MaxValue)  // If the number is larger than what a bird's 'Count' can hold, we give an error and exit.
                    {
                        string alert = "Value would exceed maximum count.  Cannot add to count.";
                        Toast.MakeText(this, alert, ToastLength.Short).Show();
                        addBirds = 0;   // Change number to add to zero (0), because we cannot add to the bird's count anyways
                    }
                }

                // Add count to existing bird count
                var totalCount = e.birdCount + addBirds;

                workingBirdList.Insert(birdIndex, new BirdCount() { Name = e.birdName, Count = totalCount, InList = true });

                filteredBirdList = Search.FilterBirdCountList(birdNameFilter.Text, workingBirdList);  // Update the filtered bird list

                userBirdListView.Adapter = new row_adapter(this, filteredBirdList);
            }
        }

        private void PopDialog_OnUpdate(object sender, OnUpdateEventArgs e)
        {
            string birdName = filteredBirdList[e.id].Name;
            int birdCountBeforeUpdate = filteredBirdList[e.id].Count;
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

            int count;
            if (string.IsNullOrEmpty(e.birdCount))
            {
                count = 0;
            }
            else
            {
                // Check that 'count' number entered is a valid 32-bit Int.  IF value parses to Int32, 'count' is set to the value.  IF NOT, the user gets an error message.
                if (!Int32.TryParse(e.birdCount, out count))
                {
                    count = birdCountBeforeUpdate;       // Use the last valid count, since we cannot use the updated count provided by the user.
                    string alert = "Invalid number.  Check value entered.  Value may be too large.";
                    Toast.MakeText(this, alert, ToastLength.Short).Show();
                }
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

                    // If the bird's count is already at the maximum value, we cannot increment or add to the count; Display an error message
                    if (workingBirdList[birdIndex].Count == Int32.MaxValue)
                    {
                        string alert = "Maximum count already reached.  Cannot increment count.";
                        Toast.MakeText(this, alert, ToastLength.Short).Show();
                    }
                    else
                    {
                        workingBirdList[birdIndex].Count++;
                    }
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

            // Calculate the total number of different bird species seen, but only birds in the list with Count >= 1
            totalSpeciesSeen = workingBirdList.Count(bird => bird.Count >= 1);

            // Calculate the total number of birds seen
            // 1) Reset the 'totalBirdsSeen' variable to begin a fresh total count
            totalBirdsSeen = 0;
            // 2) Add up the counts for all birds currently in the 'workingBirdList'
            foreach (var bird in workingBirdList)
            {
                totalBirdsSeen += bird.Count;
            }

            // Start New Intent to Open New Screen for Submit Form
            // Check with user to see what type of count report needs to be made.
            // Options are Field, Feeder, and Count Week
            // Open the correct report form for the count type selected by the user.

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            ChooseCountTypePopUp countTypePopUp = new ChooseCountTypePopUp();
            countTypePopUp.Show(transaction, "Dialog Fragment");

            // Subscribe to events in ChooseCountTypePopUp class
            countTypePopUp.OnFieldCount += Submit_OnFieldCount;
            countTypePopUp.OnFeederCount += Submit_OnFeederCount;
            countTypePopUp.OnCountWeekCount += Submit_OnCountWeekCount;
        }

        private void Submit_OnFieldCount(object sender, OnFieldCountEventArgs e)
        {
            // Open the Field count type submission form
            var intent = new Intent(this, typeof(FieldFormActivity));
            StartActivity(intent);
        }

        private void Submit_OnFeederCount(object sender, OnFeederCountEventArgs e)
        {
            // Open the Feeder count type submission form
            var intent = new Intent(this, typeof(FeederFormActivity));
            StartActivity(intent);
        }

        private void Submit_OnCountWeekCount(object sender, OnCountWeekCountEventArgs e)
        {
            // Open the Count Week count type submission form
            var intent = new Intent(this, typeof(CountWeekFormActivity));
            StartActivity(intent);
        }
    }
}

