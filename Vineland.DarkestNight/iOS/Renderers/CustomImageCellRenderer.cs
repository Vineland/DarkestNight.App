using System;
using Vineland.Necromancer.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(ImageCell), typeof(CustomImageCellRenderer))]
namespace Vineland.Necromancer.iOS
{
	public class CustomImageCellRenderer: ImageCellRenderer
	{
		public CustomImageCellRenderer ()
		{
		}

		public override UIKit.UITableViewCell GetCell (Cell item, UIKit.UITableViewCell reusableCell, UIKit.UITableView tv)
		{
			var cell = base.GetCell (item, reusableCell, tv);
			cell.SeparatorInset = UIEdgeInsets.Zero;
			cell.TextLabel.Font = UIFont.FromName ("baskervillebecker", 18f);
			return cell;
		}
	}
}
