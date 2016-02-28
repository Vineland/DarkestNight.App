using System;
using Android.Nfc;

namespace Vineland.Necromancer.UI
{
	public class SelectListItem<T>
	{
		public string Name {get;set;}
		public T Value {get;set;}
	}
}

