using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class HeroesView : ContentView
	{
		public HeroesView ()
		{
			Content = new StackLayout ();
		}

		protected override void OnBindingContextChanged ()
		{
			var viewModel = (BindingContext as HeroesViewModel);
			if (viewModel != null) {
				viewModel.Heroes.CollectionChanged += ViewModel_Heroes_CollectionChanged;
				Bind ();
			}
		}

		private void Bind(){

			var viewModel = (BindingContext as HeroesViewModel);
			var stackLayout = (Content as StackLayout);
			stackLayout.Children.Clear ();

			foreach (var hero in viewModel.Heroes)
				stackLayout.Children.Add (new HeroCell () { BindingContext = hero });


			//TODO: yeah...
			//if (viewModel.AcolytePresent || viewModel.ConjurerPresent || viewModel.ParagonPresent
			//   || viewModel.RangerPresent || viewModel.ScholarPresent || viewModel.SeerPresent
			//   || viewModel.ValkyriePresent || viewModel.WayfarerPresent || viewModel.WizardPresent)
			//	stackLayout.Children.Add (new Header () { Text = "Powers" });

			//if (viewModel.AcolytePresent) {
			//	var layout = new StackLayout () { Orientation = StackOrientation.Horizontal };
			//	layout.Children.Add (new Label () { Text = "Blinding Black" });
			//	var checkBox = new CheckButton ();
			//	checkBox.SetBinding (CheckButton.IsSelectedProperty, "HeroesState.BlindingBlackActive");
			//	layout.Children.Add (checkBox);
			//	stackLayout.Children.Add (layout);
			//}
			//if (viewModel.ConjurerPresent) {
			//	stackLayout.Children.Add (new Label () { Text = "Invisible Barrier Location" });
			//}
			//if (viewModel.ParagonPresent) {
			//	stackLayout.Children.Add (new Label () { Text = "Aura of Humility" });
			//}
			//if (viewModel.RangerPresent) {
			//	stackLayout.Children.Add (new Label () { Text = "Hermit" });
			//}
			//if (viewModel.WizardPresent) {
			//	stackLayout.Children.Add (new Label () { Text = "Rune of Misdirection" });
			//}
			//if (viewModel.SeerPresent) {
			//	stackLayout.Children.Add (new Label () { Text = "Prophecy of Doom" });
			//}
			//if (viewModel.ValkyriePresent) {
			//	stackLayout.Children.Add (new Label () { Text = "Elusive Spirit" });
			//}
			//if (viewModel.WayfarerPresent) {
			//	stackLayout.Children.Add (new Label () { Text = "Decoy" });
			//}
			//if (viewModel.ScholarPresent) {
			//	stackLayout.Children.Add (new Label () { Text = "Blinding Black" });
			//}

			stackLayout.Children.Add (new Header () { Text = "Artifacts" });
			stackLayout.Children.Add (new Label () { Text = "Void Armour" });
			stackLayout.Children.Add (new Label () { Text = "Shield of Radiance" });
		}

		void ViewModel_Heroes_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Bind ();
		}
	}
}

