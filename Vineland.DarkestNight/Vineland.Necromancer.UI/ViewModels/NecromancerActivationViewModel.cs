using System;
using Vineland.Necromancer.Core;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;

namespace Vineland.Necromancer.UI
{
	public class NecromancerActivationViewModel : BaseViewModel
	{
		NecromancerService _necromancerService;
		GameState _pendingGameState;

		public NecromancerActivationViewModel (NecromancerService necromancerService)
		{
			_necromancerService = necromancerService;
			Locations = new ObservableCollection<SpawnLocationViewModel> ();
			IsLoading = true;
		}

		public void Initialise (Hero detectedHero, int necromancerRoll, int [] heroesToIgnore = null)
		{
			Task.Run (() => {
				_pendingGameState = Application.CurrentGame.Clone ();
				SetResult (_necromancerService.Activate (_pendingGameState, detectedHero, necromancerRoll, heroesToIgnore));
				IsLoading = false;
			});
		}

		NecromancerActivationResult _result;

		public void SetResult (NecromancerActivationResult result)
		{
			_result = result;
			RaisePropertyChanged (() => NecromancerRollDisplay);
			RaisePropertyChanged (() => DarknessDisplay);
			RaisePropertyChanged (() => DetectedHeroDisplay);
			RaisePropertyChanged (() => NewLocationDisplay);

			var models = _result.NewBlights
				.GroupBy (x => x.Item1)
				.Select (x => new SpawnLocationViewModel (x.Key, 
				             x.Select (y => y.Item2))).ToList ();
			if (_result.SpawnQuest) 
			{
				var questLocation = _pendingGameState.Locations.Single (l => l.Id == _result.OldLocationId);
				var model = models.FirstOrDefault (x => x.Location.Id == _result.OldLocationId);
				if (model == null) {
					model = new SpawnLocationViewModel (questLocation, null);
					models.Add(model);
				}
				model.Spawns.Add (new QuestViewModel ());
			}
			Locations.Clear ();
			models.ForEach (x => Locations.Add (x));

		}

		public string NecromancerRollDisplay {
			get { 				
				if (_result == null)
					return string.Empty;
				
				var display = _result.NecromancerRoll.ToString ();
				if (_pendingGameState.Locations.Count (l => l.Blights.Any (b => b.Name == "Gate")) > _result.NewBlights.Count (x => x.Item2.Name == "Gate"))
					display += " (+1 detection)";

				return display;
			}
		}

		public string DarknessDisplay {
			get {
				if (_result == null)
					return string.Empty;
				return string.Format ("{0} +{1}", _pendingGameState.Darkness - _result.DarknessIncrease, _result.DarknessIncrease);
			}
		}

		public string DetectedHeroDisplay {
			get { 
				if (_result == null)
					return string.Empty;
				return _result.DetectedHero == null ? "None" : _result.DetectedHero.Name; 
			}
		}

		public string NewLocationDisplay {
			get {
				if (_result == null)
					return string.Empty;
				return _pendingGameState.Locations.SingleOrDefault (l => l.Id == _result.NewLocationId).Name;
			}
		}

		public ObservableCollection<SpawnLocationViewModel> Locations { get; set; }

		public RelayCommand AcceptCommand {
			get {
				return new RelayCommand (() => {
					//TODO: rethink this whole process. It would be nice to just set the game state to the edited one but that
					//breaks all bindings.
					//propagate all the changes to the game state
					Application.CurrentGame.Darkness = _pendingGameState.Darkness;
					Application.CurrentGame.MapCards = _pendingGameState.MapCards;
					Application.CurrentGame.Necromancer.LocationId = _pendingGameState.Necromancer.LocationId;
					Application.CurrentGame.Locations = _pendingGameState.Locations;
					Application.CurrentGame.BlightPool = _pendingGameState.BlightPool;
					//prophecy of doom is always reset after the necromancer phase
					Application.CurrentGame.Heroes.ProphecyOfDoomRoll = 0;

					//the state of these powers may have changed during the necromancer phase
					Application.CurrentGame.Heroes.InvisibleBarrierLocationId = _pendingGameState.Heroes.InvisibleBarrierLocationId; 
				
					Application.SaveCurrentGame ();

					MessagingCenter.Send<NecromancerActivationViewModel> (this, "NecromancerPhaseComplete");

					Application.Navigation.PopTo<HeroTurnPage> ();
				});
			}
		}
	}

	//TODO: Bring together with LocationViewModel

	public class SpawnLocationViewModel
	{
		public Location Location { get; private set; }

		public SpawnLocationViewModel (Location location, IEnumerable<Blight> blights)
		{
			Location = location;
			Spawns = new ObservableCollection<SpawnModelBase> ();

			if (blights != null) {
				foreach (var blight in blights) {
					Spawns.Add (new BlightViewModel (blight));
				}
			}
		}

		public ObservableCollection<SpawnModelBase> Spawns {get;set;}
	}
}

