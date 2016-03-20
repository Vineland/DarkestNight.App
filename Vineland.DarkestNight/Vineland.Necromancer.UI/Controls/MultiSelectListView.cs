using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Vineland.Necromancer.UI
{
	public class MultiSelectListView<T> : ListView
	{
		public static BindableProperty SelectedItemsProperty = 
			BindableProperty.Create("SelectedItems", typeof(ObservableCollection<T>), typeof(MultiSelectListView<T>), null);

		public ObservableCollection<T> SelectedItems
		{
			get{ return (ObservableCollection<T>)GetValue (SelectedItemsProperty); }
			set{ SetValue (SelectedItemsProperty, value); }
		}

		public MultiSelectListView ()
		{
			this.ItemSelected += MultiSelectListView_ItemSelected;
		}

		void MultiSelectListView_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;
			
			if (SelectedItems.Contains ((T)e.SelectedItem))
				SelectedItems.Remove ((T)e.SelectedItem);
			else
				SelectedItems.Add ((T)e.SelectedItem);

			SelectedItem = null;
		}
	}
}


