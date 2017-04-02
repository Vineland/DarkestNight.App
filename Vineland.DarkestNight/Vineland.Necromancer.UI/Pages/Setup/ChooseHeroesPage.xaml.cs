using Xamarin.Forms;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public partial class ChooseHeroesPage : ContentPage
	{
		public ChooseHeroesPage ()
		{
			InitializeComponent ();
			AvailableHeroesListView.ItemTapped += AvailableHeroesListView_ItemTapped;
		}

		void AvailableHeroesListView_ItemTapped (object sender, ItemTappedEventArgs e)
		{

			(BindingContext as ChooseHeroesPageModel).SelectHero (e.Item as HeroViewModel);
			AvailableHeroesListView.SelectedItem = null;
		}
	}
}

