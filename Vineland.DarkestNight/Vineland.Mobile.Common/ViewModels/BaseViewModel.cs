

namespace Vineland.Xamarin.Forms.Common
{
	public abstract class BaseViewModel
	{
		public BaseViewModel ()
		{
		}

		public virtual void OnAppearing() { }

		public virtual void OnDisappearing() { }

		public virtual void OnBackButtonPressed() { }

		public virtual void Cleanup() { }
	}
}

