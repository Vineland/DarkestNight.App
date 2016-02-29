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
			
			var absoluteLayout = new AbsoluteLayout {
				Padding = new Thickness(20,0,20,40)
			};

			var nameLabel = new Label () {

				FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				Text = hero.Name.ToUpper (),
			};

			absoluteLayout.Children.Add (nameLabel, new Rectangle (0, 0, 1, 48), AbsoluteLayoutFlags.WidthProportional);

			var image = new Image () {
				Source = ImageSource.FromFile (hero.Name.Replace (" ", string.Empty).ToLower ()),
				HorizontalOptions = LayoutOptions.Center
			};
			absoluteLayout.Children.Add (image, new Rectangle(0.5,48, 96,96), AbsoluteLayoutFlags.XProportional);


			absoluteLayout.Children.Add (new Label () { Text = "Secrecy", VerticalTextAlignment = TextAlignment.Center },
				new Rectangle(0, 152, 0.6, 32),
				AbsoluteLayoutFlags.WidthProportional);

			var secrecyStepper = new CustomStepper () { Maximum = 9 };
			secrecyStepper.SetBinding<Hero> (CustomStepper.ValueProperty, h => h.Secrecy);
			absoluteLayout.Children.Add (secrecyStepper, 
				new Rectangle(1, 152, 96, 32),
				AbsoluteLayoutFlags.XProportional);

			absoluteLayout.Children.Add (new Label () { Text = "Location", VerticalTextAlignment = TextAlignment.Center },
				new Rectangle (0, 192, 0.6, 32),
				AbsoluteLayoutFlags.WidthProportional);

			var locationPicker = new BindablePicker<Location> ();
			locationPicker.SetBinding<Hero> (BindablePicker<Location>.SelectedIndexProperty, h => h.LocationId);
			locationPicker.SetBinding (BindablePicker<Location>.ItemsSourceProperty, new Binding ("Locations", source: this.GetParentPage ().BindingContext));

			absoluteLayout.Children.Add (locationPicker,
				new Rectangle(1,192,0.4,32),
				AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.XProportional);

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
				//absoluteLayout.Children.Add (new AcolyteOptions(), 0, 2, 5, 6);				
				break;
			case "Scholar":
				optionLabel = "Ancient Defense";
				property = "AncientDefenseLocationId";
				break;
			case "Conjurer":
				optionLabel = "Invisible Barrier";
				property = "InvisibleBarrierLocationId";
				break;
//			default:
//				absoluteLayout.Children.Add (new HeroView (), 0, 2, 5, 6);
//				break;
//			case "Shaman":
//				optionLabel = "Spirit Sight";
//				property = "SpiritSightMapCard";
//				break;
			}
//
			int offset = 0;
			if (!string.IsNullOrEmpty (property)) {
				offset = 40;
				absoluteLayout.Children.Add (new Label () { Text = optionLabel, VerticalTextAlignment = TextAlignment.Center }, 
					new Rectangle(0,232,0.6,32),
					AbsoluteLayoutFlags.WidthProportional);

				if (property == "ProphecyOfDoomRoll") {
					var picker = new CustomStepper () { Maximum = 6};
					picker.SetBinding (CustomStepper.ValueProperty, new Binding (property));
					absoluteLayout.Children.Add (picker, 
						new Rectangle(1,232, 96,32),
						AbsoluteLayoutFlags.XProportional);

				} else {
					var switchControl = new CheckButton ();
					switchControl.SetBinding (CheckButton.IsSelectedProperty, new Binding (property));
					absoluteLayout.Children.Add (switchControl, 
						new Rectangle(1,232, 32,32),
						AbsoluteLayoutFlags.XProportional);
				}
			} 
			absoluteLayout.Children.Add (new Label () { Text = "Void Amour", VerticalTextAlignment = TextAlignment.Center }, 
				new Rectangle(0,string.IsNullOrEmpty(property)? 232: 272,0.6,32),
				AbsoluteLayoutFlags.WidthProportional);
			
			var voidAmorControl = new CheckButton ();
			voidAmorControl.SetBinding<Hero> (CheckButton.IsSelectedProperty, h=>h.HasVoidArmor);
			absoluteLayout.Children.Add (voidAmorControl, 
				new Rectangle(1, 232 + offset, 32,32),
				AbsoluteLayoutFlags.XProportional);

			var changeHeroButton = new Button () {
				Text = "Defeated",
				HorizontalOptions = LayoutOptions.Fill
			};
			changeHeroButton.SetBinding (Button.CommandProperty, new Binding ("RemoveHero", source: this.GetParentPage ().BindingContext));
			changeHeroButton.CommandParameter = hero;
			absoluteLayout.Children.Add (changeHeroButton,
				new Rectangle(0,272 + offset, 1, 48),
				AbsoluteLayoutFlags.WidthProportional);

			var blightsButton = new Button () 
			{
				Text = "Blights",
				HorizontalOptions = LayoutOptions.Fill,
			};
			blightsButton.SetBinding (Button.CommandProperty, new Binding ("Blights", source: this.GetParentPage ().BindingContext));
			absoluteLayout.Children.Add (blightsButton,
				new Rectangle(0,320 + offset, 1, 48),
				AbsoluteLayoutFlags.WidthProportional);

			var stackLayout = new StackLayout() {Spacing=10};
			stackLayout.Children.Add (new Image () {
				Source = ImageSource.FromFile ("darkness")
			});
			var darknessStepper = new CustomStepper ();
			darknessStepper.SetBinding (CustomStepper.ValueProperty, new Binding ("Darkness", source: this.GetParentPage ().BindingContext));
			stackLayout.Children.Add (darknessStepper);

			absoluteLayout.Children.Add (stackLayout,
				new Rectangle (0.5, 1, 96, 90),
				AbsoluteLayoutFlags.PositionProportional);

			Content = absoluteLayout;
		}
	}
}


