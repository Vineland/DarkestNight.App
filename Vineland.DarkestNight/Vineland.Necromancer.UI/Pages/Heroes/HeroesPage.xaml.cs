using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class HeroesPage : ContentPage
	{
		public HeroesPage()
		{
			InitializeComponent();
		}

	}

	public class HeroesDataTemplateSelector : DataTemplateSelector
	{
		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (item is SeerViewModel)
				return new DataTemplate(typeof(SeerView));

			return new DataTemplate(typeof(HeroView));
		}
	}
}
