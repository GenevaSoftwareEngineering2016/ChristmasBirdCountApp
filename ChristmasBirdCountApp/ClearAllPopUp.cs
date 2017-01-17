// (c) 2016 Geneva College Senior Software Project Team
using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ChristmasBirdCountApp
{

    public class OnClearAllEventArgs : EventArgs
    {
    }

    class ClearAllPopUp : DialogFragment
    {
        private Button btnCLearList;
        private Button btnCancel;

        //broadcast events
        public event EventHandler<OnClearAllEventArgs> OnClear;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            //removing title bar of fragment for cleaner look
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            base.OnActivityCreated(savedInstanceState);
            //Dialog.Window.Attributes.WindowAnimations = Resource
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.ClearAllPopUp, container, false);

            //clear the list
            btnCLearList = view.FindViewById<Button>(Resource.Id.btnClearList);
            btnCLearList.Click += BtnCLearList_Click;

            //cancel the clear action
            btnCancel = view.FindViewById<Button>(Resource.Id.btnCancel);
            btnCancel.Click += BtnCancel_Click;

            return view;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        private void BtnCLearList_Click(object sender, EventArgs e)
        {
            OnClear.Invoke(this, new OnClearAllEventArgs());
            this.Dismiss();
        }
    }
}