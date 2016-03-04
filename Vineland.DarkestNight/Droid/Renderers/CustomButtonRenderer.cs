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
	}
}
