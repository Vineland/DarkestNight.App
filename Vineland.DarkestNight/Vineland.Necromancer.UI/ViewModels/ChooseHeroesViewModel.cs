using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vineland.DarkestNight.Core;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Android.Util;

namespace Vineland.DarkestNight.UI.ViewModels
{
	public class ChooseHeroesViewModel : ViewModelBase
    {
        AppGameState _gameState;

        public ChooseHeroesViewModel(AppGameState gameState)
		{
			_gameState = gameState;
			SelectedHeroes = new ObservableCollection<Hero> ();
			SelectedHeroes.CollectionChanged += (sender, e) => { RaisePropertyChanged(()=> StartGame);};
		}
 
		public List<Hero> Heroes { get { return _gameState.Heroes.All; } }

		public ObservableCollection<Hero> SelectedHeroes { get; set; }

		public RelayCommand StartGame{
			get {
				return new RelayCommand (
					() => {
						_gameState.Save ();
					},
					() => {
						return SelectedHeroes.Count() == 4;
					}
				);
			}
		}
    }
}
