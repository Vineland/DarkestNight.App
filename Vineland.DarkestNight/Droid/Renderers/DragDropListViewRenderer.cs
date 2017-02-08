using System;
using Android.Graphics;
using Android.Views;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Linq;

[assembly: ExportRenderer(typeof(DragDropListView), typeof(DragDropListViewRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class DragDropListViewRenderer : ListViewRenderer
	{
		public DragDropListViewRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement == null)
			{
				this.Drag -= Handle_Drag;
			}

			if (e.OldElement == null)
			{
				this.Drag += Handle_Drag;
			}
		}

		void Handle_Drag(object sender, DragEventArgs e)
		{
			switch (e.Event.Action)
			{
				case DragAction.Location:
					e.Handled = true;
					var dragY = e.Event.GetY();
					var rect = new Rect();
					Control.GetGlobalVisibleRect(rect);

					Console.WriteLine("DragY: " + dragY);

					if(dragY <= 100)
						Control.SmoothScrollBy(-30,50);//SmoothScrollBy(-30, 0);

					if (dragY >= Control.Height - 150)
						Control.SmoothScrollBy(30, 50);
					//if (Control.Top < (Control.ScrollY + 100))
					//	Control.SmoothScrollBy(-30, 0);
					//this.SetBackgroundColor(Android.Graphics.Color.Transparent);
					break;
			}
		}
	}
}
