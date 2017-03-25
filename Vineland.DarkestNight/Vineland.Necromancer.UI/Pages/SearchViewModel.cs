﻿using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Collections.ObjectModel;

namespace Vineland.Necromancer.UI
{
	public class SearchViewModel : BaseViewModel
	{
		
		public SearchViewModel ()
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

		public RelayCommand DrawCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					var viewModel = new MapCardViewModel(Application.CurrentGame.MapCards.Draw());
					MapCards.Add(viewModel);
					SelectedMapCardIndex = MapCards.Count - 1;
					RaisePropertyChanged(() => SelectedMapCardIndex);
				});
			}
		}

		public RelayCommand DiscardCommand
		{
			get
			{
				return new RelayCommand(() =>
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

		public RelayCommand ReturnToBottomCommand
		{
			get
			{
				return new RelayCommand(() =>
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

		public RelayCommand ReturnToTopCommand
		{
			get
			{
				return new RelayCommand(() =>
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

		public RelayCommand DoneCommand {
			get {
				return new RelayCommand (() => {
					DiscardAll();
					//Application.CurrentGame.MapCards = _mapDeck;
					Application.Navigation.Pop ();
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

		public override void OnBackButtonPressed()
		{
			DiscardAll();
		}
	}
}

