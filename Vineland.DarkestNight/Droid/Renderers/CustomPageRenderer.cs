using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Content.Res;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;

[assembly: ExportRenderer (typeof(ContentPage), typeof(CustomPageRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class CustomPageRenderer : PageRenderer
	{
		public CustomPageRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
		{
			if (e.OldElement == null) {
				TypedArray styledAttributes = this.Context.Theme.ObtainStyledAttributes(
					new int[] { Android.Resource.Attribute.ActionBarSize });
				var actionbar = (int) styledAttributes.GetDimension(0, 0);
				styledAttributes.Recycle ();
				this.Element.Padding = new Thickness(0, actionbar / Resources.System.DisplayMetrics.Density, 0, 0);
			} else if (e.NewElement == null) {
			} else {
			}

			base.OnElementChanged (e);
		}
	}
}

