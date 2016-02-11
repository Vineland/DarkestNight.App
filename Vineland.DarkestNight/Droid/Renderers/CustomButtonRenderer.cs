using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Vineland.Necromancer.UI.Droid;
using Android.Widget;
using Android.Graphics;

[assembly: ExportRenderer (typeof(Xamarin.Forms.Button), typeof(CustomButtonRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class CustomButtonRenderer: ButtonRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Button> e)
		{
			base.OnElementChanged (e);
			try {
				var button = (Android.Widget.Button)Control;
				Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "ImperiumSerif.ttf");
				button.Typeface = font;
			} catch (Exception ex) {

			}
		}
	}
}
