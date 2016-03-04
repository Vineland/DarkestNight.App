using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Vineland.Necromancer.Core;
using Android.Bluetooth;
using System.Threading.Tasks;

namespace Vineland.Necromancer.UI
{
	public partial class HeroPhasePage : CustomCarouselPage
	{
		public HeroPhasePage ()
		{ 
			InitializeComponent ();
			Title = /*We could be */ "Heroes"; /*Just for one day*/
		}
	}
}

