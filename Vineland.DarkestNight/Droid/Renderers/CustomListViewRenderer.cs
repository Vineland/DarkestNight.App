using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.Collections.Generic;
using Android.Content.Res;
using Vineland.Necromancer.UI.Droid;

[assembly: ExportRenderer (typeof(ListView), typeof(CustomListViewRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	
	public class CustomListViewRenderer : ListViewRenderer
	{
		public CustomListViewRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.ListView> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
			}

			if (e.NewElement != null) {
				Control.SetClipToPadding (false);
				TypedArray styledAttributes = this.Context.Theme.ObtainStyledAttributes(
					new int[] { Android.Resource.Attribute.ActionBarSize });
				var actionbar = (int) styledAttributes.GetDimension(0, 0);
				styledAttributes.Recycle ();
				Control.SetPadding (0, actionbar, 0, 0);
			}
		}
	}
}

