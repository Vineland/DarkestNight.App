using System;
namespace Vineland.Necromancer.UI
{
	public class DarknessPopupViewModel :BaseViewModel
	{
		public DarknessPopupViewModel()
		{
		}

		public int Darkness
		{
			get { return Application.CurrentGame.Darkness; }
			set
			{
				Application.CurrentGame.Darkness = value;
			}
		}
	}
}
