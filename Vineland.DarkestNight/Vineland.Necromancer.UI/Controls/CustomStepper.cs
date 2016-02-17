using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class CustomStepper : ContentView
	{
		public static ImageSource Minus = ImageSource.FromFile ("minus");
		public static ImageSource MinusDisabled = ImageSource.FromFile ("minus_disabled");
	
		public static ImageSource Plus = ImageSource.FromFile ("plus");
		public static ImageSource PlusDisabled = ImageSource.FromFile ("plus_disabled");

		public int Minimum { get; set; }

		public int Maximum { get; set; }

		public static readonly BindableProperty ValueProperty = BindableProperty.Create<CustomStepper, int> (x => x.Value, -1, BindingMode.TwoWay
			, propertyChanging: (bindable, oldValue, newValue) => {
			var ctrl = (CustomStepper)bindable;

			ctrl.ValueLabel.Text = newValue.ToString ();

			if (newValue == ctrl.Minimum && ctrl.DecrementImage.Source != MinusDisabled)
				ctrl.DecrementImage.Source = MinusDisabled;
			else if (ctrl.DecrementImage.Source != Minus)
				ctrl.DecrementImage.Source = Minus;

			if (newValue == ctrl.Maximum && ctrl.IncrementImage.Source != PlusDisabled)
				ctrl.IncrementImage.Source = PlusDisabled;
			else if (ctrl.IncrementImage.Source != Plus)
				ctrl.IncrementImage.Source = Plus;
		});


		public int Value {
			get { return (int)GetValue (ValueProperty); }
			set { 
				SetValue (ValueProperty, value); 
			}
		}

		public Image DecrementImage { get; private set; }

		public Image IncrementImage { get; private set; }

		public Label ValueLabel { get; private set; }

		public CustomStepper ()
		{
			Maximum = int.MaxValue;

			var layout = new StackLayout () { Orientation = StackOrientation.Horizontal, Spacing = 10 };
			DecrementImage = new Image () { Source = Minus };
			ValueLabel = new Label () { FontFamily = "hobo", VerticalOptions= LayoutOptions.Center};
			IncrementImage = new Image () { Source = Plus };

			DecrementImage.GestureRecognizers.Add (new TapGestureRecognizer () {
				Command = new Command (() => {
					if (Value > Minimum)
						Value--;
				})
			});

			IncrementImage.GestureRecognizers.Add (new TapGestureRecognizer () {
				Command = new Command (() => {
					if (Value < Maximum)
						Value++;
				})
			});

			layout.Children.Add (DecrementImage);
			layout.Children.Add (ValueLabel);
			layout.Children.Add (IncrementImage);

			Content = layout;
		}
	}
}

