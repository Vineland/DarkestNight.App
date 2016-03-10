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
using Xamarin.Forms;
using Android.App;

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

		GameState _pendingGameState;

		public void Initialise (NecromancerDetectionResult detectionResult, GameState pendingGameState)
		{
			_pendingGameState = pendingGameState;
			_detectionResult = detectionResult;

			_spawnResult = _necromancerService.Spawn (_pendingGameState, detectionResult);
			var models = _spawnResult.NewBlights
				.GroupBy (x => x.Item1)
				.Select (x => new SpawnLocationViewModel (x.Key, 
					x.Select (y => y.Item2), false)).ToList();
			if (_spawnResult.SpawnQuest) {
				var questLocation = _pendingGameState.Locations.Single (l => l.Id == detectionResult.OldLocationId);
				models.Add (new SpawnLocationViewModel (questLocation, null, true));
			}
			NewBlightLocations = new ObservableCollection<SpawnLocationViewModel> (models);
		}

		public string Notes {get;set;}
		public string SpawnQuestMessage { get; set; }

		public ObservableCollection<SpawnLocationViewModel> NewBlightLocations { get; set; }

		public RelayCommand AcceptCommand {
			get {
				return new RelayCommand (() => 
					{
						//TODO: rethink this whole process. It would be nice to just set the game state to the edited one but that
						//breaks all bindings.
						//propagate all the changes to the game state
						Application.CurrentGame.MapCards = _pendingGameState.MapCards;
						Application.CurrentGame.Necromancer.LocationId = _pendingGameState.Necromancer.LocationId;
						Application.CurrentGame.Locations = _pendingGameState.Locations;
						Application.CurrentGame.BlightPool = _pendingGameState.BlightPool;
						//prophecy of doom is always reset after the necromancer phase
						var seer = Application.CurrentGame.Heroes.GetHero<Seer>();
						if(seer != null)
							seer.ProphecyOfDoomRoll = 0;
						//the state of these powers may have changed during the necromancer phase
						var conjurer = _pendingGameState.Heroes.GetHero<Conjurer>();
						if(conjurer != null)
							Application.CurrentGame.Heroes.GetHero<Conjurer>().InvisibleBarrierLocationId = conjurer.InvisibleBarrierLocationId; 
						
						var acolyte = _pendingGameState.Heroes.GetHero<Acolyte>();
						if(acolyte != null)
							Application.CurrentGame.Heroes.GetHero<Acolyte>().BlindingBlackActive = acolyte.BlindingBlackActive; 

						var shaman = _pendingGameState.Heroes.GetHero<Shaman>();
						if(shaman != null)
							Application.CurrentGame.Heroes.GetHero<Shaman>().SpiritSightMapCard = shaman.SpiritSightMapCard; 

						Application.SaveCurrentGame ();

						MessagingCenter.Send<NecromancerSpawnViewModel>(this, "NecromancerPhaseComplete");

						Application.Navigation.PopTo<HeroPhasePage>();
				});
			}
		}
	}

	public class SpawnLocationViewModel : ObservableCollection<SpawnViewModel>
	{		
		public Location Location { get; private set; }

		public SpawnLocationViewModel (Location location, IEnumerable<Blight> blights, bool spawnQuest)
		{
			Location = location;

			if (blights != null) {
				foreach (var blight in blights) {
					this.Add (new SpawnViewModel (blight));
				}
			}

			if (spawnQuest)
				this.Add (new SpawnViewModel (null));
		}
	}

	public class SpawnViewModel
	{		
		Blight _blight;

		public SpawnViewModel (Blight blight)
		{
			_blight = blight;
		}

		public string Name {
			get{ return _blight == null ? "Spawn Quest" : _blight.Name; }
		}

		public string ImageName {
			get { return _blight == null ? "plus" : string.Format ("blight_{0}", _blight.Name.ToLower().Replace(" ", "_")); }
		}

		public bool IsQuest{
			get{ return _blight == null; }
		}
	}
}

