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
        private List<BirdCount> mstrBirdList;
        private List<BirdCount> wrkBirdList;
        //private EditText addBirdNameFilter;

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

            addBirdListView.Adapter = new row_adapter(this.Context, mstrBirdList);

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