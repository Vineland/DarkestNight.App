﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public partial class ChooseHeroesPage : ContentPage
	{
		public ChooseHeroesPage ()
		{
			InitializeComponent ();
			AvailableHeroesListView.ItemTapped += AvailableHeroesListView_ItemTapped;
		}

		void AvailableHeroesListView_ItemTapped (object sender, ItemTappedEventArgs e)
		{

			(BindingContext as ChooseHeroesViewModel).SelectHero (e.Item as Hero);
			AvailableHeroesListView.SelectedItem = null;
		}

		protected override void OnDisappearing ()
		{
			(BindingContext as BaseViewModel).OnDisappearing ();
		}
	}
}

