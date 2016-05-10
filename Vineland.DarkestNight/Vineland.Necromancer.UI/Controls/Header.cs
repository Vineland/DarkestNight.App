using System;
using Xamarin.Forms;
using Vineland.DarkestNight.UI;

namespace Vineland.Necromancer.UI
{
	public class Header : ContentView
	{
		public Label Label { get; private set; }

		public Header ()
		{
			BackgroundColor = AppConstants.HeaderBackground;
			Padding = new Thickness (16, 8);
			Content = Label = new Label () { Text = Text, TextColor = AppConstants.HeaderTextColour };
		}

		public static readonly BindableProperty TextProperty = 
			BindableProperty.Create ("Text", typeof(string), typeof(Header), string.Empty, 
				propertyChanged:
				(bindable, oldValue, newValue) => {
					var ctrl = (Header)bindable;
					ctrl.Label.Text = newValue.ToString();
				});

		public string Text {
			get { return (string)GetValue (TextProperty); }
			set { 
				SetValue (TextProperty, value); 
			}
		}
	}
}

