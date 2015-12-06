
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Vineland.DarkestNight.Core;
using Vineland.DarkestNight.UI.Shared.ViewModels;
using Vineland.DarkestNight.UI.Shared;

namespace Vineland.DarkestNight.UI.Android
{
	[Activity (Label = "ChooseHeroesActivity")]			
	public class ChooseHeroesActivity : Activity
	{
		public ChooseHeroesViewModel ViewModel {get;set;}
		public ChooseHeroesActivity ()
		{
			IoC.BuildUp(this);
		}
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			ViewModel.Initialise();

			SetContentView(Resource.Layout.ChooseHeroes);

			var listView = FindViewById<ListView>(Resource.Id.HeroesListView);
			listView.ChoiceMode = ChoiceMode.Single;
			listView.Adapter = new HeroListAdapter(this, ViewModel.AllHeroes.ToArray());
			listView.ItemClick += ListView_ItemClick;
		}

		void ListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			var hero = ViewModel.AllHeroes[e.Position];
			var checkbox = e.View.FindViewById<CheckBox>(Resource.Id.CheckBox);
			if(checkbox.Checked)
			{
				ViewModel.RemoveHero(hero.Id);
				checkbox.Checked = false;
			}else if(ViewModel.CanAddHero)
			{
				ViewModel.AddHero(hero);
				checkbox.Checked = true;
			}
		}
	}

	public class HeroListAdapter : BaseAdapter<Hero>
	{
		Hero[] items;
		Activity context;

		public HeroListAdapter(Activity context, Hero[] items) : base() {
			this.context = context;
			this.items = items;
		}
		public override long GetItemId(int position)
		{
			return position;
		}

		public override Hero this[int position] {  
			get { return items[position]; }
		}
		public override int Count {
			get { return items.Length; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Android.Resource.Layout.HeroListItem, null);
			
			view.FindViewById<TextView>(Android.Resource.Id.Name).Text = items[position].Name;

			return view;
		}
	}
}

