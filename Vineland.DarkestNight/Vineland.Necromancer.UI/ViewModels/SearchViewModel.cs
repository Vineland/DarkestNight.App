using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace Vineland.Necromancer.UI
{
	public class SearchViewModel : BaseViewModel
	{

		public SearchViewModel ()
		{
			MapCards = new List<MapCardViewModel> ();
			var card = Application.CurrentGame.MapCards.Draw();
			MapCards.Add(new MapCardViewModel(card));
		}

		public List<MapCardViewModel> MapCards { get; set; }

		public MapCardViewModel CurrentMapCard { get; set; }

		public RelayCommand DoneCommand {
			get {
				return new RelayCommand (() => {
					//the cards have already been discarded in the editable game state
					//so this just commits those changes to the current game state
					Application.CurrentGame.MapCards.Discard(MapCards.First().MapCard);
					Application.Navigation.Pop ();
				});
			}
		}
	}
}

