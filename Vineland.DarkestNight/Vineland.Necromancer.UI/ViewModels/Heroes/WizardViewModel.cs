using System;
using Vineland.Necromancer.Core;

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
	}
}

