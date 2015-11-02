using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vineland.DarkestNight.Core.Services
{
    public class SaveGameService
    {
        public SaveGameService()
        {

        }

        //public void CreateNewGame() { }


        public List<FileInfo> GetSaveGames()
        {
            var paths = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "*.sav", SearchOption.TopDirectoryOnly);

            var saveGames = new List<FileInfo>();
            
            foreach (var path in paths)
            {
                saveGames.Add(new FileInfo(path));
            }

            return saveGames;
        }

        public GameState LoadGame(FileInfo save)
        {
            using (var streamReader = new StreamReader(save.FullName))
            {
                return JsonConvert.DeserializeObject<GameState>(streamReader.ReadToEnd());
            }
        }

        public void SaveGame(GameState game)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(folderPath, game.Name + ".sav");
            using (var streamWriter = new StreamWriter(path))
            {
                streamWriter.Write(JsonConvert.SerializeObject(game));
            }
        }

        public void DeleteGame(FileInfo save)
        {
            save.Delete();
        }
    }
}
