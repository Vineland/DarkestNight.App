using System;
using Vineland.Necromancer.Core;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using GalaSoft.MvvmLight.Command;

namespace Vineland.Necromancer.UI
{
	public class NecromancerActivationViewModel : BaseViewModel
	{
		NecromancerService _necromancerService;
		Wizard _wizard;
		Seer _seer;

		public NecromancerActivationViewModel (NecromancerService necromancerService)
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

		public RelayCommand AcceptCommand {
			get {
				return new RelayCommand (() => 
					{
						//TODO: rethink this whole process. It would be nice to just set the game state to the edited one but that
						//breaks all bindings.
						//propagate all the changes to the game state
						Application.CurrentGame.Darkness = SelectedResult.PendingGameState.Darkness;
						Application.CurrentGame.MapCards = SelectedResult.PendingGameState.MapCards;
						Application.CurrentGame.Necromancer.LocationId = SelectedResult.PendingGameState.Necromancer.LocationId;
						Application.CurrentGame.Locations = SelectedResult.PendingGameState.Locations;
						Application.CurrentGame.BlightPool = SelectedResult.PendingGameState.BlightPool;
						//prophecy of doom is always reset after the necromancer phase
						var seer = Application.CurrentGame.Heroes.GetHero<Seer>();
						if(seer != null)
							seer.ProphecyOfDoomRoll = 0;
						//the state of these powers may have changed during the necromancer phase
						var conjurer = SelectedResult.PendingGameState.Heroes.GetHero<Conjurer>();
						if(conjurer != null)
							Application.CurrentGame.Heroes.GetHero<Conjurer>().InvisibleBarrierLocationId = conjurer.InvisibleBarrierLocationId; 

						var acolyte = SelectedResult.PendingGameState.Heroes.GetHero<Acolyte>();
						if(acolyte != null)
							Application.CurrentGame.Heroes.GetHero<Acolyte>().BlindingBlackActive = acolyte.BlindingBlackActive; 

						var shaman = SelectedResult.PendingGameState.Heroes.GetHero<Shaman>();
						if(shaman != null)
							Application.CurrentGame.Heroes.GetHero<Shaman>().SpiritSightMapCard = shaman.SpiritSightMapCard; 

						Application.SaveCurrentGame ();

						MessagingCenter.Send<NecromancerActivationViewModel>(this, "NecromancerPhaseComplete");

						Application.Navigation.PopTo<HeroTurnPage>();
					});
			}
		}
	}
}

