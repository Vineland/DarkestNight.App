using System;
using Xamarin.Forms;
using System.Collections;
using Vineland.DarkestNight.UI.Utilities;
using Android.Nfc;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class BindablePicker<T> : Picker
	{
		public BindablePicker()
		{
			this.SelectedIndexChanged += OnSelectedIndexChanged;
		}

		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create<BindablePicker<T>, IList<T>>(o => o.ItemsSource, default(IList<T>), propertyChanged: OnItemsSourceChanged);

		public static BindableProperty SelectedItemProperty =
			BindableProperty.Create<BindablePicker<T>, T>(o => o.SelectedItem, default(T), 
				defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);


		public string DisplayMember { get; set; }

		public IList<T> ItemsSource
		{
			get { return (IList<T>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public T SelectedItem
		{
			get { return (T)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		private static void OnItemsSourceChanged(BindableObject bindable, IList<T> oldvalue, IList<T> newvalue)
		{
			var picker = bindable as BindablePicker<T>;

			if (picker != null)
			{
				picker.Items.Clear();
				if (newvalue == null) return;
				//now it works like "subscribe once" but you can improve
				foreach (var item in newvalue)
				{
					if (string.IsNullOrEmpty(picker.DisplayMember))
					{
						picker.Items.Add(item.ToString());
					}
					else
					{
						var type = item.GetType();

						var prop = type.GetProperty(picker.DisplayMember);


						//var value = 
						picker.Items.Add(prop.GetValue(item).ToString());
					}
				}
			}
		}

		private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
			{
				SelectedItem = default(T);
			}
			else
			{
				SelectedItem = ItemsSource[SelectedIndex];
			}
		}

		private static void OnSelectedItemChanged(BindableObject bindable, T oldvalue, T newvalue)
		{
			var picker = bindable as BindablePicker<T>;
			picker.SelectedIndex = picker.ItemsSource.IndexOf (newvalue);
		}
	}
}

