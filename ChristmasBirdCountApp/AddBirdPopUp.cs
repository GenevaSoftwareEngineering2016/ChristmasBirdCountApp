// (c) 2016 Geneva College Senior Software Project Team

using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ChristmasBirdCountApp
{
    public class AddBirdPopUp : DialogFragment
    {
        private ListView addBirdListView;
        private List<BirdCount> masterBirdList;

        public void AddBirdList(List<BirdCount> birdList)
        {
            masterBirdList = birdList;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.AddBirdPopUp, container, false);

            addBirdListView = view.FindViewById<ListView>(Resource.Id.addBirdListView);

            addBirdListView.Adapter = new row_adapter(this.Context, masterBirdList);

            return view;
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