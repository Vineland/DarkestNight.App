using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Nfc;
using Android.Views;
using Android.Widget;
using Vineland.Necromancer.Core;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

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

			if (e.OldElement == null) 
			{
				var items = e.NewElement.ItemsSource as IEnumerable<T>;
				Control.ChoiceMode = Android.Widget.ChoiceMode.Multiple;
				Control.Adapter = new MultiSelectListViewAdapter<T> (this.Control.Context, items.ToList());
				Control.Divider = new ColorDrawable (Android.Graphics.Color.ParseColor("#BC63402D"));
				Control.DividerHeight = 1;
			}
		}
	}

	public class MultiSelectListViewAdapter<T>:ArrayAdapter<T>{	

		public MultiSelectListViewAdapter (Context context, IList<T> items)
			:base(context, Android.Resource.Layout.SimpleListItemMultipleChoice, items)
		{

		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, ViewGroup parent)
		{
			var view = (TextView)base.GetView (position, convertView, parent);
			view.Typeface = FontManager.GetDefaultFont();

			return view;
		}
	}
}

