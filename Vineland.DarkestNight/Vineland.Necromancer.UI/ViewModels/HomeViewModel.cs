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

			RaisePropertyChanged (() => ContinueGameCommand);
		}

		public RelayCommand ContinueGameCommand {
			get { 
				return new RelayCommand (
					() => {
						Application.Navigation.Push<ActiveHeroesPage> ();
					},
					() => {
						return Application.CurrentGame != null;
					}); 
			}
		}

		public RelayCommand NewGameCommand {
			get { 
				return new RelayCommand (
					() => { 
						Application.Navigation.Push<NewGamePage> ();
					}); 
			}
		}

		public RelayCommand SettingsCommand{
			get{
				return new RelayCommand (() => {
					Application.Navigation.Push<SettingsPage>();
				});
			}
		}
	}
}
