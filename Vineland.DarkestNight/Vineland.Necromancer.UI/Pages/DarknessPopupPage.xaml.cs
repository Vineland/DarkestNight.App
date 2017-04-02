using Rg.Plugins.Popup.Pages;

namespace Vineland.Necromancer.UI
{
	public partial class DarknessPopupPage : PopupPage
	{
		public DarknessPopupPage()
		{
			InitializeComponent();
			CloseWhenBackgroundIsClicked = true;
			LayoutRoot.BackgroundColor = AppConstants.PopupBackground;
		}
	}
}
