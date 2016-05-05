using System;
using UIKit;
using CoreGraphics;
using Foundation;
using WebServerScannerCore;

namespace WebServerScannerIOS
{
	public class AddressTableViewSource : UITableViewSource
	{
		AddressScanner _radar;

		public event EventHandler TableViewSourceChanged;

		public AddressTableViewSource ()
		{
			_radar = new AddressScanner ();
			_radar.NewEndpointFound += TriggerTableViewSourceChangedEvent;
		}

		public void RefreshTableViewSource()
		{
			_radar.Scan ();
		}

		private void TriggerTableViewSourceChangedEvent(object sender, EventArgs args)
		{
			TableViewSourceChanged (this, EventArgs.Empty);
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return _radar.Addresses.Count;
		}
			
		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var row = _radar.Addresses [indexPath.Row];
			var cell = new UITableViewCell (CGRect.Empty);
			cell.TextLabel.Text = row;
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var url = _radar.Addresses[(int)indexPath.Item]; // NOTE: https secure request
			UIApplication.SharedApplication.OpenUrl(new NSUrl("http://" + url));
		}
	}
}

