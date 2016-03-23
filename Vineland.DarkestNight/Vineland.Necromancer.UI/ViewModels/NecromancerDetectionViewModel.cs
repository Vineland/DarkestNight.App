using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.UI;
using XLabs.Ioc;

namespace Vineland.Necromancer.UI
{
	public class NecromancerDetectionViewModel :BaseViewModel
	{
		NecromancerService _necromancerService;

		#region Notable Heroes

		Seer _seer;
		Wizard _wizard;

		#endregion

		public NecromancerDetectionViewModel (NecromancerService necromancerService)
		{
			_necromancerService = necromancerService;

			_seer = Application.CurrentGame.Heroes.GetHero<Seer> ();
			_wizard = Application.CurrentGame.Heroes.GetHero<Wizard> ();

			Results = new List<NecromancerActivationResultViewModel> ();

			if (_seer == null || _seer.ProphecyOfDoomRoll == 0)
				Results.Add (new NecromancerActivationResultViewModel (_necromancerService));
			else
				Results.Add (new NecromancerActivationResultViewModel (_necromancerService, roll: _seer.ProphecyOfDoomRoll));

			if (_wizard != null && _wizard.RuneOfMisdirectionActive)
				Results.Add (new NecromancerActivationResultViewModel (_necromancerService));

			SelectedResult = Results.First ();
		}

		public List<NecromancerActivationResultViewModel> Results { get; set; }

		public NecromancerActivationResultViewModel SelectedResult { get; set; }


		public RelayCommand SpawnCommand {
			get {
				return new RelayCommand (() => {
					
					var viewModel = Resolver.Resolve<NecromancerSpawnViewModel> ();
					viewModel.Initialise (SelectedResult.Result, SelectedResult.PendingGameState.Clone());

					Application.Navigation.Push<NecromancerSpawnPage> (viewModel);
				});
			}
		}
	}
}

