using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vineland.DarkestNight.Core.Services
{
    public class FileService
    {
        public FileService()
        {

        }

        public List<FileInfo> SearchDirectory(string path, string searchPattern, SearchOption option = SearchOption.AllDirectories)
        {
            var paths = Directory.GetFiles(path, searchPattern, option);

            var saveGames = new List<FileInfo>();
            
            foreach (var p in paths)
            {
                saveGames.Add(new FileInfo(p));
            }

            return saveGames;
        }

        public void Delete(FileInfo save)
        {
            save.Delete();
        }
    }
}
