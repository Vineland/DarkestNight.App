using System;
using Xamarin.Forms;
using System.Linq;

namespace Vineland.Necromancer.UI
{
	public class CustomCarouselPage : CarouselPage
	{
		#region Declarations

		public static readonly BindableProperty IsPagerVisibleProperty =
			BindableProperty.Create("IsPagerVisible", typeof(bool), typeof(CustomCarouselPage), true);

		public static readonly BindableProperty PagerItemColorProperty =
			BindableProperty.Create("PagerItemColor", typeof(Color), typeof(CustomCarouselPage), Color.FromHex("#BC666666"));

		public static readonly BindableProperty PagerMinimumWidthProperty =
			BindableProperty.Create("PagerMinimumWidth", typeof(double), typeof(CustomCarouselPage), 0d);

		public static readonly BindableProperty PagerPaddingProperty =
			BindableProperty.Create("PagerPadding", typeof(Thickness), typeof(CustomCarouselPage), new Thickness(0, 0, 0, 20));

		public static readonly BindableProperty PagerXAlignProperty =
			BindableProperty.Create("PagerXAlign", typeof(TextAlignment), typeof(CustomCarouselPage), TextAlignment.Center);

		public static readonly BindableProperty PagerYAlignProperty =
			BindableProperty.Create ("PagerYAlign", typeof(TextAlignment), typeof(CustomCarouselPage), TextAlignment.End);

		public static readonly BindableProperty SelectedPagerItemColorProperty =
			BindableProperty.Create("SelectedPagerItemColor", typeof(Color), typeof(CustomCarouselPage), Color.FromHex("#BC63402d"));

		#endregion

		#region Public Properties

		public bool IsPagerVisible
		{
			get { return (bool)GetValue(IsPagerVisibleProperty); }
			set { SetValue(IsPagerVisibleProperty, value); }
		}

		public Color PagerItemColor
		{
			get { return (Color)GetValue(PagerItemColorProperty); }
			set { SetValue(PagerItemColorProperty, value); }
		}

		public double PagerMinimumWidth
		{
			get { return (double)GetValue(PagerMinimumWidthProperty); }
			internal set { SetValue(PagerMinimumWidthProperty, value); }
		}

		public Thickness PagerPadding
		{
			get { return (Thickness)base.GetValue(PagerPaddingProperty); }
			set { base.SetValue(PagerPaddingProperty, value); }
		}

		public TextAlignment PagerXAlign
		{
			get { return (TextAlignment)base.GetValue(PagerXAlignProperty); }
			set { base.SetValue(PagerXAlignProperty, value); }
		}

		public TextAlignment PagerYAlign
		{
			get { return (TextAlignment)base.GetValue(PagerYAlignProperty); }
			set { base.SetValue(PagerYAlignProperty, value); }
		}

		public Color SelectedPagerItemColor
		{
			get { return (Color)GetValue(SelectedPagerItemColorProperty); }
			set { SetValue(SelectedPagerItemColorProperty, value); }
		}

		#endregion

		protected override void OnPropertyChanged (string propertyName)
		{
			base.OnPropertyChanged (propertyName);
			if (propertyName == "ItemsSource") {
				var count = 0;
				foreach (var item in ItemsSource) {
					count++;
				}
				IsPagerVisible = count > 1;
			}
		}
	}
}

