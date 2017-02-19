using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class BindablePicker<T> : Picker
	{
		public BindablePicker ()
		{
			this.SelectedIndexChanged += OnSelectedIndexChanged;
		}

		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create ("ItemsSource", typeof(IList<T>), typeof(BindablePicker<T>), default(IList<T>), propertyChanged: OnItemsSourceChanged);

		public static BindableProperty SelectedItemProperty =
			BindableProperty.Create ("SelectedItem", typeof(T), typeof(BindablePicker<T>), default(T), 
				defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);



		public bool IncludeEmptyOption { get; set; }

		public string EmptyOptionDisplay = "None";

		public string DisplayMember { get; set; }

		public IList<T> ItemsSource {
			get { return (IList<T>)GetValue (ItemsSourceProperty); }
			set { SetValue (ItemsSourceProperty, value); }
		}

		public T SelectedItem {
			get { return (T)GetValue (SelectedItemProperty); }
			set { SetValue (SelectedItemProperty, value); }
		}

		private static void OnItemsSourceChanged (BindableObject bindable, object oldValue, object newValue)
		{
			var picker = bindable as BindablePicker<T>;

			if (picker != null) {
				picker.Items.Clear ();
				var items = newValue as IList<T>;

				if (newValue == null)
					return;
				//for now it works like "subscribe once" but can improve
				if (picker.IncludeEmptyOption)
					picker.Items.Add (picker.EmptyOptionDisplay);
				
				foreach (var item in items) {
					if (string.IsNullOrEmpty (picker.DisplayMember)) {
						picker.Items.Add (item.ToString ());
					} else {
						var type = item.GetType ();

						var prop = type.GetProperty (picker.DisplayMember);


						//var value = 
						picker.Items.Add (prop.GetValue (item).ToString ());
					}
				}
			}
		}

		private void OnSelectedIndexChanged (object sender, EventArgs eventArgs)
		{
			if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1) {
				SelectedItem = default(T);
			} else {
				if (IncludeEmptyOption) {
					if (SelectedIndex == 0)
						SelectedItem = default(T);
					else
						SelectedItem = ItemsSource [SelectedIndex - 1];
				} else
					SelectedItem = ItemsSource [SelectedIndex];
			}
		}

		private static void OnSelectedItemChanged (BindableObject bindable, object oldvalue, object newvalue)
		{
			var picker = bindable as BindablePicker<T>;

			foreach(var item in picker.ItemsSource) {
				if (item.Equals(newvalue)) 
				{
					if (picker.IncludeEmptyOption)
						picker.SelectedIndex = picker.ItemsSource.IndexOf (item) + 1;
					else
						picker.SelectedIndex = picker.ItemsSource.IndexOf (item);
					
					break;
				}
			}	

		}
	}
}

