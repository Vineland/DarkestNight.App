using System;

using Xamarin.Forms;
using Android.Hardware.Camera2;
using Android.Locations;
using DLToolkit.Forms.Controls;
using System.Collections.Generic;

namespace Vineland.Necromancer.UI
{
	public class BlightLocationsPage : ContentPage
	{
		public BlightLocationsPage ()
		{
			Title = "Blights";
			BackgroundImage = "background";
//		}
//
//		protected override void OnBindingContextChanged ()
//		{
//			var viewModel = BindingContext as BlightLocationsViewModel;
//
			var flowListView = new FlowListView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorVisibility = SeparatorVisibility.None,
				//GroupDisplayBinding = new Binding("Location.Name"),
				//HasUnevenRows=false,
				RowHeight=72,
				HasUnevenRows = true,
				GroupHeaderTemplate = new DataTemplate(typeof(LocationHeaderCell)),
				FlowColumnsTemplates = new List<FlowColumnTemplateSelector>() {
					new BlightAddTemplateSelector(),
					new BlightAddTemplateSelector(),
					new BlightAddTemplateSelector(),
					new BlightAddTemplateSelector(),
				},
				IsGroupingEnabled = true,
				FlowGroupKeySorting = FlowSorting.Ascending,
				FlowGroupGroupingKeySelector = new CustomGroupKeySelector(),
				FlowGroupItemSortingKeySelector = new CustomItemSortingKeySelector()
			};

			flowListView.SetBinding<BlightLocationsViewModel>(FlowListView.FlowItemsSourceProperty, v => v.Blights);
			flowListView.FlowItemTapped += FlowListView_FlowItemTapped;

			Content = new StackLayout() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					flowListView,
				}
			};
		}

		async void FlowListView_FlowItemTapped (object sender, ItemTappedEventArgs e)
		{
			var item = e.Item as BlightViewModel;
			if (item != null){
				LogHelper.Info(string.Format("FlowListView tapped: {0}", item.Blight.Name));
				var answer = await DisplayAlert("Confirm", "Are you sure you want to remove this blight?", "Yes", "No");
				if (answer) {
					(BindingContext as BlightLocationsViewModel).RemoveBlight (item);
				}
			}
		}


	}

	public class BlightAddTemplateSelector : FlowColumnTemplateSelector{

		public override Type GetColumnType (object bindingContext)
		{
			var blightModel = (BlightViewModel)bindingContext;

			if (blightModel == null || blightModel.Blight == null)
				return typeof(ContentView);

			return typeof(BlightCell);
		}
	}

	public class CustomGroupKeySelector : FlowPropertySelector
	{
		public override object GetProperty(object bindingContext)
		{
			var blightModel = (BlightViewModel)bindingContext;

			return blightModel.Location.Name;
		}
	}

	public class CustomItemSortingKeySelector : FlowPropertySelector
	{
		public override object GetProperty(object bindingContext)
		{
			var flowItem = (BlightViewModel)bindingContext;
			if (flowItem.Blight == null)
				return string.Empty;
			
			return flowItem.Blight.Name;
		}
	}
}


