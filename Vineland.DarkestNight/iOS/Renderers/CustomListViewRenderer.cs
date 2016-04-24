using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Vineland.Necromancer.iOS;

[assembly: ExportRenderer(typeof(ListView), typeof(CustomListViewRenderer))]
namespace Vineland.Necromancer.iOS
{
	public class CustomListViewRenderer: ListViewRenderer
	{
		public CustomListViewRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null) {
				Control.SeparatorInset = UIKit.UIEdgeInsets.Zero;

				Control.TableFooterView = new UIKit.UIView (CoreGraphics.CGRect.Empty);
			}
		}
	}
}

