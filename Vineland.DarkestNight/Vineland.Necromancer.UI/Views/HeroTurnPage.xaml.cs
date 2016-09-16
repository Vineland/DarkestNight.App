using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Vineland.Necromancer.Core;
using System.Threading.Tasks;

namespace Vineland.Necromancer.UI
{
	public partial class HeroTurnPage : ContentPage
	{
		public HeroTurnPage ()
		{ 
			InitializeComponent ();

			BlightsListView.ItemTapped += (sender, e) => {
				BlightsListView.SelectedItem = null;
			};
		}
	}
}

