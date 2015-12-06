using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vineland.DarkestNight.Core;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;

namespace Vineland.DarkestNight.UI.ViewModels
{
	public class ChooseHeroesViewModel : ViewModelBase
    {
        AppGameState _gameState;

        public ChooseHeroesViewModel(AppGameState gameState)
        {
            _gameState = gameState;
        }

        public void Initialise()
        {
			AllHeroes = Hero.All;
        }

        public List<Hero> AllHeroes { get; set; }

		public RelayCommand<Hero> AddHero
		{

			get{
				return new RelayCommand<Hero> (
					(hero) => {
						if (!_gameState.Heroes.Active.Any(x => x.Id == hero.Id))
							_gameState.Heroes.Active.Add(hero);
					},
					(hero) => {
						return _gameState.Heroes.Active.Count < 4;
					}
				);
			}
		}

		public RelayCommand<Hero> RemoveHero 
		{
			get { 
				return new RelayCommand<Hero> ((hero) => {
					if (hero != null)
						_gameState.Heroes.Active.Remove (hero);
				});
			}
		}

		public RelayCommand StartGame{
			get {
				return new RelayCommand (
					() => {
						_gameState.Save ();
					},
					() => {
						return _gameState.Heroes.Active.Count == 4;
					}
				);
			}
		}
    }
}
