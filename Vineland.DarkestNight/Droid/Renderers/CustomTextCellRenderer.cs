using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Vineland.Necromancer.UI.Droid;
using Android.Views;

[assembly: ExportRenderer (typeof(TextCell), typeof(CustomTextCellRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class CustomTextCellRenderer: TextCellRenderer
	{
		public CustomTextCellRenderer ()
		{
		}

		protected override Android.Views.View GetCellCore (Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
		{
			var textCell = (TextCell)item;
			var view = base.GetCellCore(item, convertView, parent, context) as ViewGroup;
//			if (String.IsNullOrEmpty((item as TextCell).Text))
//			{
//				view.Visibility = ViewStates.Gone;
//				while (view.ChildCount > 0)
//				{
//					view.RemoveViewAt(0);
//				}
//				view.SetMinimumHeight(0);
//				view.SetPadding(0, 0, 0, 0);
//			}
			if (string.IsNullOrEmpty (textCell.Text))
				view.Visibility = Android.Views.ViewStates.Gone;
			
			return view;
		}

	}
}

