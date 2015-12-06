using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vineland.DarkestNight.Core;
using Vineland.DarkestNight.UI;
using Vineland.DarkestNight.UI.Services;

namespace Vineland.DarkestNight.UI
{
	public class AppGameState : GameState
	{
		FileService _fileService;

		public AppGameState(FileService fileService)
		{
			_fileService = fileService;
		}

		public void Save()
		{
			var path = Path.Combine(AppConstants.SavesLocation, this.CreatedDate.GetHashCode () + AppConstants.SaveFileExtension);
			_fileService.SaveFile(path, JsonConvert.SerializeObject(this));
		}

		public void Load(FileInfo save)
		{
			var gameState = JsonConvert.DeserializeObject<GameState>(_fileService.LoadFile(save));

				this.DarknessLevel = gameState.DarknessLevel;
				this.Heroes = gameState.Heroes;
				this.Locations = gameState.Locations;
				this.Mode = gameState.Mode;
				this.Necromancer = gameState.Necromancer;
				this.PallOfSuffering = gameState.PallOfSuffering;
				this.CreatedDate = gameState.CreatedDate;
		
		}

	}
}

