using System;
using Xamarin.Forms;
using Vineland.DarkestNight.Core;
using Android.Provider;
using Dalvik.SystemInterop;
using Android.Content;

namespace Vineland.Necromancer.UI
{
	public class OptionsX : ContentPageBase<OptionsViewModel>
	{
		public OptionsX ()
		{
		}

		protected override void OnBindingContextChanged ()
		{
			var grid = new Grid {
				RowSpacing = 20,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = {
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
				}
			};

			grid.Children.Add (new Label () { Text = "Game Mode" }, 0, 0);
			var modePicker = new BindablePicker<DarknessCardsMode> ();
			modePicker.ItemsSource = ViewModel.DarknessCardsModeOptions;
			modePicker.SetBinding<OptionsViewModel> (BindablePicker<DarknessCardsMode>.SelectedItemProperty, vm => vm.Settings.DarknessCardsMode);
			grid.Children.Add (modePicker, 1, 0);

			grid.Children.Add (new Label () { "Starting Darkness" }, 0, 1);
			var darknessStepper = new CustomStepper ();
			darknessStepper.SetBinding<OptionsViewModel> (CustomStepper.ValueProperty, vm => vm.Settings.StartingDarkness);
			grid.Children.Add (darknessStepper, 1, 1);

			grid.Children.Add (new Label () { "Pall Of Suffering" }, 0, 2);
			var pallOfSufferingSwitch = new Switch ();
			pallOfSufferingSwitch.SetBinding<OptionsViewModel> (Switch.IsToggledProperty, vm => vm.Settings.PallOfSuffering);
			grid.Children.Add (pallOfSufferingSwitch, 1, 2);

			grid.Children.Add (new Label () { "Always Use" }, 0, 3);
			var alwaysUseSwitch = new Switch ();
			pallOfSufferingSwitch.SetBinding<OptionsViewModel> (Switch.IsToggledProperty, vm => vm.Settings.AlwaysUseDefaults);
			grid.Children.Add (alwaysUseSwitch, 1, 3);

			Content = grid;
		}
	}
}

