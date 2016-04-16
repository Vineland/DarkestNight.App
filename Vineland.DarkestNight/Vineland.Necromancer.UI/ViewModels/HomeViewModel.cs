using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Vineland.DarkestNight.UI;
using Vineland.DarkestNight.UI.Services;
using Vineland.Necromancer.Core;

namespace Vineland.Necromancer.UI
{
	public class HomeViewModel : BaseViewModel
	{
		public HomeViewModel ()
		{
		}

		public override void OnAppearing ()
		{
			if (Application.CurrentGame == null && Application.FileService.DoesFileExist (AppConstants.SaveFilePath))
				Application.CurrentGame = JsonConvert.DeserializeObject<GameState> (Application.FileService.LoadFile (AppConstants.SaveFilePath), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
		}

		public RelayCommand PlayGameCommand {
			get { 
				return new RelayCommand (
					async () => {
						if(Application.CurrentGame == null
							|| !await Application.Navigation.DisplayConfirmation("Continue the last game?", null, "Yes", "No"))
							Application.Navigation.Push<NewGamePage>();
						else
							Application.Navigation.Push<HeroTurnPage> ();
					}); 
			}
		}

		public RelayCommand SettingsCommand{
			get{
				return new RelayCommand (async () => {
					await Application.Navigation.Push<SettingsPage>();
				});
			}
		}

		public RelayCommand HelpCommand{
			get{
				return new RelayCommand (async () => {
					//await Application.Navigation.Push<SettingsPage>();
				});
			}
		}
	}
}
