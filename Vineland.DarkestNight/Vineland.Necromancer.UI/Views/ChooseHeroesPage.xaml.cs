using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class ChooseHeroesPage : ContentPage
	{
		public ChooseHeroesPage ()
		{
			InitializeComponent ();

			Title = "Select Heroes";

			BackgroundImage = "background";
		}

		protected override void OnDisappearing ()
		{
			(BindingContext as BaseViewModel).OnDisappearing ();
		}
	}
}

