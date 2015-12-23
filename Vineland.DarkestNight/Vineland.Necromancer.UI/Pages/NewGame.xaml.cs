using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using Vineland.DarkestNight.UI.ViewModels;
using Vineland.DarkestNight.Core;

namespace Vineland.Necromancer.UI
{
	public partial class NewGame : ContentPage
	{
		public NewGame ()
		{
			InitializeComponent();

			BindingContext = IoC.Get<NewGameViewModel>();
			//TODO: figure out why selecteditem binding doesn't work
			DarknessCardsModePicker.SelectedIndex = (int)(BindingContext as NewGameViewModel).Mode;
			DarknessCardsModePicker.SelectedIndexChanged += DarknessCardsModePicker_SelectedIndexChanged;
		}

		void DarknessCardsModePicker_SelectedIndexChanged (object sender, EventArgs e)
		{
			(BindingContext as OptionsViewModel).DarknessCardsMode = (DarknessCardsMode)DarknessCardsModePicker.SelectedIndex;
		}
	}
}

