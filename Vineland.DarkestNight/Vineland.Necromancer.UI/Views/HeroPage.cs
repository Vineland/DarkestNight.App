using System;

using Xamarin.Forms;
using Vineland.Necromancer.Core;
using System.Security.Policy;
using Java.Net;
using Dalvik.SystemInterop;

namespace Vineland.Necromancer.UI
{
	public class HeroPage : ContentPage
	{
		public HeroPage ()
		{	
		}

		protected override void OnParentSet ()
		{
			var hero = BindingContext as Hero;
			if (hero == null)
				return;
			
			var grid = new Grid {
				Padding = new Thickness(20,20,20,40),
				RowSpacing = 10,
				RowDefinitions = {
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = new GridLength (1, GridUnitType.Star) },
					new RowDefinition { Height = GridLength.Auto },
				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
				}
			};

			var nameLabel = new Label () {

				FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
				HorizontalOptions = LayoutOptions.Center,
				Text = hero.Name.ToUpper (),
			};
			grid.Children.Add (nameLabel, 0, 2, 0, 1);

			var image = new Image () {
				Source = ImageSource.FromFile (hero.Name.Replace (" ", string.Empty).ToLower ()),
				HorizontalOptions = LayoutOptions.Center
			};
			grid.Children.Add (image, 0, 2, 1, 2);
			grid.Children.Add (new Label () { Text = "Secrecy", VerticalOptions = LayoutOptions.Center }, 0, 2);

			var secrecyStepper = new CustomStepper () { HorizontalOptions = LayoutOptions.End, Maximum = 9 };
			secrecyStepper.SetBinding<Hero> (CustomStepper.ValueProperty, h => h.Secrecy);
			grid.Children.Add (secrecyStepper, 1, 2);

			grid.Children.Add (new Label () { Text = "Location", VerticalOptions = LayoutOptions.Center }, 0, 3);

			var locationPicker = new BindablePicker<Location> ();
			locationPicker.SetBinding<Hero> (BindablePicker<Location>.SelectedIndexProperty, h => h.LocationId);
			locationPicker.SetBinding (BindablePicker<Location>.ItemsSourceProperty, new Binding ("Locations", source: this.GetParentPage ().BindingContext));

			grid.Children.Add (locationPicker, 1, 3);

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
			case "Scholar":
				optionLabel = "Ancient Defense";
				property = "AncientDefenseLocationId";
				break;
			case "Conjurer":
				optionLabel = "Invisible Barrier";
				property = "InvisibleBarrierLocationId";
				break;
//			case "Shaman":
//				optionLabel = "Spirit Sight";
//				property = "SpiritSightMapCard";
//				break;
			}

			if (!string.IsNullOrEmpty (property)) {

				grid.Children.Add (new Label () { Text = optionLabel, VerticalOptions = LayoutOptions.Center }, 0, 4);

				if (property == "ProphecyOfDoomRoll") {
					var picker = new CustomStepper () { Maximum = 6, HorizontalOptions = LayoutOptions.End };
					picker.SetBinding (CustomStepper.ValueProperty, new Binding (property));
					grid.Children.Add (picker, 1, 4);

				} else {
					var switchControl = new CheckButton () { HorizontalOptions = LayoutOptions.End };
					switchControl.SetBinding (CheckButton.IsSelectedProperty, new Binding (property));
					grid.Children.Add (switchControl, 1, 4);
				}
			} 

			var changeHeroButton = new Button () {
				Text = "Defeated",
				HorizontalOptions = LayoutOptions.Fill
			};
			changeHeroButton.SetBinding (Button.CommandProperty, new Binding ("RemoveHero", source: this.GetParentPage ().BindingContext));
			changeHeroButton.CommandParameter = hero;
			grid.Children.Add (changeHeroButton, 0, 2, 5, 6);

			var blightsButton = new Button () 
			{
				Text = "Blights",
				HorizontalOptions = LayoutOptions.Fill,
			};
			blightsButton.SetBinding (Button.CommandProperty, new Binding ("Blights", source: this.GetParentPage ().BindingContext));
			grid.Children.Add (blightsButton, 0, 2, 6, 7);

			grid.Children.Add (new Image () {
				Source = ImageSource.FromFile ("darkness"),
				VerticalOptions = LayoutOptions.End,
				HorizontalOptions = LayoutOptions.Center
			}, 0, 2, 7, 8);
			var darknessStepper = new CustomStepper () { HorizontalOptions = LayoutOptions.Center };
			darknessStepper.SetBinding (CustomStepper.ValueProperty, new Binding ("Darkness", source: this.GetParentPage ().BindingContext));
			grid.Children.Add (darknessStepper, 0, 2, 8, 9);

			Content = grid;
		}
	}
}


