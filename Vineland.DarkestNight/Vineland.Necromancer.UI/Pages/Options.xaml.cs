using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using Vineland.DarkestNight.Core;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public partial class Options : ContentPageBase<OptionsViewModel>
	{
		public Options ()
		{
			InitializeComponent ();
			Title = "Options";

			DarknessCardsModePicker.SelectedIndex = (int)(BindingContext as OptionsViewModel).DarknessCardsMode;
			DarknessCardsModePicker.SelectedIndexChanged += DarknessCardsModePicker_SelectedIndexChanged;
		}

		void DarknessCardsModePicker_SelectedIndexChanged (object sender, EventArgs e)
		{
			(BindingContext as OptionsViewModel).DarknessCardsMode = (DarknessCardsMode)DarknessCardsModePicker.SelectedIndex;
		}
	}
}

