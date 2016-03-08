using System;
using Vineland.Necromancer.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Android.Util;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Android.Views.InputMethods;
using Android.Nfc.CardEmulators;
using Java.Nio.Channels;

namespace Vineland.Necromancer.UI
{
	public class NecromancerSpawnViewModel : BaseViewModel
	{
		NecromancerService _necromancerService;
		NecromancerDetectionResult _detectionResult;
		NecromancerSpawnResult _spawnResult;

		public NecromancerSpawnViewModel (NecromancerService necromancerService)
		{
			_necromancerService = necromancerService;
		}

		GameState _editableGameState;

		public void Initialise (NecromancerDetectionResult detectionResult, GameState editableGameState)
		{
			_editableGameState = editableGameState;
			_detectionResult = detectionResult;

			_spawnResult = _necromancerService.Spawn (detectionResult.NewLocation, detectionResult.MovementRoll, _editableGameState);
			var models = _spawnResult.NewBlights.GroupBy (x => x.Item1).Select (x => new BlightLocationViewModel (x.Key.Name, x.Select (y => y.Item2)));
			NewBlightLocations = new ObservableCollection<BlightLocationViewModel> (models);
		}

		public string Notes {get;set;}
		public string SpawnQuestMessage { get; set; }

		public ObservableCollection<BlightLocationViewModel> NewBlightLocations { get; set; }


//		public void DestroyBlightCommand (SpawnViewModel prospectiveBlight)
//		{
//			//if a blight is instantly destroyed we need to:
//			// 1) discard the cards used to spawn it
//			prospectiveBlight.Cards.ForEach (c => Application.CurrentGame.MapCards.Discard (c));
//			prospectiveBlight.Cards.Clear ();
//			// 2) decrement the required number of blights to spawn
//			if (prospectiveBlight.Location.Id != LocationIds.Monastery)
//				_blightsToSpawnAtNewLocation--;
//			else
//				_blightsToSpawnAtMonastery--;
//					
//			// 3) get the prospective blights again (in case this removal has changed spill over blights to the monastery)
//			GetAllProspectiveBlights ();					
//		}
//
		public RelayCommand AcceptCommand {
			get {
				return new RelayCommand (() => 
					{
						//TODO: rethink this whole process. It would be nice to just set the game state to the edited one but that
						//breaks all bindings.
						//propagate all the changes to the game state
						Application.CurrentGame.MapCards = _editableGameState.MapCards;
						Application.CurrentGame.Necromancer.LocationId = _editableGameState.Necromancer.LocationId;
						Application.CurrentGame.Locations = _editableGameState.Locations;
						Application.CurrentGame.BlightPool = _editableGameState.BlightPool;

						Application.SaveCurrentGame ();

						Application.Navigation.PopTo<HeroPhasePage>();
				});
			}
		}
	}
}

