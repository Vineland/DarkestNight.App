using Android.App;
using Android.Graphics.Drawables;
using Vineland.Necromancer.UI.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationRenderer))]

namespace Vineland.Necromancer.UI.Droid
{
	public class CustomNavigationRenderer : NavigationRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
		{
			base.OnElementChanged (e);

			var actionBar = ((Activity)Context).ActionBar;
			actionBar.SetIcon (new ColorDrawable(Color.Transparent.ToAndroid()));
			actionBar.SetBackgroundDrawable (new ColorDrawable (Color.FromHex("#8B4500").ToAndroid()));
		}
	}
}
