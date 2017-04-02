using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		//protected override void OnAppearing ()
		//{
		//	(BindingContext as BaseViewModel).OnAppearing ();
		//}
	}
}

