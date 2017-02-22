using System;
using Vineland.Necromancer.Domain;

namespace Vineland.Necromancer.UI
{
	public class DifficultyLevelPicker :BindablePicker<DifficultyLevel>
	{
		public DifficultyLevelPicker()
			:base()
		{
			DisplayMember = "Name";
		}
	}
}
