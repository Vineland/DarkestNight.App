using System;
using Android.App;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Vineland.Necromancer.UI.Droid
{
	public class DisplayAlertService : IDisplayAlertService
	{
		#region IDisplayAlertService implementation

		public void DisplayAlert (string title, string message, string cancel)
		{
			var alert = new AlertDialog.Builder(Forms.Context);
			alert.SetTitle(title);
			alert.SetMessage(message);
			alert.SetNegativeButton(cancel, (sender, e) => { });

			var dialog = alert.Show();
			BrandAlertDialog(dialog);
		}

		public static void BrandAlertDialog(AlertDialog dialog)
		{
	//		try
//			{
//				Resources resources = dialog.Context.Resources;
//				var color = dialog.Context.Resources.GetColor(Resource.Color.accent);
//
//				var alertTitleId = resources.GetIdentifier("alertTitle", "id", "android");
//				var alertTitle = (TextView)dialog.Window.DecorView.FindViewById(alertTitleId);
//				alertTitle.SetTextColor(color); // change title text color
//
//				var titleDividerId = resources.GetIdentifier("titleDivider", "id", "android");
//				var titleDivider = dialog.Window.DecorView.FindViewById(titleDividerId);
//				titleDivider.SetBackgroundColor(color); // change divider color
//			}
//			catch
//			{
//				//Can't change dialog brand color
//			}
		}

		public async Task<bool> DisplayConfirmation (string title, string message, string accept, string cancel)
		{
			bool? result = null;

			var alert = new AlertDialog.Builder(Forms.Context);
			alert.SetTitle(title);
			alert.SetMessage(message);
			alert.SetNegativeButton(cancel, (sender, e) => { result = false;});
			alert.SetPositiveButton (accept, (sender, e) => {
				result = true;
			});
			alert.Show ();

			await Task.Run(()=> 
				{
				while (!result.HasValue);
			});

			return result.Value;
		}

		public string DisplayActionSheet (string title, string cancel, string destruction, params string[] buttons)
		{
			return string.Empty;
		}

		#endregion
	}
}

