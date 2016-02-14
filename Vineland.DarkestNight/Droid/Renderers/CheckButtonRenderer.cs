using System;
using Xamarin.Forms;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.App;
using Android.Graphics.Drawables;


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

			if(e.NewElement != null)
			{
				var checkBoxElement = Element as CheckButton;

				var control = new Android.Widget.CheckBox(this.Context);

				control.Checked = checkBoxElement.IsSelected;
				control.CheckedChange += (sender, args) => { checkBoxElement.IsSelected = args.IsChecked;};
				control.SetBackground (Resources.GetDrawable (Resource.Drawable.checkbox_background));
				control.SetButtonDrawable(Resources.GetDrawable (Resource.Drawable.checkbox_button));

				this.SetNativeControl(control);
			}
		}
	}
}