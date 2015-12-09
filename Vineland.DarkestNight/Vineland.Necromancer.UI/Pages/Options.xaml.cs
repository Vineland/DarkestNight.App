using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.DarkestNight.UI.ViewModels;
using Vineland.DarkestNight.UI;
using Vineland.DarkestNight.Core;

namespace Vineland.Necromancer.UI
{
	public partial class Options : ContentPage
	{
		public Options ()
		{
			InitializeComponent ();
			BindingContext = IoC.Get<OptionsViewModel> ();

			//TODO: figure out why selecteditem binding doesn't work
			DarknessCardsModePicker.SelectedIndex = (int)(BindingContext as OptionsViewModel).DarknessCardsMode;
			DarknessCardsModePicker.SelectedIndexChanged += DarknessCardsModePicker_SelectedIndexChanged;
		}

		void DarknessCardsModePicker_SelectedIndexChanged (object sender, EventArgs e)
		{
			(BindingContext as OptionsViewModel).DarknessCardsMode = (DarknessCardsMode)DarknessCardsModePicker.SelectedIndex;
		}
	}
}

