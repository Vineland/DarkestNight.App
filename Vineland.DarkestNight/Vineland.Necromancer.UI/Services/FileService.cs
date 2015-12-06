using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vineland.DarkestNight.UI.Services
{
    public class FileService
    {
        public FileService()
        {

        }

		public void SaveFile(string path, string @value)
		{
			using (var streamWriter = new StreamWriter(path))
			{
				streamWriter.Write(@value);
			}
		}

		public string LoadFile(FileInfo fileInfo)
		{
			using (var streamReader = new StreamReader(fileInfo.FullName)) {
				return streamReader.ReadToEnd();
			}
		}

        public List<FileInfo> SearchDirectory(string path, string searchPattern, SearchOption option = SearchOption.AllDirectories)
        {
			if(!Directory.Exists(path))
				return new List<FileInfo>();
			
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
