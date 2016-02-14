using System;
using Xamarin.Forms;
using Vineland.Necromancer.UI.Droid;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;

[assembly: ExportRenderer (typeof(Picker), typeof(CustomPickerRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class CustomPickerRenderer : ViewRenderer<Picker, Spinner>
	{
		protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged (e);
			if (e.OldElement == null) { 
				
				var spinner = new Spinner (this.Context);
				var adapter = new SpinnerAdapter (this.Context, Android.Resource.Layout.SimpleSpinnerItem, Element.Items);

				adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);

				spinner.Adapter = adapter;
				spinner.SetSelection (Element.SelectedIndex);
				spinner.ItemSelected += NativePickerView_ItemSelected;
 
				//Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "hobo.ttf");
				//nativePickerView.Typeface = font;

				this.SetNativeControl (spinner);
			} else if (e.NewElement == null) {

				Control.ItemSelected -= NativePickerView_ItemSelected;
			}
		}

		void NativePickerView_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Element.SelectedIndex = e.Position;
		}
	}

	public class SpinnerAdapter: ArrayAdapter<string>
	{
		Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "hobo.ttf");
		public SpinnerAdapter (Context context, int resource, IList<string> items)
			:base(context, resource, items)
		{
			
		}
		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			var view = (TextView)base.GetView (position, convertView, parent);
			view.Typeface = font;

			return view;
		}

		public override Android.Views.View GetDropDownView (int position, Android.Views.View convertView, ViewGroup parent)
		{
			var view = (TextView)base.GetDropDownView (position, convertView, parent);
			view.Typeface = font;
			view.SetBackgroundColor (Xamarin.Forms.Color.FromHex ("#D2C096").ToAndroid());
			return view;
		}
	}
}

