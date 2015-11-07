using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Vineland.DarkestNight.Core;
using Vineland.DarkestNight.UI.Shared.Services;

namespace Vineland.DarkestNight.UI.Shared.ViewModels
{
    public class NewGameViewModel
    {
        AppSettings _appSettings;
        GameState _gameState;

        public NewGameViewModel(AppSettings appSettings, GameState gameState)
        {
            _appSettings = appSettings;
            _gameState = gameState;
            DarknessCardsModeOptions = (DarknessCardsMode[])Enum.GetValues(typeof(DarknessCardsMode));
        }
        
        public void Initialise()
        {
            _gameState.DarknessLevel = _appSettings.StartingDarkness;
            _gameState.PallOfSuffering = _appSettings.PallOfSuffering;
            _gameState.Mode = _appSettings.DarknessCardsMode;
        }

        public DarknessCardsMode[] DarknessCardsModeOptions { get; private set; }

        public int DarknessLevel
        {
            get { return _gameState.DarknessLevel; }
            set { _gameState.DarknessLevel = value; }
        }

        public bool PallOfSuffering
        {
            get { return _gameState.PallOfSuffering; }
            set { _gameState.PallOfSuffering = value; }
        }

        public DarknessCardsMode Mode
        {
            get { return _gameState.Mode; }
            set { _gameState.Mode = value; }
        }

        public string Name
        {
            get { return _gameState.Name;}
            set { _gameState.Name = value; }
        }

        public void RemoveHero(Hero hero)
        {
            _gameState.Heroes.Active.Remove(hero);
        }

        public void StartGame()
        {
            _gameState.Save();
        }
    }
}
