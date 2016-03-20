using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;

namespace Vineland.Necromancer.UI
{
	public class SearchViewModel : BaseViewModel
	{
		GameState _editableGameState;

		public enum MapDeckContext
		{
			Search
		}

		public SearchViewModel (int cardsToDraw)
		{
			_editableGameState = Application.CurrentGame.Clone ();
			MapCards = new List<MapCardViewModel> ();
			for (int i = 0; i < cardsToDraw; i++) {
				var card = _editableGameState.MapCards.Draw ();
				_editableGameState.MapCards.Discard (card);
				MapCards.Add (new MapCardViewModel (card));
			}
		}

		public List<MapCardViewModel> MapCards { get; set; }

		public RelayCommand DiscardCommand {
			get {
				return new RelayCommand (() => {
					//the cards have already been discarded in the editable game state
					//so this just commits those changes to the current game state
					MapCards.ForEach (x => _editableGameState.MapCards.Discard (x.MapCard));
					Application.CurrentGame.MapCards = _editableGameState.MapCards;
					Application.Navigation.Pop ();
				});
			}
		}

		public void ReturnToTop ()
		{
			MapCards.ForEach (x => x.ReturnToTop ());
		}

		public void ReturnToBottom ()
		{
			MapCards.ForEach (x => x.ReturnToBottom ());
		}
	}
}

