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

		public event EventHandler AddressSourceChanged;

		public AddressTableViewSource ()
		{
			_radar = new AddressScanner ();
			_radar.NewEndpointFound += AddressListChanged;
		}

		public void RefreshTableViewSource()
		{
			_radar.Scan ();
		}

		private void AddressListChanged(object sender, EventArgs args)
		{
			AddressSourceChanged (this, EventArgs.Empty);
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

