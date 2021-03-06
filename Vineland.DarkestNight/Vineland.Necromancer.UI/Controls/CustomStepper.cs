﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Xml.Schema;

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

		public static readonly BindableProperty ValueProperty = 
			BindableProperty.Create ("Value", typeof(int), typeof(CustomStepper), -1, BindingMode.TwoWay
			, propertyChanging: (bindable, oldValue, newValue) => {
				var ctrl = (CustomStepper)bindable;

				ctrl.ValueLabel.Text = newValue.ToString ();
			
				var @value = newValue as int?;
				if (@value == null)
					return;

				if (@value == ctrl.Minimum && ctrl.DecrementImage.Source != MinusDisabled)
					ctrl.DecrementImage.Source = MinusDisabled;
					else if (ctrl.DecrementImage.Source != Minus && ctrl.IsEnabled)
					ctrl.DecrementImage.Source = Minus;

				if (@value == ctrl.Maximum && ctrl.IncrementImage.Source != PlusDisabled)
					ctrl.IncrementImage.Source = PlusDisabled;
					else if (ctrl.IncrementImage.Source != Plus && ctrl.IsEnabled)
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
			this.PropertyChanged += CustomStepper_PropertyChanged;
			//var layout = new RelativeLayout ();// { Orientation = StackOrientation.Horizontal, Spacing = 10 };
			var layout = new AbsoluteLayout ();

			DecrementImage = new Image () { Source = Minus };
			ValueLabel = new Label () {
				FontFamily = "hobo",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};
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

			//layout.Children.Add (DecrementImage, xConstraint: Constraint.Constant(0));
			//layout.Children.Add (ValueLabel, xConstraint:Constraint.RelativeToParent((parent) => parent.Width / 3));
			//layout.Children.Add (IncrementImage, xConstraint:Constraint.RelativeToParent((parent) => (parent.Width / 3) * 2));
			layout.Children.Add (DecrementImage, new Rectangle (0, 0, 32, 32));
			layout.Children.Add (ValueLabel, new Rectangle (32, 0, 32, 32));
			layout.Children.Add (IncrementImage, new Rectangle (64, 0, 32, 32));
			Content = layout;
		}

		void CustomStepper_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsEnabled") {
				IncrementImage.IsEnabled = IsEnabled;
				DecrementImage.IsEnabled = IsEnabled;

				if (!IsEnabled) {
					IncrementImage.Source = PlusDisabled;
					DecrementImage.Source = MinusDisabled;
				} else {
					if(Value < Maximum)
						IncrementImage.Source = Plus;

					if(Value > Minimum)
						DecrementImage.Source = Minus;
				}
			}
		}
	}
}

