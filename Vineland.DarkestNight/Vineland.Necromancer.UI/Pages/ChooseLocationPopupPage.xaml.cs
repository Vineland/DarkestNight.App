using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class ChooseLocationPopupPage : PopupPage
	{

		public ChooseLocationPopupPage()
		{
			InitializeComponent();
			CloseWhenBackgroundIsClicked = true;
		}
	}
}
