using System;
using System.Collections.Generic;
using System.Text;
using Android.Views;
using Android.Widget;
using Android.App;
using Android.Graphics;
using Vineland.DarkestNight.UI.Shared.Utilities;

namespace Vineland.DarkestNight.UI.Android
{
    public class EnumListAdapter<T> : BaseAdapter<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        protected List<T> _enumValues;
        protected Activity _context;
        protected int? _listItemLayout;

        public int? SelectedIndex { get; set; }

        public EnumListAdapter(Activity context, List<T> enumValues, int listItemLayout = Resource.Layout.ListItem)
        {
            _enumValues = enumValues;
            _context = context;
            _listItemLayout = listItemLayout;
        }

        public override T this[int position]
        {
            get { return _enumValues[position]; }
        }

        public override int Count
        {
            get { return _enumValues.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public int GetPosition(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].ToString() == item.ToString())
                    return i;
            }
            return -1;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = _context.LayoutInflater.Inflate(_listItemLayout ?? Resource.Layout.ListItem, null);

            // set fields
            var enumValue = _enumValues[position];

            string labelText = EnumUtil.GetDescription(enumValue);
            view.FindViewById<TextView>(Resource.Id.ListItemTitle).Text = labelText;

            if (SelectedIndex.HasValue && SelectedIndex.Value == position)
                view.FindViewById<LinearLayout>(Resource.Id.ListItemLayoutRoot).SetBackgroundColor(_context.Resources.GetColor(Resource.Color.minda_lightgrey));
            else
                view.FindViewById<LinearLayout>(Resource.Id.ListItemLayoutRoot).SetBackgroundColor(Color.White);

            return view;
        }

        public void UpdateSelectedItem(int index, ListView listView)
        {
            var previousIndex = SelectedIndex;

            //visually deselect previous index
            if (previousIndex.HasValue)
            {
                var previousSelection = listView.GetChildAt(previousIndex.Value);
                previousSelection.FindViewById<LinearLayout>(Resource.Id.ListItemLayoutRoot).SetBackgroundColor(Color.White);
            }

            //visually select the new index
            var newSelection = listView.GetChildAt(index);
            newSelection.FindViewById<LinearLayout>(Resource.Id.ListItemLayoutRoot).SetBackgroundColor(_context.Resources.GetColor(Resource.Color.minda_lightgrey));

            SelectedIndex = index;
        }
    }
}
