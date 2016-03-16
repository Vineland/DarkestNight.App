using System;
using Xamarin.Forms;
using Vineland.Necromancer.Core;
using System.Security.Policy;
using Java.Net;
using Dalvik.SystemInterop;
using GalaSoft.MvvmLight;
using System.Threading;

namespace Vineland.Necromancer.UI
{
	public class HeroPage : ContentPage
	{
		public HeroPage ()
		{	
		}

		protected override void OnBindingContextChanged ()
		{
			var heroViewModel = BindingContext as HeroViewModel;
			if (heroViewModel == null)
				return;
			
			var absoluteLayout = new AbsoluteLayout {
				Padding = new Thickness (20, 0, 20, 40)
			};

			var nameLabel = new Label () {

				FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label)),
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				Text = heroViewModel.Name,
			};

			absoluteLayout.Children.Add (nameLabel, new Rectangle (0, 0, 1, 48), AbsoluteLayoutFlags.WidthProportional);

			var image = new Image () {
				Source = ImageSource.FromFile (heroViewModel.Name.Replace (" ", string.Empty).ToLower ()),
				HorizontalOptions = LayoutOptions.Center
			};
			absoluteLayout.Children.Add (image, new Rectangle (0.5, 48, 72, 72), AbsoluteLayoutFlags.XProportional);

			var grid = new Grid () {
				ColumnDefinitions = new ColumnDefinitionCollection(){
					new ColumnDefinition() { Width = new GridLength(32, GridUnitType.Absolute)},
					new ColumnDefinition() { Width = new GridLength(48, GridUnitType.Absolute)},
					new ColumnDefinition() { Width = new GridLength(32, GridUnitType.Absolute)},
					new ColumnDefinition() { Width = new GridLength(48, GridUnitType.Absolute)},
				},
				RowDefinitions = new RowDefinitionCollection(){
					new RowDefinition() { Height = new GridLength(32, GridUnitType.Absolute)}
				}
			};


			grid.Children.Add (new Image () { Source = ImageSource.FromFile ("grace") }, 0, 0);

			var gracePicker = new BindablePicker<string> ();
			gracePicker.ItemsSource = heroViewModel.GraceOptions;
			gracePicker.SetBinding<HeroViewModel> (BindablePicker<string>.SelectedIndexProperty, h => h.Grace);
			grid.Children.Add (gracePicker, 1,0);

			grid.Children.Add (new Image () { Source = ImageSource.FromFile ("secrecy") }, 2, 0);

			var secrecyPicker = new BindablePicker<string> ();
			secrecyPicker.ItemsSource = heroViewModel.SecrecyOptions;
			secrecyPicker.SetBinding<HeroViewModel> (BindablePicker<string>.SelectedIndexProperty, h => h.Secrecy);
			grid.Children.Add (secrecyPicker, 3,0);

			absoluteLayout.Children.Add (grid,
				new Rectangle (0.5, 128, 160, 32),
				AbsoluteLayoutFlags.XProportional);

			absoluteLayout.Children.Add (new Label () { Text = "Location", VerticalTextAlignment = TextAlignment.Center },
				new Rectangle (0, 168, 0.6, 32),
				AbsoluteLayoutFlags.WidthProportional);

			var locationPicker = new BindablePicker<Location> () { ItemsSource = heroViewModel.Locations};
			locationPicker.SetBinding<HeroViewModel> (BindablePicker<Location>.SelectedIndexProperty, h => h.Location);

			absoluteLayout.Children.Add (locationPicker,
				new Rectangle (1, 168, 0.4, 32),
				AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.XProportional);

			//TODO: this option stuff needs to be revisited at some point
			var optionLabel = string.Empty;
			var property = string.Empty;
			var offset = 0;

			if (heroViewModel is RangerViewModel) {
				optionLabel = "Hermit";
				property = "HermitActive";
			} else if (heroViewModel is ParagonViewModel) {
				optionLabel = "Aura of Humility";
				property = "AuraOfHumilityActive";
			} else if (heroViewModel is WizardViewModel) {
				optionLabel = "Rune of Misdirection";
				property = "RuneOfMisdirectionActive";
			} else if (heroViewModel is SeerViewModel) {
				optionLabel = "Prophecy of Doom";
				property = "ProphecyOfDoomRoll";
			} else if (heroViewModel is WayfarerViewModel) {
				optionLabel = "Decoy";
				property = "DecoyActive";
			} else if (heroViewModel is ValkyrieViewModel) {
				optionLabel = "Elusive Spirit";
				property = "ElusiveSpiritActive";
			} else if (heroViewModel is AcolyteViewModel) {
				optionLabel = "Blinding Black";
				property = "BlindingBlackActive";
			} else if (heroViewModel is ScholarViewModel) {
				optionLabel = "Ancient Defense";
				property = "AncientDefenseLocation";
			} else if (heroViewModel is ConjurerViewModel) {
				optionLabel = "Invisible Barrier";
				property = "InvisibleBarrierLocation";
			} else if (heroViewModel is ShamanViewModel){
				optionLabel = "Spirit Sight";
				property = "SpiritSightCommand";
			}

