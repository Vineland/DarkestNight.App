using System;
using Vineland.Necromancer.Core;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Vineland.Necromancer.UI
{
	public class NecromancerActivationResultViewModel : BaseViewModel
	{
		NecromancerActivationResult _result;
		NecromancerService _necromancerService;

		public GameState PendingGameState { get; private set; }

		Acolyte _acolyte;
		Wayfarer _wayfarer;
		Valkyrie _valkyrie;
		Shaman _shaman;

		public NecromancerActivationResultViewModel (NecromancerService necroService, int? roll = null)
		{
			_necromancerService = necroService;

			ActivateNecromancer (roll: roll);

			_acolyte = Application.CurrentGame.Heroes.GetHero<Acolyte> ();
			_wayfarer = Application.CurrentGame.Heroes.GetHero <Wayfarer> ();
			_valkyrie = Application.CurrentGame.Heroes.GetHero <Valkyrie> ();
			_shaman = Application.CurrentGame.Heroes.GetHero<Shaman> ();
		}

		private void ActivateNecromancer (Hero detectedHero = null,
			int? roll = null, 
			int[] heroesToIgnore = null)
		{
			PendingGameState = Application.CurrentGame.Clone ();
			Result = _necromancerService.Activate (PendingGameState, detectedHero, roll, heroesToIgnore);
			var models = Result.NewBlights
				.GroupBy (x => x.Item1)
				.Select (x => new SpawnLocationViewModel (x.Key, 
					x.Select (y => y.Item2), false)).ToList();
			if (Result.SpawnQuest) {
				var questLocation = PendingGameState.Locations.Single (l => l.Id == Result.OldLocationId);
				models.Add (new SpawnLocationViewModel (questLocation, null, true));
			}
			NewBlightLocations = new ObservableCollection<SpawnLocationViewModel> (models);
		}

		public NecromancerActivationResult Result {
			get{ return _result; }
			set {
				_result = value;
				RaisePropertyChanged (() => Result);
				RaisePropertyChanged (() => NewLocation);
				RaisePropertyChanged (() => BlindingBlackVisible);
				RaisePropertyChanged (() => DecoyVisible);
				RaisePropertyChanged (() => VoidArmorVisible);
				RaisePropertyChanged (() => ElusiveSpiritVisible);
			}
		}

		public Location NewLocation
		{
			get{ return PendingGameState.Locations.SingleOrDefault (l => l.Id == Result.NewLocationId); }
		}

		public ObservableCollection<SpawnLocationViewModel> NewBlightLocations { get; set; }

		List<int> _heroesToIgnore = new List<int> ();

		public bool VoidArmorVisible {
			get{ return Result.DetectedHero != null && Result.DetectedHero.HasVoidArmor; }
		}

		public RelayCommand VoidArmorCommand {
			get {
				return new RelayCommand (() => {
					_heroesToIgnore.Add (Result.DetectedHero.Id);
					ActivateNecromancer (roll: Result.NecromancerRoll, heroesToIgnore: _heroesToIgnore.ToArray ());
				},
					() => {
						return Result.DetectedHero != null;
					});
			}
		}

		public bool BlindingBlackVisible {
			get {
				return _acolyte != null
					&& _acolyte.BlindingBlackActive
					&& Result.DetectedHero != null;
			}

		}

		public RelayCommand BlindingBlackCommand {
			get {
				return new RelayCommand (() => {
					_heroesToIgnore = PendingGameState.Heroes.Select (x => x.Id).ToList();
					_acolyte.BlindingBlackActive = false;
					ActivateNecromancer (roll: Result.NecromancerRoll, heroesToIgnore: _heroesToIgnore.ToArray ());
				});
			}
		}

		public bool DecoyVisible {
			get {
				return _wayfarer != null
					&& _wayfarer.DecoyActive
					&& Result.DetectedHero != null
					&& _wayfarer.LocationId != (int)LocationIds.Monastery
					&& _wayfarer.Secrecy >= Result.DetectionRoll;
			}
		}

		public RelayCommand DecoyCommand {
			get {
				return new RelayCommand (() => {
					ActivateNecromancer (detectedHero: _wayfarer, roll: Result.NecromancerRoll, heroesToIgnore: _heroesToIgnore.ToArray ());
				});
			}
		}

		public bool ElusiveSpiritVisible {
			get {
				return _valkyrie != null
					&& _valkyrie.ElusiveSpiritActive
					&& Result.DetectedHero != null
					&& Result.DetectionRoll == Result.DetectedHero.Secrecy + 1;
			}
		}

		public RelayCommand ElusiveSpiritCommand {
			get {
				return new RelayCommand (() => {
					//TODO could be triggered more than once
					_heroesToIgnore.Add (Result.DetectedHero.Id);
					ActivateNecromancer(roll:Result.NecromancerRoll, heroesToIgnore: _heroesToIgnore.ToArray ());
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
			get{ return _blight == null ? "Add Quest" : _blight.Name; }
		}

		public string ImageName {
			get { return _blight == null ? "plus" : string.Format ("blight_{0}", _blight.Name.ToLower().Replace(" ", "_")); }
		}

		public bool IsQuest{
			get{ return _blight == null; }
		}
	}
}

