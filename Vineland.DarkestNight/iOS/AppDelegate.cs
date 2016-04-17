using System;
using System.Collections.Generic;
using System.Linq;
using Vineland.Necromancer.Core.Services;
using Vineland.Necromancer.UI;

using Foundation;
using UIKit;

namespace Vineland.Necromancer.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			//Insights.Initialize("0b86252c1fff1cce0924366fd12f82f6c55f29aa", ApplicationContext);
			//#endif

			try
			{
				LogHelper.Info("Bootstrapper");
				var bootstrapper = new iOSBootstrapper();
				bootstrapper.Run();

				LogHelper.Info("Initialising Forms");
				global::Xamarin.Forms.Forms.Init ();

				LogHelper.Info("Loading  NecromancerApp");
				LoadApplication (new NecromancerApp ());

//				foreach (NSString family in UIFont.FamilyNames)
//				{
//					foreach (NSString font in UIFont.FontNamesForFamilyName(family))
//					{
//						Console.WriteLine(@"{0}", font);
//					}
//				}
				return base.FinishedLaunching (app, options);
			}
			catch (Exception ex)
			{
				LogHelper.Error(ex);
				throw;
			}
		}
	}
}

