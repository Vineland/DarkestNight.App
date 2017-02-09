using System;
namespace Vineland.Necromancer.Core
{
	public interface IFileService
	{
		bool DoesFileExist(string path);

		void SaveFile(string path, string @value);

		string LoadFile(string fullPath);
	}
}
