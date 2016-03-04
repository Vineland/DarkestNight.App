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
		BlightService _blightService;

		public NecromancerSpawnViewModel (BlightService blightService)
		{
			_blightService = blightService;
			ProspectiveSpawns = new ObservableCollection<SpawnViewModel> ();
		}

		int _blightsToSpawnAtNewLocation;
		int _blightsToSpawnAtMonastery;

		public void Initialise (int blightsToSpawnAtNewLocation, int blightsToSpawnAtMonastery)
		{
			Task.Run (() => {
				_blightsToSpawnAtNewLocation = blightsToSpawnAtNewLocation;
				_blightsToSpawnAtMonastery = blightsToSpawnAtMonastery;

				GetAllProspectiveBlights ();
			});
		}

		public ObservableCollection<SpawnViewModel> ProspectiveSpawns { get; set; }

		public Location NecromancerLocation {
			get {
				return Application.CurrentGame.Locations.Single (l => l.Id == Application.CurrentGame.Necromancer.LocationId);
			}
		}

		private void GetAllProspectiveBlights ()
		{
			//if this ain't our first rodeo, put everything back - we gonna can take another crack
			foreach (var pb in ProspectiveSpawns.Reverse()) {				
				Application.CurrentGame.BlightPool.Add (pb.Blight);
				pb.Cards.Reverse ();
				pb.Cards.ForEach (c => Application.CurrentGame.MapCards.Return (c));
			}
			ProspectiveSpawns.Clear ();

			for (int i = 0; i < _blightsToSpawnAtNewLocation; i++) {
				//we've gone over so spawn the blight at the monastery instead
				if (NecromancerLocation.BlightCount + ProspectiveSpawns.Sum (x => x.Location.Id == LocationIds.Monastery ? 0 : x.Blight.Weight) >= 4) {
					ProspectiveSpawns.Add (GetProspectiveSpawn (LocationIds.Monastery));
				} else {
					ProspectiveSpawns.Add (GetProspectiveSpawn (NecromancerLocation.Id));
				}
			}

			for (int i = 0; i < _blightsToSpawnAtMonastery; i++) {
				ProspectiveSpawns.Add (GetProspectiveSpawn (LocationIds.Monastery));
			}
		}

		private SpawnViewModel GetProspectiveSpawn (int locationId)
		{
			var prospectiveSpawn = new SpawnViewModel () {
				Location = Application.CurrentGame.Locations.Single (l => l.Id == locationId)
			};
			do {
				var card = Application.CurrentGame.MapCards.Draw ();
				var blightName = card.Rows.Single (x => x.LocationId == locationId).BlightName;

				prospectiveSpawn.Blight = Application.CurrentGame.BlightPool.FirstOrDefault (b => b.Name == blightName);
				prospectiveSpawn.Cards.Add (card);

			} while(prospectiveSpawn.Blight == null);

			Application.CurrentGame.BlightPool.Remove (prospectiveSpawn.Blight);

			return prospectiveSpawn;
		}

		public void DestroyBlightCommand (SpawnViewModel prospectiveBlight)
		{
			//if a blight is instantly destroyed we need to:
			// 1) discard the cards used to spawn it
			prospectiveBlight.Cards.ForEach (c => Application.CurrentGame.MapCards.Discard (c));
			prospectiveBlight.Cards.Clear ();
			// 2) decrement the required number of blights to spawn
			if (prospectiveBlight.Location.Id != LocationIds.Monastery)
				_blightsToSpawnAtNewLocation--;
			else
				_blightsToSpawnAtMonastery--;
					
			// 3) get the prospective blights again (in case this removal has changed spill over blights to the monastery)
			GetAllProspectiveBlights ();					
		}

		public RelayCommand AcceptCommand {
			get {
				return new RelayCommand (() => 
					{
						foreach(var spawn in ProspectiveSpawns)
							spawn.Location.Blights.Add(spawn.Blight);

						Application.Navigation.PopTo<HeroPhasePage>();
				});
			}
		}
	}

	public class SpawnViewModel : BlightViewModel
	{
		/// <summary>
		/// Multiple cards could have be drawn if none of a particular blight are in the pool
		/// </summary>
		/// <value>The card.</value>
		public List<MapCard> Cards { get; set; }

		public SpawnViewModel ()
		{
			Cards = new List<MapCard> ();
		}
	}
}

