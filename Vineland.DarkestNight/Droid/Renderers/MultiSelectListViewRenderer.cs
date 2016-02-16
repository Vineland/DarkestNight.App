using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Widget;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;
using Vineland.Necromancer.Core;
using Android.Nfc;
using Android.Views;
using Android.Graphics;
using System.Collections;
using System.Collections.Generic;
using Android.Content;
using System.Linq;
using Android.App;
using System.Security.Cryptography;
using System.Threading.Tasks;

[assembly: ExportRenderer (typeof(MultiSelectListView<Hero>), typeof(MultiSelectListViewRenderer<Hero>))]
namespace Vineland.Necromancer.UI.Droid
{
		public class MultiSelectListViewRenderer<T> : ListViewRenderer
{
			public MultiSelectListViewRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.ListView> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
			}

			if (e.NewElement != null) {
				var items = e.NewElement.ItemsSource as IEnumerable<T>;
				Control.ChoiceMode = Android.Widget.ChoiceMode.Multiple;
				Control.Adapter = new MultiSelectListViewAdapter<T> (this.Control.Context, items.ToList());
			}
		}
	}

	public class MultiSelectListViewAdapter<T>:ArrayAdapter<T>{	

		Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "baskerville_becker.ttf");

		public MultiSelectListViewAdapter (Context context, IList<T> items)
			:base(context, Android.Resource.Layout.SimpleListItemMultipleChoice, items)
		{

		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, ViewGroup parent)
		{
			var view = (TextView)base.GetView (position, convertView, parent);
			view.Typeface = font;

			return view;
			//convertView.
		}
	}
}

