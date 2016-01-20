using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.DarkestNight.UI;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public partial class ChooseHeroesX : ContentPageBase<ChooseHeroesViewModel>
	{
		public ChooseHeroesX ()
		{
			InitializeComponent ();
			Title = "Choose Heroes";
		}
	}
}

