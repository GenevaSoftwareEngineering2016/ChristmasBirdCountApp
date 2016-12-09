// (c) 2016 Geneva College Senior Software Project Team
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Linq;

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
        //private EditText addBirdNameFilter;

        //broadcast events
        public event EventHandler<OnTapEventArgs> OnTap;

        public void AddBirdLists(List<BirdCount> masterBirdList, List<BirdCount> workingBirdList)
        {
            mstrBirdList = masterBirdList;
            wrkBirdList = workingBirdList;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.AddBirdPopUp, container, false);

            //addBirdNameFilter = view.FindViewById<EditText>(Resource.Id.txtAddBirdNameFilter);

            addBirdListView = view.FindViewById<ListView>(Resource.Id.addBirdListView);

            addBirdListView.Adapter = new row_adapter(this.Activity, mstrBirdList);

            addBirdListView.ItemClick += AddBirdListView_ItemClick;

            return view;
        }

        private void AddBirdListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string birdName = mstrBirdList[e.Position].Name;
            if (wrkBirdList.Exists(x => x.Name == birdName))
            {
                string alert = "This bird already exists on your current list";
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