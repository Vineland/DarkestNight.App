using System;
using System.Collections.Generic;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.Domain;
using Xamarin.Forms;
using System.Linq;

namespace Vineland.Necromancer.UI
{
	public class SpawnBlightPopupViewModel :BaseViewModel
	{
		public Action<SpawnBlightOption> OnOptionSelected;

		public SpawnBlightPopupViewModel()
		{
			Options = new List<SpawnBlightOption>();
			Options.Add(new SpawnBlightOption(null));
			var blights = Application.CurrentGame.BlightPool.GroupBy(x => x.Name).Select(x => x.First());
			foreach (var blight in blights.OrderBy(x => x.Name))
				Options.Add(new SpawnBlightOption(blight));
		}

		public List<SpawnBlightOption> Options { get; private set; }

		SpawnBlightOption _selectedOption;
		public SpawnBlightOption SelectedOption
		{
			get { return _selectedOption; }
			set
			{
				if (_selectedOption != value)
				{
					_selectedOption = value;
					OnOptionSelected(value);
					Application.Navigation.PopLastPopup();
				}
			}
		}
	}

	public class SpawnBlightOption
	{
		Blight _blight;

		public SpawnBlightOption(Blight blight)
		{
			_blight = blight;
		}

		public string Description
		{
			get
			{
				if (_blight == null)
					return "Random";

				return _blight.Name;
			}
		}

		public ImageSource Image
		{
			get
			{
				if (_blight != null)
					return ImageSourceUtil.GetBlightImage(_blight.Name);

				return ImageSource.FromFile("quest");
			}
		}
	}
}
