using System;
using Android.Views;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LocationBlightsCell), typeof(HeroPhaseLocationCellRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class HeroPhaseLocationCellRenderer : ViewCellRenderer
	{
		public HeroPhaseLocationCellRenderer()
		{
			
		}
		protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Android.Content.Context context)
		{
			var locationCell = (LocationBlightsCell)item;
			var view = base.GetCellCore(item, convertView, parent, context) as BaseCellView;
			view.Drag += Handle_Drag;
			return view;
		}

		void Handle_Drag(object sender, Android.Views.View.DragEventArgs e)
		{
			var view = sender as BaseCellView;
			switch (e.Event.Action)
			{
				case DragAction.Started:
					e.Handled = true;
					break;
				case DragAction.Entered:
					view.SetBackgroundColor(Android.Graphics.Color.Red);
					break;
				case DragAction.Exited:
					view.SetBackgroundColor(Android.Graphics.Color.Transparent);
					break;
				case DragAction.Drop:
					
					e.Handled = true;
					var data = e.Event.ClipData.GetItemAt(0).Text;

					view.SetBackgroundColor(Android.Graphics.Color.Transparent);

					break;
				case DragAction.Ended:

					view.SetBackgroundColor(Android.Graphics.Color.Transparent);
					e.Handled = true;
					break;
			}
		}
	}
}
