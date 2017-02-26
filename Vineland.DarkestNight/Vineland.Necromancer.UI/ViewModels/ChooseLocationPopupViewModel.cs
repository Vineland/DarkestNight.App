using System;
using System.Collections.Generic;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class ChooseLocationPopupViewModel :BaseViewModel
	{
		public Action<Location> OnLocationSelected;

		public ChooseLocationPopupViewModel()
		{
		}

		public List<Location> Locations
		{
			get { return Application.CurrentGame.Locations; }
		}

		Location _selectedLocation;
		public Location SelectedLocation
		{
			get { return _selectedLocation; }
			set
			{
				if (_selectedLocation != value)
				{
					_selectedLocation = value;
					OnLocationSelected(value);
				}
			}
		}
	}
}
