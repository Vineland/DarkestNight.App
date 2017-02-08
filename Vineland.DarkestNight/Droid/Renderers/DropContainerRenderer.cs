using System;
using Android.Views;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DropContainer), typeof(DropContainerRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class DropContainerRenderer : VisualElementRenderer<StackLayout>
	{
		public DropContainerRenderer()
		{
			
		}
		protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
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

		void Handle_Drag(object sender, Android.Views.View.DragEventArgs e)
		{
			e.Handled = false;
			switch (e.Event.Action)
			{
				case DragAction.Started:
					e.Handled = true;
					break;
				case DragAction.Entered:
					this.SetBackgroundColor(Android.Graphics.Color.Red);
					break;
				case DragAction.Exited:
					this.SetBackgroundColor(Android.Graphics.Color.Transparent);
					break;
				case DragAction.Drop:
					
					//e.Handled = true;
					var data = e.Event.ClipData.GetItemAt(0).Text;

					this.SetBackgroundColor(Android.Graphics.Color.Transparent);

					break;
				case DragAction.Ended:

					this.SetBackgroundColor(Android.Graphics.Color.Transparent);
					//e.Handled = true;
					break;
			}
		}
	}
}
