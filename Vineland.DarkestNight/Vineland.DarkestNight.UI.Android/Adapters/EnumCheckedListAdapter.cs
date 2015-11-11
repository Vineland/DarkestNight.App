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
using Vineland.DarkestNight.UI.Shared.Utilities;

namespace Vineland.DarkestNight.UI.Android.Adapters
{
    public class EnumCheckedListAdapter<T> : EnumListAdapter<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        public EnumCheckedListAdapter(Activity context, List<T> enumValues)
            :base(context, enumValues)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = _context.LayoutInflater.Inflate(Android.Resource.Layout.CheckedListItem, null);

            //set fields
            var enumValue = _enumValues[position];
            view.FindViewById<CheckedTextView>(Resource.Id.ListItemTitle).Text = EnumUtil.GetDescription(enumValue);

            return view;
        }
    }
}