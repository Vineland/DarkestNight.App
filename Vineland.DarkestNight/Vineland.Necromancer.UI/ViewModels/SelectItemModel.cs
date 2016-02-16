using System;
using Xamarin.Forms;
using Android.Views.InputMethods;

namespace Vineland.Necromancer.UI
{
	public class SelectItemViewModel<T> where T:class
	{
		public T Model { get; set; }
		public string Display { get { return Model.ToString ();} }
		public bool IsSelected {get;set;}
	}
}


