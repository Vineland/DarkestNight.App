using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class HomePageModel : BaseViewModel
	{
		public HomePageModel()
		{

		}

		public Command PlayGameCommand {
			get { 
				return new Command (
					async (x) => {
						if(Application.CurrentGame == null
					       || !await CoreMethods.DisplayAlert("Continue the last game?", null, "Yes", "No"))
							await CoreMethods.PushPageModel<NewGamePageModel>();
						else
							await CoreMethods.PushPageModel<HeroesPageModel>();
					}); 
			}
		}

		public Command HelpCommand{
			get{
				return new Command (() => {
					//await Application.Navigation.Push<SettingsPage>();
				});
			}
		}
	}
}