			if (!string.IsNullOrEmpty (property)) {
				offset = 40;
				absoluteLayout.Children.Add (new Label () { Text = optionLabel, VerticalTextAlignment = TextAlignment.Center }, 
					new Rectangle (0, 208, 0.8, 32),
					AbsoluteLayoutFlags.WidthProportional);

				if (property == "ProphecyOfDoomRoll") {
					var picker = new CustomStepper () { Maximum = 6 };
					picker.SetBinding (CustomStepper.ValueProperty, new Binding (property));
					absoluteLayout.Children.Add (picker, 
						new Rectangle (1, 208, 96, 32),
						AbsoluteLayoutFlags.XProportional);

				} else if (property == "InvisibleBarrierLocation" || property == "AncientDefenseLocation") {
					var spinner = new BindablePicker<Location> () { IncludeEmptyOption = true, ItemsSource = heroViewModel.Locations };
					spinner.SetBinding (BindablePicker<Location>.SelectedItemProperty, new Binding (property));

					absoluteLayout.Children.Add (spinner, 
						new Rectangle (1, 208, 0.4, 32),
						AbsoluteLayoutFlags.XProportional | AbsoluteLayoutFlags.WidthProportional);
				} else if (property == "SpiritSightCommand") {
					var button = new Button ();
					button.SetBinding (Button.TextProperty, new Binding ("SpiritSightButtonLabel"));
					button.SetBinding (Button.CommandProperty, new Binding (property));
					absoluteLayout.Children.Add (button, 
						new Rectangle (1, 208, 0.4, 32),
						AbsoluteLayoutFlags.XProportional | AbsoluteLayoutFlags.WidthProportional);
				}
				else 
				{
					var switchControl = new CheckButton ();
					switchControl.SetBinding (CheckButton.IsSelectedProperty, new Binding (property));
					absoluteLayout.Children.Add (switchControl, 
						new Rectangle (1, 208, 32, 32),
						AbsoluteLayoutFlags.XProportional);
				}
			}

			absoluteLayout.Children.Add (new Label () { Text = "Void Armor", VerticalTextAlignment = TextAlignment.Center }, 				
				new Rectangle(0, 208 + offset,0.6,32),
				AbsoluteLayoutFlags.WidthProportional);
			
			var voidAmorControl = new CheckButton ();
			voidAmorControl.SetBinding<HeroViewModel> (CheckButton.IsSelectedProperty, h=>h.HasVoidArmor);
			absoluteLayout.Children.Add (voidAmorControl, 
				new Rectangle(1, 208 + offset, 32,32),
				AbsoluteLayoutFlags.XProportional);

			absoluteLayout.Children.Add (new Label () { Text = "Shield of Radiance", VerticalTextAlignment = TextAlignment.Center }, 
				new Rectangle(0, 248 + offset, 0.8, 32),
				AbsoluteLayoutFlags.WidthProportional);

			var sheildOfRadianceControl = new CheckButton ();
			sheildOfRadianceControl.SetBinding<HeroViewModel> (CheckButton.IsSelectedProperty, h=>h.HasShieldOfRadiance);
			absoluteLayout.Children.Add (sheildOfRadianceControl, 
				new Rectangle(1, 248 + offset, 32,32),
				AbsoluteLayoutFlags.XProportional);

			var stackLayout = new StackLayout () { Spacing = 8 };

			var changeHeroButton = new Button () {
				Text = "Defeated",
				HorizontalOptions = LayoutOptions.Fill
			};
			//changeHeroButton.SetBinding (Button.CommandProperty, new Binding ("RemoveHero"));
			changeHeroButton.Clicked += async (sender, e) => {
				var result = await DisplayAlert("Confirm", "Has this hero has been defeated?", "Yes", "No");
				if(result)
					heroViewModel.RemoveHero();
			};
			stackLayout.Children.Add (changeHeroButton);

//			var blightsButton = new Button () {
//				Text = "Blights",
//				HorizontalOptions = LayoutOptions.Fill,
//			};
//			blightsButton.SetBinding (Button.CommandProperty, new Binding ("Blights", source: this.GetParentPage ().BindingContext));
//			stackLayout.Children.Add (blightsButton);
//
			var searchButton = new Button () {
			Text = "Search",
				HorizontalOptions = LayoutOptions.Fill,
			};
			searchButton.Clicked += async (sender, e) => {
				var option = await DisplayActionSheet("Number Of Cards To Draw", "Cancel", null, "1", "2", "3", "4");
				heroViewModel.Search(option);
			};
			stackLayout.Children.Add (searchButton);

//			var darknessStepper = new CustomStepper ();
//			darknessStepper.SetBinding (CustomStepper.ValueProperty, new Binding ("Darkness", source: this.GetParentPage ().BindingContext));
//			stackLayout.Children.Add (darknessStepper);

			absoluteLayout.Children.Add (stackLayout,
				new Rectangle (0, 1, 1, 80),
				AbsoluteLayoutFlags.YProportional | AbsoluteLayoutFlags.WidthProportional);


			Content = absoluteLayout;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
		}
	}
}


