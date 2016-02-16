using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class CustomStepper : ContentView
	{
		public static ImageSource Minus = ImageSource.FromFile("minus");
		public static ImageSource MinusDisabled = ImageSource.FromFile("minus_disabled");

		public static ImageSource Plus = ImageSource.FromFile("plus");
		public static ImageSource PlusDisabled = ImageSource.FromFile("plus_disabled");

		public int Minimum { get; set;}
		public int Maximum {get;set;}

		public static readonly BindableProperty ValueProperty = BindableProperty.Create<CustomStepper, int>(x=>x.Value, -1, BindingMode.TwoWay
			, propertyChanging: (bindable, oldValue, newValue) => {
				var ctrl = (CustomStepper)bindable;
				//ctrl.Value = newValue;
				ctrl.ValueLabel.Text = newValue.ToString ();

				if (newValue == ctrl.Minimum && ctrl.DecrementButton.Source != MinusDisabled)
					ctrl.DecrementButton.Source = MinusDisabled;
				else if(ctrl.DecrementButton.Source != Minus)
					ctrl.DecrementButton.Source = Minus;

				if(newValue == ctrl.Maximum && ctrl.IncrementButton.Source != PlusDisabled)
					ctrl.IncrementButton.Source = PlusDisabled;
				else if(ctrl.DecrementButton.Source != Plus)
					ctrl.IncrementButton.Source = Plus;
			});
		
		public int Value {
			get { return (int)GetValue (ValueProperty); }
			set { 
				SetValue (ValueProperty, value); 
			}
		}

		public CustomStepper ()
		{
			Maximum = int.MaxValue;

			InitializeComponent ();

			//DecrementButton.IsEnabled = true;
			DecrementButton.GestureRecognizers.Add(new TapGestureRecognizer((view)=> { DecrementButton_Clicked();}));
			IncrementButton.GestureRecognizers.Add(new TapGestureRecognizer((view)=> { IncrementButton_Clicked();}));

			Value = Value;
		}

		void IncrementButton_Clicked ()
		{
			if (Value < Maximum) 
				Value = Value + 1;
		}

		void DecrementButton_Clicked ()
		{
			if(Value > Minimum)
				Value = Value - 1;
		}
	}
}

