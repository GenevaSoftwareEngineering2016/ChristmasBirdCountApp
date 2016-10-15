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
    class row_adapter : BaseAdapter<BirdCount>
    {
        private readonly IList<BirdCount> _items;
        private readonly Context _context;

        public row_adapter(Context context, IList<BirdCount> items)
        {
            _items = items;
            _context = context;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];
            var view = convertView;

            if(view == null)
            {
                var inflater = LayoutInflater.FromContext(_context);
                view = inflater.Inflate(Resource.Layout.row, parent, false);
            }

            view.FindViewById<TextView>(Resource.Id.left).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.right).Text = item.count;

            return view;

        }

        public override int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public override BirdCount this[int position]
        {
            get { return _items[position];  }
        }
    }
}