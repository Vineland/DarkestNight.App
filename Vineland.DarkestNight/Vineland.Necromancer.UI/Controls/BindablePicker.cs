using System;
using Xamarin.Forms;
using System.Collections;
using Vineland.DarkestNight.UI.Utilities;
using Android.Nfc;
using System.Collections.Generic;
using Android.Views.InputMethods;
using System.Linq;

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



		public bool IncludeEmptyOption { get; set; }
		public string EmptyOptionDisplay = "None";

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
				if (newvalue == null) 
					return;
				//for now it works like "subscribe once" but can improve
				if (picker.IncludeEmptyOption)
					picker.Items.Add (picker.EmptyOptionDisplay);
				
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
				if(IncludeEmptyOption)
				{
					if (SelectedIndex == 0)
						SelectedItem = default(T);
					else
						SelectedItem = ItemsSource [SelectedIndex - 1];
				}else
					SelectedItem = ItemsSource[SelectedIndex];
			}
		}

		private static void OnSelectedItemChanged(BindableObject bindable, T oldvalue, T newvalue)
		{
			var picker = bindable as BindablePicker<T>;

			if(picker.IncludeEmptyOption)
				picker.SelectedIndex = picker.ItemsSource.IndexOf (newvalue) + 1;
			else
				picker.SelectedIndex = picker.ItemsSource.IndexOf (newvalue);
		}
	}

	/// <summary>
	/// Picker for binding to selected value.
	/// T: class of item
	/// S: class of selected value
	/// </summary>
	public class BindablePicker<T, S> : Picker
	{
		public BindablePicker()
		{
			this.SelectedIndexChanged += OnSelectedIndexChanged;
		}

		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create<BindablePicker<T,S>, IList<T>>(o => o.ItemsSource, default(IList<T>), propertyChanged: OnItemsSourceChanged);

		public static BindableProperty SelectedValueProperty =
			BindableProperty.Create<BindablePicker<T,S>, S>(o => o.SelectedValue, default(S), 
				defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnSelectedValueChanged);


		public bool IncludeEmptyOption { get; set; }
		public string EmptyOptionDisplay = "None";

		public string DisplayMember { get; set; }
		public string ValueMember { get; set; }

		public IList<T> ItemsSource
		{
			get { return (IList<T>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public S SelectedValue
		{
			get { return (S)GetValue(SelectedValueProperty); }
			set { SetValue(SelectedValueProperty, value); }
		}

		private static void OnItemsSourceChanged(BindableObject bindable, IList<T> oldvalue, IList<T> newvalue)
		{
			var picker = bindable as BindablePicker<T,S>;

			if (picker != null)
			{
				picker.Items.Clear();
				if (newvalue == null) 
					return;
				//for now it works like "subscribe once" but can improve
				if (picker.IncludeEmptyOption)
					picker.Items.Add (picker.EmptyOptionDisplay);

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
			if (SelectedIndex < 0 
				|| SelectedIndex > Items.Count - 1
				|| (IncludeEmptyOption && SelectedIndex == 0))
			{
				SelectedValue = default(S);
			}
			else
			{
				T item = IncludeEmptyOption ? ItemsSource [SelectedIndex - 1] : ItemsSource [SelectedIndex];

				var type = item.GetType();

				var prop = type.GetProperty(ValueMember);

				SelectedValue = (S)prop.GetValue (item);
			}
		}

		private static void OnSelectedValueChanged(BindableObject bindable, S oldvalue, S newvalue)
		{
			var picker = bindable as BindablePicker<T,S>;
			foreach (var item in picker.ItemsSource) {

				var type = item.GetType();

				var prop = type.GetProperty(picker.ValueMember);

				if (newvalue.Equals(prop.GetValue (item))) 
				{
					var index = picker.ItemsSource.IndexOf (item);
					if (picker.IncludeEmptyOption)
						index++;

					picker.SelectedIndex = index;
					return;
				}
			}

			picker.SelectedIndex = 0;
		}
	}
}

