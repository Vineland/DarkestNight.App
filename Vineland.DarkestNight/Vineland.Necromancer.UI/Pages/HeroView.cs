using System;

using Xamarin.Forms;
using Vineland.DarkestNight.Core;

namespace Vineland.Necromancer.UI
{
	public class HeroView : ContentView
	{
		public HeroView ()
		{	
			
		}

		protected override void OnParentSet ()
		{
			base.OnParentSet ();

			var hero = BindingContext as Hero;

			var grid = new Grid
			{
				RowSpacing=20,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = 
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
				},
				ColumnDefinitions = 
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				}
				};

			var nameLabel = new Label (){

				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			nameLabel.SetBinding<Hero>(Label.TextProperty, h => h.Name);
			grid.Children.Add (nameLabel, 0, 2,0,1);

			grid.Children.Add (new Label() { Text= "Secrecy", VerticalOptions=LayoutOptions.Center}, 0, 1);

			var secrecyStepper = new CustomStepper () { HorizontalOptions = LayoutOptions.End};
			secrecyStepper.SetBinding<Hero>(CustomStepper.ValueProperty, h => h.Secrecy);
			grid.Children.Add (secrecyStepper, 1, 1);

			grid.Children.Add (new Label() { Text= "Location", VerticalOptions=LayoutOptions.Center}, 0, 2);

			var locationPicker = new BindablePicker ();
			locationPicker.SetBinding<Hero> (BindablePicker.SelectedIndexProperty, h => h.LocationId);
			locationPicker.ItemsSource = Location.All;

			grid.Children.Add(locationPicker, 1, 2);

			var optionLabel = string.Empty;
			var property = string.Empty;
			switch (hero.Name) {
			case "Rogue":
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
				property = "ProphecyOfDoomAttained";
				break;
			case "Wayfarer":
				optionLabel = "Decoy";
				property = "DecoyAttained";
				break;
			case "Valkyrie":
				optionLabel = "Elusive Spirit";
				property = "ElusiveSpiritAttained";
				break;
			case "Acolyte":
				optionLabel = "Blinding Black";
				property = "BlindingBlackAttained";
				break;
			}

			if (!string.IsNullOrEmpty(property)) {
				grid.Children.Add (new Label () { Text = optionLabel, VerticalOptions=LayoutOptions.Center}, 0,3);
				var switchControl = new Switch () { HorizontalOptions = LayoutOptions.End};
				switchControl.SetBinding (Switch.IsToggledProperty, new Binding ("BindingContext.Heroes." + property, source: this.GetParentPage ()));
				grid.Children.Add (switchControl, 1, 3);
			} 

			var changeHeroButton = new Button (){
				Text = "Change Hero",
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			changeHeroButton.SetBinding (Button.CommandProperty, new Binding ("BindingContext.RemoveHero", source: this.GetParentPage ()));
			changeHeroButton.CommandParameter = hero;
			grid.Children.Add (changeHeroButton, 0, 2,4,5);
			//TODO
			this.WidthRequest = this.ParentView.Width;// 300;
			this.MinimumWidthRequest =  this.ParentView.Width;//300;
			Content = grid;
		}
	}
}


