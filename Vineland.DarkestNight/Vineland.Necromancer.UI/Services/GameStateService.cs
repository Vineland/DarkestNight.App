using System;
using Vineland.DarkestNight.UI.Services;
using System.IO;
using Vineland.DarkestNight.UI;
using Newtonsoft.Json;
using Vineland.Necromancer.Core;
using Android.Util;
using System.Linq;
using Java.Lang;
using System.Threading.Tasks;
using Org.Apache.Http.Impl.Client;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class GameStateService
	{
		FileService _fileService;
		DataService _dataService;

		public GameState CurrentGame { get; private set; }

		public GameStateService (FileService fileService,
		                         DataService dataService)
		{
			_fileService = fileService;
			_dataService = dataService;

			if (_fileService.DoesFileExist (AppConstants.SaveFilePath))
				Continue ();
		}

		public void NewGame (Settings appSettings)
		{			
			CurrentGame = new GameState ();
			CurrentGame.CreatedDate = DateTime.Now;
			CurrentGame.Darkness = appSettings.StartingDarkness;
			CurrentGame.PallOfSuffering = appSettings.PallOfSuffering;
			CurrentGame.Mode = appSettings.DarknessCardsMode;

			CurrentGame.Necromancer.LocationId = LocationIds.Ruins;

			LogHelper.Info ("Setting up Map deck");
			CurrentGame.MapDeck = _dataService.GetMapCards (appSettings);
			CurrentGame.MapDiscard = new List<MapCard> ();

			CurrentGame.Locations = new List<Location> (Location.All);

			var startingMapCard = DrawMapCard ();

			CurrentGame.Locations.ForEach (x => {
				x.Blights = new List<Blight> ();
				if (x.Id == LocationIds.Monastery) {
					x.NumberOfBlights = 0;
				} else {
					x.NumberOfBlights = 1;
					var blightName = startingMapCard.Rows.Single (r => r.Location == x.Name).Blight;
					LogHelper.Info (string.Format ("Adding {0} blight to {1}", blightName, x.Name));
					x.Blights.Add (_dataService.GetBlight (blightName));
				}
			});
		}

		public async void Save ()
		{
			await Task.Run (() => {
				_fileService.SaveFile (AppConstants.SaveFilePath, JsonConvert.SerializeObject (CurrentGame));
			});
		}

		public void Continue ()
		{
			CurrentGame = JsonConvert.DeserializeObject<GameState> (_fileService.LoadFile (AppConstants.SaveFilePath));
		}

		public MapCard DrawMapCard ()
		{
			if (!CurrentGame.MapDeck.Any ()) {
				LogHelper.Info ("Shuffling discard and forming new Map deck");
				CurrentGame.MapDiscard.Shuffle ();

				CurrentGame.MapDeck = CurrentGame.MapDiscard;

				CurrentGame.MapDiscard = new List<MapCard> ();
			}

			var mapCard = CurrentGame.MapDeck.First ();

			CurrentGame.MapDeck.Remove (mapCard);
			CurrentGame.MapDiscard.Add (mapCard);

			return mapCard;
		}
	}
}

