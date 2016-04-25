using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class HeroPhaseDataTemplateSelector :DataTemplateSelector
	{
		private readonly DataTemplate _heroTemplate;
		private readonly DataTemplate _blightsTemplate;

		public HeroPhaseDataTemplateSelector ()
		{

			_heroTemplate = new DataTemplate(typeof(HeroCell));
			_blightsTemplate = new DataTemplate(typeof(LocationBlightsCell));
		}

		protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
		{
			if (item is HeroSummaryViewModel)
				return _heroTemplate;

			if (item is Vineland.Necromancer.UI.HeroTurnViewModel.LocationBlightsViewModel)
				return _blightsTemplate;

			return new DataTemplate();
		}
	}
}

