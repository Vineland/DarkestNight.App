using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Widget;
using Vineland.Necromancer.UI;
using Vineland.Necromancer.UI.Droid;
using Vineland.DarkestNight.Core;
using Android.Nfc;
using Android.Views;
using System.Collections;
using System.Collections.Generic;
using Android.Content;
using System.Linq;
using Android.App;

[assembly: ExportRenderer (typeof(MultiSelectListView<Hero>), typeof(HeroMultiSelectListViewRenderer))]
namespace Vineland.Necromancer.UI.Droid
{
	public class HeroMultiSelectListViewRenderer : ListViewRenderer
	{
		public HeroMultiSelectListViewRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.ListView> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
			}

			if (e.NewElement != null) {
				// subscribe
				//Control.Adapter = new NativeAndroidListViewAdapter (Forms.Context as Android.App.Activity, e.NewElement as NativeListView);
				//Control.ItemClick += OnItemClick;
				var items = e.NewElement.ItemsSource as IEnumerable<Hero>;
				Control.ChoiceMode = Android.Widget.ChoiceMode.Multiple;
				Control.Adapter = new ArrayAdapter<Hero> (Control.Context, Android.Resource.Layout.SimpleListItemMultipleChoice, items.ToArray ());

				var selectedItems = (e.NewElement as MultiSelectListView<Hero>).SelectedItems;
				int i = 0;
				foreach (var item in items) {
					Control.SetItemChecked (i, selectedItems.Contains (item));
					i++;
				}
			}
		}
	}

	public class HeroListViewAdapter:BaseAdapter<Hero>{		
		List<Hero> _heroes;
		public HeroListViewAdapter (List<Hero> heroes)
		{
			_heroes = heroes;
		}
		public override Hero this [int index] {
			get {
				return _heroes [index];
			}
		}
		public override int Count {
			get {

				return _heroes.Count;
			}
		}

		public override long GetItemId (int position)
		{
			return _heroes[position].Id;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, ViewGroup parent)
		{
			if (convertView == null) {
				var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
				convertView = inflater.Inflate (Android.Resource.Layout.SimpleListItemMultipleChoice, null);
			}

			return convertView;
			//convertView.
		}
	}
}

