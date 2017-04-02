using Vineland.Necromancer.Core;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class SearchPageModel : BaseViewModel
	{
		
		public SearchPageModel ()
		{
			MapCards = new ObservableCollection<MapCardViewModel> ();

			//use a copy of the deck so that's it's easy to bail out 
			//if the user decides to undo a bunch of stuff
			//_mapDeck = Application.CurrentGame.Clone().MapCards;
			MapCards.Add(new MapCardViewModel(Application.CurrentGame.MapCards.Draw()));
		}

		//Deck<Domain.MapCard> _mapDeck;

		public ObservableCollection<MapCardViewModel> MapCards { get; set; }

		public MapCardViewModel SelectedMapCard
		{
			get
			{
				if (SelectedMapCardIndex < 0)
					return null;
				
				return MapCards[SelectedMapCardIndex];
			}
		}

		public int SelectedMapCardIndex { get; set; }

		public Command DrawCommand
		{
			get
			{
				return new Command(() =>
				{
					var viewModel = new MapCardViewModel(Application.CurrentGame.MapCards.Draw());
					MapCards.Add(viewModel);
					SelectedMapCardIndex = MapCards.Count - 1;
					RaisePropertyChanged(() => SelectedMapCardIndex);
				});
			}
		}

		public Command DiscardCommand
		{
			get
			{
				return new Command(() =>
				{
					if (SelectedMapCard != null)
					{
						Application.CurrentGame.MapCards.Discard(SelectedMapCard.MapCard);
						MapCards.Remove(SelectedMapCard);
					}
				},
				() => { return MapCards.Any(); });
			}
		}

		public Command ReturnToBottomCommand
		{
			get
			{
				return new Command(() =>
				{
					if (SelectedMapCard != null)
					{
						Application.CurrentGame.MapCards.Return(SelectedMapCard.MapCard, DeckPosition.Bottom);
						MapCards.Remove(SelectedMapCard);
					}
				}, 
                () => { return MapCards.Any(); });
			}
		}

		public Command ReturnToTopCommand
		{
			get
			{
				return new Command(() =>
				{
					if (SelectedMapCard != null)
					{
						Application.CurrentGame.MapCards.Return(SelectedMapCard.MapCard);
						MapCards.Remove(SelectedMapCard);
					}
				},
				() => { return MapCards.Any(); });
			}
		}

		public Command DoneCommand {
			get {
				return new Command (() => {
					DiscardAll();
					//Application.CurrentGame.MapCards = _mapDeck;
					CoreMethods.PopPageModel();
				});
			}
		}

		private void DiscardAll()
		{
			foreach (var mapCard in MapCards)
			{
				Application.CurrentGame.MapCards.Discard(mapCard.MapCard);
			}

		}

		protected override void ViewIsDisappearing(object sender, System.EventArgs e)
		{
			base.ViewIsDisappearing(sender, e);

			DiscardAll();
		}
	}
}

