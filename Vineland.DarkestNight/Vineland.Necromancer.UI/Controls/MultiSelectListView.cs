﻿using System;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Vineland.Necromancer.UI
{
	public class MultiSelectListView<T> : ListView
	{
		//public int Maximum { get; set;}

		public static BindableProperty SelectedItemsProperty = BindableProperty.Create<MultiSelectListView<T>, ObservableCollection<T>>(x=>x.SelectedItems, null);

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
			else //if (SelectedItems.Count != Maximum)
				SelectedItems.Add ((T)e.SelectedItem);

			SelectedItem = null;
		}
	}
}


