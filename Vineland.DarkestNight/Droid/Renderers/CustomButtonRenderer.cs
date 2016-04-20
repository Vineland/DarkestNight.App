using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Vineland.Necromancer.UI.Droid;
using Android.Widget;
using Android.Graphics;
using System.Runtime.InteropServices;

[assembly: ExportRenderer (typeof(Xamarin.Forms.Button), typeof(CustomButtonRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class CustomButtonRenderer: ButtonRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Button> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null) {
				var button = (Android.Widget.Button)Control;
				button.Typeface = FontManager.GetFont (e.NewElement.FontFamily);

			}
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);
			if (changed) {
				var drawables = Control.GetCompoundDrawables ();
				if (drawables [0] != null) {
					Rect drawableBounds = new Rect ();
					drawables [0].CopyBounds (drawableBounds);
					int leftOffset = ((Control.Width - Control.PaddingLeft - Control.PaddingRight) - drawableBounds.Width ()) / 2;
					drawableBounds.Offset (leftOffset, 0);
					drawables [0].SetBounds (drawableBounds.Left, drawableBounds.Top, drawableBounds.Right, drawableBounds.Bottom);
				}
			}
		}
	}
}
