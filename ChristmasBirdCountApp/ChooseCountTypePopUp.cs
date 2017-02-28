// (c) 2017 Geneva College Senior Software Project Team
using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ChristmasBirdCountApp
{
    public class OnFieldCountEventArgs : EventArgs { }
    public class OnFeederCountEventArgs : EventArgs { }
    public class OnCountWeekCountEventArgs : EventArgs { }

    public class ChooseCountTypePopUp : DialogFragment
    {
        private Button _btnFieldCount;
        private Button _btnFeederCount;
        private Button _btnCountWeekCount;
        private Button _btnCancelSubmit;

        // Broadcast Events
        public EventHandler<OnFieldCountEventArgs> OnFieldCount;
        public EventHandler<OnFeederCountEventArgs> OnFeederCount;
        public EventHandler<OnCountWeekCountEventArgs> OnCountWeekCount;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            //removing title bar of fragment for cleaner look
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            base.OnActivityCreated(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);

            View view = inflater.Inflate(Resource.Layout.ChooseCountTypePopUp, container, false);

            // Submit a Field Report
            _btnFieldCount = view.FindViewById<Button>(Resource.Id.btnFieldCount);
            _btnFieldCount.Click += BtnFieldCount_OnClick;

            // Submit a Feeder Report
            _btnFeederCount = view.FindViewById<Button>(Resource.Id.btnFeederCount);
            _btnFeederCount.Click += BtnFeederCount_OnClick;

            // Submit a Count Week Report
            _btnCountWeekCount = view.FindViewById<Button>(Resource.Id.btnCountWeekCount);
            _btnCountWeekCount.Click += BtnCountWeekCount_OnClick;

            // Cancel Report Submission
            _btnCancelSubmit = view.FindViewById<Button>(Resource.Id.btnCancelSubmit);
            _btnCancelSubmit.Click += BtnCancel_OnClick;

            return view;
        }

        private void BtnFieldCount_OnClick(object sender, EventArgs e)
        {
            OnFieldCount.Invoke(this, new OnFieldCountEventArgs());
            this.Dismiss();
        }

        private void BtnFeederCount_OnClick(object sender, EventArgs e)
        {
            OnFeederCount.Invoke(this, new OnFeederCountEventArgs());
            this.Dismiss();
        }

        private void BtnCountWeekCount_OnClick(object sender, EventArgs e)
        {
            OnCountWeekCount.Invoke(this, new OnCountWeekCountEventArgs());
            this.Dismiss();
        }

        private void BtnCancel_OnClick(object sender, EventArgs e)
        {
            this.Dismiss();
        }
    }
}