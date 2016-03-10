using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using Android.Views.InputMethods;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.Database;
using System.Runtime.InteropServices;
using Android.Util;

namespace Vineland.Necromancer.UI
{
	public class BlightLocationsViewModel : BaseViewModel
	{
		BlightService _blightService;

		public BlightLocationsViewModel (BlightService blightService)
		{
			_blightService = blightService;
			var models = Application.CurrentGame.Locations.Select (l => new BlightLocationViewModel (l));
			LocationSections = new ObservableCollection<BlightLocationViewModel> (models);
		}

		public ObservableCollection<BlightLocationViewModel> LocationSections { get; set; }

		BlightRowViewModel _selectedRow;

		public BlightRowViewModel SelectedRow {
			get { return _selectedRow; }
			set {
				if (value != null) {
					if (value.IsPlaceholder)
						SpawnBlight (value);
					else
						DestoryBlight (value);
				}
				_selectedRow = null;
				RaisePropertyChanged (() => SelectedRow);
			}
		}

		public void SpawnBlight (BlightRowViewModel spawnRow)
		{
			var locationSection = LocationSections.Single (l => l.Contains (spawnRow));

			var blight = _blightService.SpawnBlight (locationSection.Location, Application.CurrentGame).Item2;

			locationSection.Insert (locationSection.Count - 1, new BlightRowViewModel (blight));
			if (locationSection.Location.BlightCount >= 4)
				locationSection.Remove (spawnRow);

			Task.Run (() => {
				Application.SaveCurrentGame ();
			});
		}

		public void DestoryBlight (BlightRowViewModel selectedRow)
		{
			
			var locationSection = LocationSections.Single (l => l.Contains (selectedRow));
			locationSection.Remove (selectedRow);
			if (locationSection.Count < 4
			    && !locationSection.Any (x => x.IsPlaceholder))
				locationSection.Add (new BlightRowViewModel (null));

			Task.Run (() => {				
				_blightService.DestroyBlight (locationSection.Location, selectedRow.Blight, Application.CurrentGame);
				Application.SaveCurrentGame ();
			});
		}
	}

	public class BlightLocationViewModel : ObservableCollection<BlightRowViewModel>
	{
		public Location Location { get; private set; }

		public BlightLocationViewModel (Location location)
		{
			Location = location;

			foreach (var blight in location.Blights) {
				this.Add (new BlightRowViewModel (blight));
			}

			if (location.BlightCount < 4)
				this.Add (new BlightRowViewModel (null));
			
		}
	}

	public class BlightRowViewModel : BaseViewModel
	{
		public Blight Blight{ get ; private set; }

		public BlightRowViewModel (Blight blight)
		{
			Blight = blight;
		}

		public string Name {
			get{ return Blight == null ? "Spawn Blight" : Blight.Name; }
		}

		public string ImageName {
			get { return Blight == null ? "plus" : string.Format ("blight_{0}", Blight.Name.ToLower ().Replace (" ", "_")); }
		}

		public bool IsPlaceholder { get { return Blight == null; } }
	}
}

