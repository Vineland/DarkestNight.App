using System;
using Xamarin.Forms;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.App;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Content;

[assembly: ExportRenderer (typeof(CustomImage), typeof(CustomImageRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class CustomImageRenderer : ImageRenderer
	{
		private CustomImageGestureListener _listener;
		private GestureDetector _detector;

		public CustomImageRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
			{
					this.GenericMotion -= HandleGenericMotion;
					this.Touch -= HandleTouch;

			}

			if (e.OldElement == null)
			{

				_listener = new CustomImageGestureListener(Element as CustomImage);
				_detector = new GestureDetector(_listener);
				//this.GenericMotion += HandleGenericMotion;
				//this.Touch += HandleTouch;
				this.LongClick += (sender, args) =>
				{
					ClipData data = ClipData.NewPlainText("Image", "whut");
					this.StartDrag(data, new DragShadowBuilder(this), null, 0);
				};
			}
		}

		void HandleTouch(object sender, TouchEventArgs e)
		{
			_detector.OnTouchEvent(e.Event);
		}

		void HandleGenericMotion(object sender, GenericMotionEventArgs e)
		{
			_detector.OnTouchEvent(e.Event);
		}
	}

	public class CustomImageGestureListener : GestureDetector.SimpleOnGestureListener
	{
		CustomImage _image;

		public CustomImageGestureListener(CustomImage image)
		{
			_image = image;
		}

		public override void OnLongPress(MotionEvent e)
		{
			if(_image.OnLongPress != null)
				_image.OnLongPress.Execute(null);
			base.OnLongPress(e);
		}

		public override bool OnSingleTapUp(MotionEvent e)
		{
			if(_image.OnTap != null)
				_image.OnTap.Execute(null);
			return base.OnSingleTapUp(e);
		}
	}
}