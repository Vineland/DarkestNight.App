using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using Vineland.DarkestNight.UI.ViewModels;

namespace Vineland.Necromancer.UI
{
	public partial class ChooseHeroes : ContentPage
	{
		public ChooseHeroes ()
		{
			InitializeComponent ();
			BindingContext = IoC.Get<ChooseHeroesViewModel> ();

			//HeroesListView
		}
	}
}

