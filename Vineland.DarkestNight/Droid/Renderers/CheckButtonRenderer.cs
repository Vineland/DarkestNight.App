using System;
using Xamarin.Forms;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.App;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;


[assembly: ExportRenderer (typeof(CheckButton), typeof(CheckButtonRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class CheckButtonRenderer : ButtonRenderer
	{
		public CheckButtonRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null) {
				var checkBoxElement = Element as CheckButton;

				var control = new Android.Widget.CheckBox (this.Context);

				control.Checked = checkBoxElement.IsSelected;
				control.CheckedChange += Control_CheckedChange;
				control.SetButtonDrawable (Context.GetDrawable (Resource.Drawable.checkbox_button));

				this.SetNativeControl (control);
			} else if (e.NewElement == null) 
			{
				(Control as Android.Widget.CheckBox).CheckedChange -= Control_CheckedChange;
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsSelected")
				(Control as Android.Widget.CheckBox).Checked = (Element as CheckButton).IsSelected;
			if (e.PropertyName == "IsEnabled")
				(Control as Android.Widget.CheckBox).Enabled = (Element as CheckButton).IsEnabled;
		}

		void Control_CheckedChange (object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
		{ 
			(Element as CheckButton).IsSelected = e.IsChecked;
		}
	}
}