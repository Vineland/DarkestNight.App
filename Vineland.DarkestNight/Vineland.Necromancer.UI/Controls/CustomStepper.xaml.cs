using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public partial class CustomStepper : ContentView
	{
		public int Minimum { get; set;}
		public int Maximum {get;set;}

		public static readonly BindableProperty ValueProperty = BindableProperty.Create<CustomStepper, int>(x=>x.Value, -1, BindingMode.TwoWay
			, propertyChanging: (bindable, oldValue, newValue) => {
				var ctrl = (CustomStepper)bindable;
				ctrl.Value = newValue;
			});
		
		public int Value {
			get { return (int)GetValue (ValueProperty); }
			set { 
				SetValue (ValueProperty, value); 
				ValueLabel.Text = value.ToString ();

				DecrementButton.IsEnabled = value != Minimum;
				IncrementButton.IsEnabled = value != Maximum;
			}
		}

		public CustomStepper ()
		{
			Maximum = int.MaxValue;

			InitializeComponent ();

			DecrementButton.Clicked += DecrementButton_Clicked;
			IncrementButton.Clicked += IncrementButton_Clicked;
		}

		void IncrementButton_Clicked (object sender, EventArgs e)
		{
			if (Value < Maximum) 
				Value = Value + 1;
		}

		void DecrementButton_Clicked (object sender, EventArgs e)
		{
			if(Value > Minimum)
				Value = Value - 1;
		}
	}
}

