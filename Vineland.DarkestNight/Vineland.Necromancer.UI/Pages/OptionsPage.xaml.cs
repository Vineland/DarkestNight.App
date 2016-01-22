using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using Vineland.Necromancer.Core;
using XLabs.Ioc;
using Xamarin.Forms.Xaml;

namespace Vineland.Necromancer.UI
{
	public partial class OptionsPage : ContentPage
	{
		public OptionsPage ()
		{
			Title = "Options";
			InitializeComponent ();
		}

		protected override void OnBindingContextChanged ()
		{
			DarknessCardsModePicker.SelectedIndex = (int)(BindingContext as OptionsViewModel).Settings.DarknessCardsMode;
			DarknessCardsModePicker.SelectedIndexChanged += DarknessCardsModePicker_SelectedIndexChanged;
		}

		void DarknessCardsModePicker_SelectedIndexChanged (object sender, EventArgs e)
		{
			(BindingContext as OptionsViewModel).Settings.DarknessCardsMode = (DarknessCardsMode)DarknessCardsModePicker.SelectedIndex;
		}
	}
}

