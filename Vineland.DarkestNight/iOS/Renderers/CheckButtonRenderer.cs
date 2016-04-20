using System;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer (typeof(CheckButton), typeof(CheckButtonRenderer))]
namespace Vineland.Necromancer.iOS
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

				var control = new UIButton(UIButtonType.Custom);
				control.Selected = checkBoxElement.IsSelected;
				control.SetImage (new UIImage ("check_selected.png"), UIControlState.Selected);
				control.SetImage (new UIImage ("check_unselected.png"), UIControlState.Normal);
				control.TouchUpInside += Control_TouchUpInside;


				this.SetNativeControl (control);

			} else if (e.NewElement == null) 
			{
				(Control as UIButton).TouchUpInside -= Control_TouchUpInside;
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsSelected")
				(Control as UIButton).Selected = (Element as CheckButton).IsSelected;
			if (e.PropertyName == "IsEnabled")
				(Control as UIButton).Enabled = (Element as CheckButton).IsEnabled;
		}

		void Control_TouchUpInside (object sender, EventArgs e)
		{
			var button = sender as UIButton;
			button.Selected = !button.Selected;
			(Element as CheckButton).IsSelected = button.Selected;
		}
	}
}

