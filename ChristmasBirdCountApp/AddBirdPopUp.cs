// (c) 2016 Geneva College Senior Software Project Team
using System;
using Android.App;
using Android.OS;
using Android.Views;

namespace ChristmasBirdCountApp
{
    class AddBirdPopUp : DialogFragment
    {
        public void AddButton_OnClick(object sender, EventArgs e)
        {
            //string txtNameFilter = FindViewById<EditText>(Resource.Id.txtNameFilter).Text;
            //if (txtNameFilter != "")
            //{
            //    workingBirdList.Insert(0, new BirdCount() { Name = txtNameFilter, Count = 0 });
            //    mListView.Adapter = new row_adapter(this, workingBirdList);
            //    FindViewById<EditText>(Resource.Id.txtNameFilter).Text = "";
            //}
            //else
            //{
            //    Toast.MakeText(this, "Please enter a bird name", ToastLength.Short).Show();
            //}
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Pop, container, false);

            //Set Edit texts
            //txtBirdName = view.FindViewById<EditText>(Resource.Id.txtBirdName);
            //txtBirdName.Text = birdName;

            //txtBirdCount = view.FindViewById<EditText>(Resource.Id.txtBirdCount);
            //txtBirdCount.Text = count.ToString();

            return view;
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