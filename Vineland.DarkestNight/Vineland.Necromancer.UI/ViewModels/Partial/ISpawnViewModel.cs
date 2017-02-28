using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public interface ISpawnViewModel
	{
		ImageSource Image { get; }
		bool IsPlaceHolder { get; }
	}
}

