using System;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class LocationPicker :BindablePicker<Location>
	{
		public LocationPicker()
		{
			DisplayMember = "Name";
		}
	}
}
