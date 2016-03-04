using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Vineland.Necromancer.UI.Droid;
using Android.Widget;
using Android.Graphics;
using Android.Util;

[assembly: ExportRenderer (typeof(ImageCell), typeof(CustomImageCellRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class CustomImageCellRenderer :ImageCellRenderer
	{
		public CustomImageCellRenderer ()
		{
		}

		protected override Android.Views.View GetCellCore (Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
		{
			var view = base.GetCellCore (item, convertView, parent, context) as BaseCellView;

			var image = view.GetChildAt (0);
			image.SetPadding (20, 0, 0, 0);
			var label = (view.GetChildAt (1) as LinearLayout).GetChildAt(0) as TextView; //yikes!
			label.Typeface = Typeface.CreateFromAsset (Forms.Context.Assets,"baskerville_becker.ttf");
			label.SetTextColor (Android.Graphics.Color.Black);
			label.SetTextSize (ComplexUnitType.Dip, 20);

			return view;
		}
	}
}

