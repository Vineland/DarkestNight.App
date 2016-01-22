using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using Vineland.Necromancer.Core;
using XLabs.Ioc;
using Xamarin.Forms.Xaml;

namespace Vineland.Necromancer.UI
{
	public partial class Options : ContentPage
	{
		public Options ()
		{
			Title = "Options";
		}

		protected override void OnBindingContextChanged ()
		{
			InitializeComponent ();
			DarknessCardsModePicker.SelectedIndex = (int)(BindingContext as OptionsViewModel).DarknessCardsMode;
			DarknessCardsModePicker.SelectedIndexChanged += DarknessCardsModePicker_SelectedIndexChanged;
		}

		void DarknessCardsModePicker_SelectedIndexChanged (object sender, EventArgs e)
		{
			(BindingContext as OptionsViewModel).DarknessCardsMode = (DarknessCardsMode)DarknessCardsModePicker.SelectedIndex;
		}
	}
}

