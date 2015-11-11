using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Vineland.DarkestNight.UI.Shared.Services;

namespace Vineland.DarkestNight.UI.Shared.ViewModels
{
    public class HomeViewModel
    {
        FileService _fileService;
        FileInfo _lastestSave;

        public HomeViewModel(FileService fileService)
        {
            _fileService = fileService;
        }

        public void Initialise()
        {
            _lastestSave = _fileService.SearchDirectory(AppConstants.SavesLocation, "*" + AppConstants.SaveFileExtension)
                .OrderByDescending(x => x.CreationTime)
                .FirstOrDefault();
        }

        public bool CanContinue
        {
            get { return _lastestSave != null; }
        }

        public bool CanLoad
        {
            get { return _lastestSave != null; }
        }
    }
}
