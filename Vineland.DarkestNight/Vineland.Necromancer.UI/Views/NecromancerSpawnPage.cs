using System;

using Xamarin.Forms;
using DLToolkit.Forms.Controls;
using System.Collections.Generic;
using Dalvik.SystemInterop;

namespace Vineland.Necromancer.UI
{
	public class NecromancerSpawnPage : ContentPage
	{
		public NecromancerSpawnPage ()
		{
			var nextButton = new ToolbarItem () { Text = "Accept" };
			nextButton.SetBinding<NecromancerSpawnViewModel> (ToolbarItem.CommandProperty, vm => vm.AcceptCommand);
			this.ToolbarItems.Add (nextButton);

			var flowListView = new FlowListView() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorVisibility = SeparatorVisibility.None,
				RowHeight=72,
				HasUnevenRows = true,
				GroupHeaderTemplate = new DataTemplate(typeof(LocationHeaderCell)),
				FlowColumnsTemplates = new List<FlowColumnTemplateSelector>() {
					new FlowColumnSimpleTemplateSelector(){ ViewType= typeof(BlightCell)},
					new FlowColumnSimpleTemplateSelector(){ ViewType= typeof(BlightCell)},
					new FlowColumnSimpleTemplateSelector(){ ViewType= typeof(BlightCell)},
					new FlowColumnSimpleTemplateSelector(){ ViewType= typeof(BlightCell)}
				},
				IsGroupingEnabled = true,
				FlowGroupKeySorting = FlowSorting.Ascending,
				FlowGroupGroupingKeySelector = new CustomGroupKeySelector(),
				FlowGroupItemSortingKeySelector = new CustomItemSortingKeySelector()
			};

			flowListView.SetBinding<NecromancerSpawnViewModel>(FlowListView.FlowItemsSourceProperty, v => v.ProspectiveSpawns);
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
			var item = e.Item as SpawnViewModel;
			if (item != null){
				LogHelper.Info(string.Format("FlowListView tapped: {0}", item.Blight.Name));
				var answer = await DisplayAlert("Confirm", "Are you sure you want to remove this blight?", "Yes", "No");
				if (answer) {
					//(BindingContext as NecromancerSpawnViewModel).RemoveBlight (item);
				}
			}
		}
	}
}


