using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using Vineland.Necromancer.Core;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public partial class NewGame : ContentPageBase<NewGameViewModel>
	{
		public NewGame ()
		{
			InitializeComponent();
			Title = "Game Setup";

			DarknessCardsModePicker.SelectedIndex = (int)(BindingContext as NewGameViewModel).Mode;
			DarknessCardsModePicker.SelectedIndexChanged += DarknessCardsModePicker_SelectedIndexChanged;
		}

		void DarknessCardsModePicker_SelectedIndexChanged (object sender, EventArgs e)
		{
			(BindingContext as NewGameViewModel).Mode = (DarknessCardsMode)DarknessCardsModePicker.SelectedIndex;
		}
	}
}

