using System;
using Xamarin.Forms;
using Android.Views.InputMethods;

namespace Vineland.Necromancer.UI
{
	public class BlightCell : ContentView
	{

		public BlightCell ()
		{
			
		}

		protected override void OnBindingContextChanged ()
		{
			var viewModel = BindingContext as BlightViewModel;

			var image = string.Format ("blight_{0}", viewModel.Blight.Name.Replace (" ", "_").ToLower());

			Content = new Image () { Source = ImageSource.FromFile (image) };
		}
	}
}

