using System;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class WizardViewModel : HeroViewModel
	{
		Wizard _wizard;

		public WizardViewModel (Wizard wizard)
			: base(wizard)
		{
			_wizard = wizard;
		}

		public bool RuneOfMisdirectionActive{
			get{ return _wizard.RuneOfMisdirectionActive; }
			set{ _wizard.RuneOfMisdirectionActive = value; }
		}

		public bool RuneOfClairvoyanceActive{
			get{ return _wizard.RuneOfClairvoyanceActive; }
			set{ _wizard.RuneOfClairvoyanceActive = value; }
		}
	}
}

