using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class DarknessPopupPageModel :BaseViewModel
	{
		public DarknessPopupPageModel()
		{
		}

		public int Darkness
		{
			get { return Application.CurrentGame.Darkness; }
			set
			{
				Application.CurrentGame.Darkness = value;
			}
		}

		protected override void ViewIsDisappearing(object sender, System.EventArgs e)
		{
			base.ViewIsDisappearing(sender, e);
			MessagingCenter.Send<DarknessPopupPageModel>(this, "DarknessUpdated");
		}
	}
}
