using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Vineland.Necromancer.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(ContentPage), typeof(CustomPageRenderer))]
namespace Vineland.Necromancer.iOS
{
	public class CustomPageRenderer : PageRenderer
	{
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(false);

			UIGraphics.BeginImageContext (this.View.Frame.Size);
			var contentPage = Element as ContentPage;
			if (contentPage.BackgroundImage != null) {
				UIImage i = UIImage.FromFile (contentPage.BackgroundImage);
				i = i.Scale (this.View.Frame.Size);
				//this.NavigationController.NavigationBar.Translucent = true;
				this.View.BackgroundColor = UIColor.FromPatternImage (i);
			}

		}
	}
}

