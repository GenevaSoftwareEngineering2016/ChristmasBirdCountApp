using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace ChristmasBirdCountApp
{
    //custome event arguments for deleting a bird
    public class OnDeleteEventArgs : EventArgs
    {
        //private int mid;

        public int id { get; set; }

        public OnDeleteEventArgs(int birdid) : base()
        {
            id = birdid;
        }
    }
    //custom event arguments for updating a bird
    public class OnUpdateEventArgs : EventArgs
    {
        //private int mid;
        //private string mBirdName;
        //private string mBirdCount;

        public int id { get; set; }
        public string birdName { get; set; }
        public string birdCount { get; set; }

        public OnUpdateEventArgs(int birdid, string birdname, string birdcount) : base()
        {
            id = birdid;
            birdName = birdname;
            birdCount = birdcount;

        }
    }
    class PopUp : DialogFragment
    {
        private EditText txtBirdName;
        private EditText txtBirdCount;
        private Button btnClearListing;
        private Button btnUpdateListing;
        private string birdName;
        private int count;
        private int birdId;

        //broadcast events
        public event EventHandler<OnDeleteEventArgs> OnDelete;
        public event EventHandler<OnUpdateEventArgs> OnUpdate;

        public PopUp(int id, string name, int number)
        {
            birdName = name;
            count = number;
            birdId = id;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Pop, container, false);

            //Set Edit texts
            txtBirdName = view.FindViewById<EditText>(Resource.Id.txtBirdName);
            txtBirdName.Text = birdName;

            txtBirdCount = view.FindViewById<EditText>(Resource.Id.txtBirdCount);
            txtBirdCount.Text = count.ToString();

            //deleting a bird
            btnClearListing = view.FindViewById<Button>(Resource.Id.btnClearListing);
            btnClearListing.Click += btnClearListing_Click;

            //Updating a bird
            btnUpdateListing = view.FindViewById<Button>(Resource.Id.btnUpdateListing);
            btnUpdateListing.Click += BtnUpdateListing_Click;
            return view;
        }

        private void BtnUpdateListing_Click(object sender, EventArgs e)
        {
            //calling the update event
            OnUpdate.Invoke(this, new OnUpdateEventArgs(birdId, txtBirdName.Text, txtBirdCount.Text));
            this.Dismiss();
        }

        void btnClearListing_Click(object sender, EventArgs e)
        {
            //calling the delete event
            OnDelete.Invoke(this, new OnDeleteEventArgs(birdId));
            this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            //removing title bar of fragment for cleaner look
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            base.OnActivityCreated(savedInstanceState);
            //Dialog.Window.Attributes.WindowAnimations = Resource
        }
    }
}