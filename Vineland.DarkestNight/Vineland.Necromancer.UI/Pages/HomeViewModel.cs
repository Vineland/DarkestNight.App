using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class HomeViewModel : BaseViewModel
	{
		public HomeViewModel()
		{

		}

		public RelayCommand PlayGameCommand {
			get { 
				return new RelayCommand (
					async () => {
						if(Application.CurrentGame == null
							|| !await Application.Navigation.DisplayConfirmation("Continue the last game?", null, "Yes", "No"))
							await Application.Navigation.Push<NewGamePage>();
						else
						await Application.Navigation.Push<HeroesPage>();
					}); 
			}
		}

		public RelayCommand HelpCommand{
			get{
				return new RelayCommand (() => {
					//await Application.Navigation.Push<SettingsPage>();
				});
			}
		}
	}
}
