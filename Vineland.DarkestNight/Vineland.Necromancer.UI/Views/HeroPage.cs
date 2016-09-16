using System;
using Xamarin.Forms;
using Vineland.Necromancer.Core;
using System.Security.Policy;
using GalaSoft.MvvmLight;
using System.Threading;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public class HeroPage : ContentPage
	{
		public HeroPage ()
		{	
		}

//			protected override void OnBindingContextChanged ()
//			{
//				var heroViewModel = BindingContext as HeroViewModel;
//				if (heroViewModel == null)
//					return;
//
//				Title = heroViewModel.Name;
//				BackgroundImage = "background_framed";
//				
//				var absoluteLayout = new AbsoluteLayout {
//					Padding = new Thickness (20, 0, 20, 40)
//				};
//
//				var image = new Image () {
//					Source = ImageSource.FromFile (heroViewModel.Name.Replace (" ", string.Empty).ToLower ()),
//					HorizontalOptions = LayoutOptions.Center
//				};
//				absoluteLayout.Children.Add (image, new Rectangle (0.5, 12, 60, 60), AbsoluteLayoutFlags.XProportional);
//
//				var grid = new Grid () {
//					ColumnDefinitions = new ColumnDefinitionCollection(){
//						new ColumnDefinition() { Width = new GridLength(32, GridUnitType.Absolute)},
//						new ColumnDefinition() { Width = new GridLength(48, GridUnitType.Absolute)},
//						new ColumnDefinition() { Width = new GridLength(32, GridUnitType.Absolute)},
//						new ColumnDefinition() { Width = new GridLength(48, GridUnitType.Absolute)},
//					},
//					RowDefinitions = new RowDefinitionCollection(){
//						new RowDefinition() { Height = new GridLength(32, GridUnitType.Absolute)}
//					}
//				};
//
//
//				grid.Children.Add (new Image () { Source = ImageSource.FromFile ("grace") }, 0, 0);
//
//				var gracePicker = new BindablePicker<string> ();
//				gracePicker.ItemsSource = heroViewModel.GraceOptions;
//				gracePicker.SetBinding<HeroViewModel> (BindablePicker<string>.SelectedIndexProperty, h => h.Grace);
//				grid.Children.Add (gracePicker, 1,0);
//
//				grid.Children.Add (new Image () { Source = ImageSource.FromFile ("secrecy") }, 2, 0);
//
//				var secrecyPicker = new BindablePicker<string> ();
//				secrecyPicker.ItemsSource = heroViewModel.SecrecyOptions;
//				secrecyPicker.SetBinding<HeroViewModel> (BindablePicker<string>.SelectedIndexProperty, h => h.Secrecy);
//				grid.Children.Add (secrecyPicker, 3,0);
//
//				absoluteLayout.Children.Add (grid,
//					new Rectangle (0.5, 96, 160, 32),
//					AbsoluteLayoutFlags.XProportional);
//
//				absoluteLayout.Children.Add (new Label () { Text = "Location", VerticalTextAlignment = TextAlignment.Center },
//					new Rectangle (0, 136, 0.6, 32),
//					AbsoluteLayoutFlags.WidthProportional);
//
//				var locationPicker = new BindablePicker<Location> () { ItemsSource = heroViewModel.Locations};
//				locationPicker.SetBinding<HeroViewModel> (BindablePicker<Location>.SelectedItemProperty, h => h.Location);
//
//				absoluteLayout.Children.Add (locationPicker,
//					new Rectangle (1, 136, 0.4, 32),
//					AbsoluteLayoutFlags.WidthProportional | AbsoluteLayoutFlags.XProportional);
//
//				//TODO: this option stuff needs to be revisited at some point
//				var optionLabel = string.Empty;
//				var property = string.Empty;
//				var offset = 0;
//
//				if (heroViewModel is RangerViewModel) {
//					optionLabel = "Hermit";
//					property = "HermitActive";
//				} else if (heroViewModel is ParagonViewModel) {
//					optionLabel = "Aura of Humility";
//					property = "AuraOfHumilityActive";
//				} else if (heroViewModel is WizardViewModel) {
//					optionLabel = "Rune of Misdirection";
//					property = "RuneOfMisdirectionActive";
//				} else if (heroViewModel is SeerViewModel) {
//					optionLabel = "Prophecy of Doom";
//					property = "ProphecyOfDoomRoll";
//				} else if (heroViewModel is WayfarerViewModel) {
//					optionLabel = "Decoy";
//					property = "DecoyActive";
//				} else if (heroViewModel is ValkyrieViewModel) {
//					optionLabel = "Elusive Spirit";
//					property = "ElusiveSpiritActive";
//				} else if (heroViewModel is AcolyteViewModel) {
//					optionLabel = "Blinding Black";
//					property = "BlindingBlackActive";
//				} else if (heroViewModel is ScholarViewModel) {
//					optionLabel = "Ancient Defense";
//					property = "AncientDefenseLocation";
//				} else if (heroViewModel is ConjurerViewModel) {
//					optionLabel = "Invisible Barrier";
//					property = "InvisibleBarrierLocation";
//				}// else if (heroViewModel is ShamanViewModel){
//				//	optionLabel = "Spirit Sight";
//				//	property = "SpiritSightCommand";
//				//}
//
//				if (!string.IsNullOrEmpty (property)) {
//					offset = 40;
//					absoluteLayout.Children.Add (new Label () { Text = optionLabel, VerticalTextAlignment = TextAlignment.Center }, 
//						new Rectangle (0, 176, 0.8, 32),
//						AbsoluteLayoutFlags.WidthProportional);
//
//					if (property == "ProphecyOfDoomRoll") {
//						var picker = new CustomStepper () { Maximum = 6 };
//						picker.SetBinding (CustomStepper.ValueProperty, new Binding (property));
//						absoluteLayout.Children.Add (picker, 
//							new Rectangle (1, 176, 96, 32),
//							AbsoluteLayoutFlags.XProportional);
//
//					} else if (property == "InvisibleBarrierLocation" || property == "AncientDefenseLocation") {
//						var spinner = new BindablePicker<Location> () { IncludeEmptyOption = true, ItemsSource = heroViewModel.Locations };
//						spinner.SetBinding (BindablePicker<Location>.SelectedItemProperty, new Binding (property));
//
//						absoluteLayout.Children.Add (spinner, 
//							new Rectangle (1, 176, 0.4, 32),
//							AbsoluteLayoutFlags.XProportional | AbsoluteLayoutFlags.WidthProportional);
//		//				} else if (property == "SpiritSightCommand") {
//		//					offset = 56;
//		//					var button = new Button ();
//		//					button.SetBinding (Button.TextProperty, new Binding ("SpiritSightButtonLabel"));
//		//					button.SetBinding (Button.CommandProperty, new Binding (property));
//		//					absoluteLayout.Children.Add (button, 
//		//						new Rectangle (1, 176, 0.4, 48),
//		//						AbsoluteLayoutFlags.XProportional | AbsoluteLayoutFlags.WidthProportional);
//					}
//					else 
//					{
//						var switchControl = new CheckButton ();
//						switchControl.SetBinding (CheckButton.IsSelectedProperty, new Binding (property));
//						absoluteLayout.Children.Add (switchControl, 
//							new Rectangle (1, 176, 32, 32),
//							AbsoluteLayoutFlags.XProportional);
//					}
//				}
//
//				absoluteLayout.Children.Add (new Label () { Text = "Void Armor", VerticalTextAlignment = TextAlignment.Center }, 				
//					new Rectangle(0, 176 + offset,0.6,32),
//					AbsoluteLayoutFlags.WidthProportional);
//				
//				var voidAmorControl = new CheckButton ();
//				voidAmorControl.SetBinding<HeroViewModel> (CheckButton.IsSelectedProperty, h=>h.HasVoidArmor);
//				absoluteLayout.Children.Add (voidAmorControl, 
//					new Rectangle(1, 176 + offset, 32,32),
//					AbsoluteLayoutFlags.XProportional);
//
//				absoluteLayout.Children.Add (new Label () { Text = "Shield of Radiance", VerticalTextAlignment = TextAlignment.Center }, 
//					new Rectangle(0, 216 + offset, 0.8, 32),
//					AbsoluteLayoutFlags.WidthProportional);
//
//				var sheildOfRadianceControl = new CheckButton ();
//				sheildOfRadianceControl.SetBinding<HeroViewModel> (CheckButton.IsSelectedProperty, h=>h.HasShieldOfRadiance);
//				absoluteLayout.Children.Add (sheildOfRadianceControl, 
//					new Rectangle(1, 216 + offset, 32,32),
//					AbsoluteLayoutFlags.XProportional);
//
//
//				var defeatedButton = new ToolbarItem () { Text = "Defeated" };
//				defeatedButton.SetBinding<HeroViewModel> (ToolbarItem.CommandProperty, vm => vm.DefeatedCommand);
//				ToolbarItems.Add (defeatedButton);
//
//		//			var stackLayout = new StackLayout () { Spacing = 10, Orientation = StackOrientation.Horizontal };
//		//
//		//			var defeatedButton = new Button () {
//		//				Image = ImageSource.FromFile("death") as FileImageSource,
//		//				WidthRequest = 48
//		//			};
//		//			defeatedButton.SetBinding (Button.CommandProperty, new Binding ("DefeatedCommand"));
//		//
//		//			stackLayout.Children.Add (defeatedButton);
//
//		//			var blightsButton = new Button () {
//		//				Text = "Blights",
//		//				HorizontalOptions = LayoutOptions.Fill,
//		//			};
//		//			blightsButton.SetBinding (Button.CommandProperty, new Binding ("Blights", source: this.GetParentPage ().BindingContext));
//		//			stackLayout.Children.Add (blightsButton);
//		//
//		//			var searchButton = new Button () {
//		//				Image = ImageSource.FromFile("search") as FileImageSource,
//		//					WidthRequest = 48
//		//			};
//		//			searchButton.SetBinding (Button.CommandProperty, new Binding ("SearchCommand"));
//		//			absoluteLayout.Children.Add (searchButton,
//		//				new Rectangle (96, 1, 48, 48),
//		//				AbsoluteLayoutFlags.YProportional);
//		//			stackLayout.Children.Add (searchButton);
//
//		//			var darknessStepper = new CustomStepper ();
//		//			darknessStepper.SetBinding (CustomStepper.ValueProperty, new Binding ("Darkness", source: this.GetParentPage ().BindingContext));
//		//			stackLayout.Children.Add (darknessStepper);
//
//		//			absoluteLayout.Children.Add (stackLayout,
//		//				new Rectangle (0.5, 1, 106, 48),
//		//				AbsoluteLayoutFlags.XProportional | AbsoluteLayoutFlags.YProportional);
//
//
//				Content = absoluteLayout;
//			}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			(BindingContext as BaseViewModel).OnDisappearing ();
		}
	}
}


