using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public class SelectHeroViewModel :BaseViewModel
	{
		public SelectHeroViewModel (DataService dataService)
		{
			AvailableHeroes = dataService.GetAllHeroes().Where(x => !Application.CurrentGame.Heroes.Any(y => y.Id == x.Id)).ToList();
		}

		public IList<Hero> AvailableHeroes { get; private set; }

		Hero _selectedHero;
		public Hero SelectedHero
		{ 
			get { return _selectedHero; } 
			set {
				_selectedHero = value;
				if (value != null) 
				{
					if (HeroSelected != null)
						HeroSelected (this, value);
					
					Application.Navigation.Pop();
				}
			}
		}

		public event EventHandler<Hero> HeroSelected;
	}
}

