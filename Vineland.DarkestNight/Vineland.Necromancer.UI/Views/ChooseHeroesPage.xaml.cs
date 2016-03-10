using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class ChooseHeroesPage : ContentPage
	{
		public ChooseHeroesPage ()
		{
			Title = "Select Heroes";

			InitializeComponent ();
		}

		protected override void OnDisappearing ()
		{
			(BindingContext as BaseViewModel).OnDisappearing ();
		}
	}
}

