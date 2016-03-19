using System;
using System.Threading.Tasks;

namespace Vineland.Necromancer.UI
{
	public interface IDisplayAlertService
	{
		void DisplayAlert (string title, string message, string cancel);

		Task<bool> DisplayConfirmation (string title, string message, string accept, string cancel);

		string DisplayActionSheet (string title, string cancel, string destruction, params string[] buttons);
	}
}

