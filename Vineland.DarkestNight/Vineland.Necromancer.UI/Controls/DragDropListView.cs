using System;
using Xamarin.Forms;

namespace Vineland.Necromancer.UI
{
	public class DragDropListView : ListView
	{
		public DragDropListView()
			:base(ListViewCachingStrategy.RecycleElement)
		{
		}
	}
}
