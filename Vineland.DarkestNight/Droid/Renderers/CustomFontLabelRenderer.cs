using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Vineland.Necromancer.UI.Droid;
using Android.Widget;
using Android.Graphics;

[assembly: ExportRenderer (typeof(Label), typeof(CustomFontLabelRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class CustomFontLabelRenderer: LabelRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);
			if (e.OldElement == null) {
				try {
					var label = (TextView)Control;					
					Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "baskerville_becker.ttf");
					label.Typeface = font;

				} catch (Exception ex) {
				
				}
			}
		}
	}
}

