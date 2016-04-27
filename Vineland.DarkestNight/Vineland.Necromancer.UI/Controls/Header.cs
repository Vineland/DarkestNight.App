using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class Header : ContentView
	{
		public Label Label { get; private set; }

		public Header ()
		{
			BackgroundColor = Color.FromHex ("#BC63402D");
			Padding = new Thickness (16, 8);
			Content = Label = new Label () { Text = Text, TextColor = Color.White };
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

