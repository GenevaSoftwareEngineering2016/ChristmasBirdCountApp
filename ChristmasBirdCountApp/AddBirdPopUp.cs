// (c) 2016 Geneva College Senior Software Project Team
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Android.Text;

namespace ChristmasBirdCountApp
{
    public class OnTapEventArgs : EventArgs
    {
        public string birdName { get; set; }

        public OnTapEventArgs(string birdname) : base()
        {
            birdName = birdname;
        }
    }

    public class AddBirdPopUp : DialogFragment
    {
        private ListView addBirdListView;
        private List<BirdCount> mstrBirdList;
        private List<BirdCount> wrkBirdList;
        private List<BirdCount> filteredMstrBirdList;
        private EditText addBirdNameFilter;
        private ImageButton addBirdClearFilter;

        //broadcast events
        public event EventHandler<OnTapEventArgs> OnTap;

        public void AddBirdLists(List<BirdCount> masterBirdList, List<BirdCount> workingBirdList)
        {
            mstrBirdList = masterBirdList;
            wrkBirdList = workingBirdList;
            filteredMstrBirdList = mstrBirdList;    // Use so that the actual 'Master' bird list never needs to be changed (e.g. by filtering).
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.AddBirdPopUp, container, false);

            addBirdNameFilter = view.FindViewById<EditText>(Resource.Id.txtAddBirdNameFilter);

            addBirdListView = view.FindViewById<ListView>(Resource.Id.addBirdListView);

            addBirdListView.Adapter = new row_adapter(this.Activity, filteredMstrBirdList);

            addBirdClearFilter = view.FindViewById<ImageButton>(Resource.Id.ibAddBirdClearFilter);

            addBirdListView.ItemClick += AddBirdListView_ItemClick;
            addBirdNameFilter.TextChanged += AddBirdNameFilter_OnTextChanged;
            addBirdClearFilter.Click += IBAddBirdClearFilter_OnClick;

            return view;
        }

        private void AddBirdNameFilter_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            filteredMstrBirdList = Search.FilterBirdCountList(addBirdNameFilter.Text, mstrBirdList);    // Always compare filter requests to the 'Master' list, but use the 'filtered Master' for displaying view to user.
            addBirdListView.Adapter = new row_adapter(this.Activity, filteredMstrBirdList);
        }

        private void IBAddBirdClearFilter_OnClick(object sender, EventArgs e)
        {
            addBirdNameFilter.Text = "";
        }

        private void AddBirdListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string birdName = filteredMstrBirdList[e.Position].Name;    // Must use 'filtered Master' or picking the third item in the filtered list will actually add the third bird name in 'Master' list.
            if (wrkBirdList.Exists(x => x.Name == birdName))
            {
                string alert = "This bird already exists in your current list.";
                Toast.MakeText(this.Activity, alert, ToastLength.Short).Show();
            }
            else
            {
                OnTap.Invoke(this, new OnTapEventArgs(birdName));
                this.Dismiss();
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            // Removing title bar of fragment for cleaner look
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            base.OnActivityCreated(savedInstanceState);
            // Dialog.Window.Attributes.WindowAnimations = Resource
        }
    }
}