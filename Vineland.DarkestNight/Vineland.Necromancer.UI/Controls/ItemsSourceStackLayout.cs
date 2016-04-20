using System;
using Xamarin.Forms;
using System.Collections;
using System.Collections.ObjectModel;
using Vineland.Necromancer.Core;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Vineland.Necromancer.UI
{
	public class ItemsSourceStackLayout:StackLayout
	{
		public ItemsSourceStackLayout ()
		{
		}


		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create ("ItemsSource", typeof(IList), typeof(ItemsSourceStackLayout), default(IList),
				propertyChanged: (bindableObject, oldValue, newValue) => {
					((ItemsSourceStackLayout)bindableObject).ItemsSourceChanged ();
				}
			);

		public static BindableProperty ItemSelectedProperty = BindableProperty.Create("ItemSelected", typeof(ICommand), typeof(ItemsSourceStackLayout)
			, null,
			propertyChanged: (bindable, oldValue, newValue) => {
				((ItemsSourceStackLayout)bindable).SetTapGestureRecognizers();
			});

		public IList ItemsSource {
			get {
				return (IList)GetValue (ItemsSourceProperty);
			}
			set {
				SetValue (ItemsSourceProperty, value);
			}
		}

		public ICommand ItemSelected {
			get {
				return (ICommand)GetValue (ItemSelectedProperty);
			}
			set {
				SetValue (ItemSelectedProperty, value);
			}
		}

		public DataTemplate ItemTemplate {get;set;}

	 	void ItemsSourceChanged ()
		{
			this.Children.Clear ();
			if (ItemsSource == null)
				return;
			
			if(ItemsSource is INotifyCollectionChanged)
				(ItemsSource as INotifyCollectionChanged).CollectionChanged += ItemsSource_CollectionChanged;
			
			foreach (var item in ItemsSource) {
				var view = (View)ItemTemplate.CreateContent ();
				var bindableObject = view as BindableObject;
				if (bindableObject != null)
					bindableObject.BindingContext = item;
					
				this.Children.Add (view);
			}
			SetTapGestureRecognizers ();
		}

	 	void ItemsSource_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
	 	{
			this.Children.Clear ();
			//TODO: use the notifycollectionchangedeventargs
			foreach (var item in ItemsSource) {
				var view = (View)ItemTemplate.CreateContent ();
				var bindableObject = view as BindableObject;
				if (bindableObject != null)
					bindableObject.BindingContext = item;
				this.Children.Add (view);
			}

			SetTapGestureRecognizers ();
	 	}

		void SetTapGestureRecognizers(){
			foreach (var view in this.Children) {
				if (ItemSelected == null)
					view.GestureRecognizers.Clear ();
				else
					view.GestureRecognizers.Add (new TapGestureRecognizer () {
						Command = ItemSelected,
							CommandParameter =  view.BindingContext
					});
			}
		}
	}
}

