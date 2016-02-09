using System;
using Xamarin.Forms;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Widget;

[assembly: ExportRenderer (typeof(CheckBoxCell), typeof(CheckBoxCellRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class CheckBoxCellRenderer: ViewCellRenderer
	{
		public CheckBoxCellRenderer ()
		{
		}

		protected override Android.Views.View GetCellCore (Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
		{
			var x = (CheckBoxCell)item;

			var view = convertView;

			if (view == null) {
				// no view to re-use, create new
				view = (context as Activity).LayoutInflater.Inflate (Resource.Layout.CheckBoxCell, null);
			}

			var checkBox = view.FindViewById<Android.Widget.CheckBox> (Resource.Id.CheckBox);
			checkBox.Text = x.Text;
			checkBox.Checked = x.IsSelected;

			checkBox.CheckedChange += (sender, e) => {x.IsSelected = e.IsChecked; };
		
			return view;
		}
	}
}

