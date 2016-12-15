// (c) 2016 Geneva College Senior Software Project Team
using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ChristmasBirdCountApp
{
    //custom event arguments for adding to the total count of birds
    public class OnAddEventArgs : EventArgs
    {
        public int id { get; set; }
        public string birdName { get; set; }
        public int birdCount { get; set; }
        public string addNumber { get; set; }

        public OnAddEventArgs(int birdid, string birdname, int birdcount, string addCount) : base()
        {
            id = birdid;
            birdName = birdname;
            birdCount = birdcount;
            addNumber = addCount;
        }
    }

    public class OnEditEventArgs : EventArgs
    {
        public int id { get; set; }
        public string birdName { get; set; }
        public int birdCount { get; set; }

        public OnEditEventArgs(int birdid, string birdname, int birdcount) : base()
        {
            id = birdid;
            birdName = birdname;
            birdCount = birdcount;
        }
    }

    class PopUpAdd : DialogFragment
    {
        private EditText txtNumber;
        private Button btnAddBirds;
        private Button btnEditBird;
        private string birdName;
        private int count;
        private int birdId;

        //broadcast events
        public event EventHandler<OnAddEventArgs> OnAdd;
        public event EventHandler<OnEditEventArgs> OnEdit;

        public PopUpAdd(int id, string name, int number)
        {
            birdId = id;
            birdName = name;
            count = number;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.BulkAdd, container, false);

            // Set Edit texts
            txtNumber = view.FindViewById<EditText>(Resource.Id.txtNumber);

            //Adding birds
            btnAddBirds = view.FindViewById<Button>(Resource.Id.btnAddBirds);
            btnAddBirds.Click += BtnAddBirds_Click;

            // updating/deleting birds
            btnEditBird = view.FindViewById<Button>(Resource.Id.btnEditBird);
            btnEditBird.Click += BtnEditBird_Click;

            return view;
        }

        private void BtnEditBird_Click(object sender, EventArgs e)
        {
            OnEdit.Invoke(this, new OnEditEventArgs(birdId, birdName, count));
            this.Dismiss();
        }

        private void BtnAddBirds_Click(object sender, EventArgs e)
        {
            OnAdd.Invoke(this, new OnAddEventArgs(birdId, birdName, count, txtNumber.Text));
            this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }
    }
}