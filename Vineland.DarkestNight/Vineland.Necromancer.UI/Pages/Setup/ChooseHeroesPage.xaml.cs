using Xamarin.Forms;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public partial class ChooseHeroesPage : BasePage
	{
		public ChooseHeroesPage ()
		{
			InitializeComponent ();
			AvailableHeroesListView.ItemTapped += AvailableHeroesListView_ItemTapped;
		}

		void AvailableHeroesListView_ItemTapped (object sender, ItemTappedEventArgs e)
		{

			(BindingContext as ChooseHeroesViewModel).SelectHero (e.Item as Hero);
			AvailableHeroesListView.SelectedItem = null;
		}
	}
}

