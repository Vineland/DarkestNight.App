using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Vineland.DarkestNight.UI.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Vineland.DarkestNight.UI;
using Android.InputMethodServices;

namespace Vineland.Necromancer.UI
{
	public class HomeViewModel : BaseViewModel
	{
		public HomeViewModel ()
		{
		}

		public RelayCommand ContinueGameCommand {
			get { 
				return new RelayCommand (
					() => {
						Application.Navigation.Push<HeroPhasePage> ();
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
