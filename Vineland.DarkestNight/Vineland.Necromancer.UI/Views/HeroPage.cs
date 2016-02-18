using System;

using Xamarin.Forms;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class HeroPage : ContentPage
	{
		public HeroPage ()
		{	
			
		}

		protected override void OnParentSet ()
		{
			base.OnParentSet ();

			var hero = BindingContext as Hero;

			this.Padding = new Thickness (20, 60, 20, 0);
			var grid = new Grid {
				RowSpacing = 20,
				RowDefinitions = {
					new RowDefinition { Height = GridLength.Auto },
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

			var nameLabel = new Label () {

				FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HorizontalTextAlignment = TextAlignment.Center,
				Text = hero.Name.ToUpper ()
			};
			grid.Children.Add (nameLabel, 0, 2, 0, 1);

			grid.Children.Add (new Label () { Text = "Secrecy", VerticalOptions = LayoutOptions.Center }, 0, 1);

			var secrecyStepper = new CustomStepper () { HorizontalOptions = LayoutOptions.End, Maximum = 9 };
			secrecyStepper.SetBinding<Hero> (CustomStepper.ValueProperty, h => h.Secrecy);
			grid.Children.Add (secrecyStepper, 1, 1);

			grid.Children.Add (new Label () { Text = "Location", VerticalOptions = LayoutOptions.Center }, 0, 2);

			var locationPicker = new BindablePicker<Location> ();
			locationPicker.SetBinding<Hero> (BindablePicker<Location>.SelectedIndexProperty, h => h.LocationId);
			locationPicker.ItemsSource = Location.All;

			grid.Children.Add (locationPicker, 1, 2);

			var optionLabel = string.Empty;
			var property = string.Empty;
			switch (hero.Name) {
			case "Ranger":
				optionLabel = "Hermit";
				property = "HermitActive";
				break;
			case "Paragon":
				optionLabel = "Aura of Humility";
				property = "AuraOfHumilityActive";
				break;
			case "Wizard":
				optionLabel = "Rune of Misdirection";
				property = "RuneOfMisdirectionActive";
				break;
			case "Seer":
				optionLabel = "Prophecy of Doom";
				property = "ProphecyOfDoomRoll";
				break;
			case "Wayfarer":
				optionLabel = "Decoy";
				property = "DecoyActive";
				break;
			case "Valkyrie":
				optionLabel = "Elusive Spirit";
				property = "ElusiveSpiritActive";
				break;
			case "Acolyte":
				optionLabel = "Blinding Black";
				property = "BlindingBlackActive";
				break;
			}

			if (!string.IsNullOrEmpty (property)) {

				grid.Children.Add (new Label () { Text = optionLabel, VerticalOptions = LayoutOptions.Center }, 0, 3);

				if (property == "ProphecyOfDoomRoll") {
					var picker = new CustomStepper () { Maximum = 6, HorizontalOptions = LayoutOptions.End };
					picker.SetBinding(CustomStepper.ValueProperty, new Binding("BindingContext.HeroesState." + property, source: this.Parent));
					grid.Children.Add (picker, 1, 3);

				} else {
					var switchControl = new CheckButton () { HorizontalOptions = LayoutOptions.End };
					switchControl.SetBinding (CheckButton.IsSelectedProperty, new Binding ("BindingContext.HeroesState." + property, source: this.GetParentPage ()));
					grid.Children.Add (switchControl, 1, 3);
				}
			} 

			var changeHeroButton = new Button () {
				Text = "Change Hero",
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			changeHeroButton.SetBinding (Button.CommandProperty, new Binding ("BindingContext.RemoveHero", source: this.GetParentPage ()));
			changeHeroButton.CommandParameter = hero;
			grid.Children.Add (changeHeroButton, 0, 2, 4, 5);

			Content = grid;
		}
	}
}


