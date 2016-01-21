using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using Vineland.Necromancer.Core;
using XLabs.Ioc;
using Xamarin.Forms.Xaml;

namespace Vineland.Necromancer.UI
{
	[XamlCompilation (XamlCompilationOptions.Compile)]
	public partial class Options : ContentPageBase<OptionsViewModel>
	{
		public Options ()
		{
			InitializeComponent ();
			Title = "Options";

			DarknessCardsModePicker.ItemsSource = ViewModel.DarknessCardsModeOptions;
			DarknessCardsModePicker.SelectedIndex = (int)(BindingContext as OptionsViewModel).DarknessCardsMode;
			DarknessCardsModePicker.SelectedIndexChanged += DarknessCardsModePicker_SelectedIndexChanged;
		}

		void DarknessCardsModePicker_SelectedIndexChanged (object sender, EventArgs e)
		{
			(BindingContext as OptionsViewModel).DarknessCardsMode = (DarknessCardsMode)DarknessCardsModePicker.SelectedIndex;
		}
	}
}

