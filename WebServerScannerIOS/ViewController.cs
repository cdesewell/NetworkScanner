using System;
using UIKit;
using System.Timers;
using System.Threading;
using CoreGraphics;

namespace WebServerScannerIOS
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		UITableView _addressTable;
		UIButton _refreshButton;
		AddressTableViewSource _dataSource;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_refreshButton = new UIButton (UIButtonType.System);
			_refreshButton.SetTitle("Refresh",UIControlState.Normal);
			_refreshButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
			_refreshButton.Frame = new CGRect (0, 20, this.View.Frame.Width, 30);

			_addressTable = new UITableView (new CGRect(0,50,this.View.Frame.Width,this.View.Frame.Height - 50));
			_dataSource = new AddressTableViewSource (); 
			_addressTable.Source = _dataSource;

			_refreshButton.TouchUpInside += RefreshDataSource;
			_dataSource.TableViewSourceChanged += UpdateTableView;
			this.Add (_refreshButton);
			this.Add (_addressTable);

		}

		private void RefreshDataSource(object sender, EventArgs args)
		{
			_dataSource.RefreshTableViewSource ();
		}

		private void UpdateTableView(object sender, EventArgs args)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(
				new Action(() => {
					_addressTable.ReloadData ();
				}));
		}
	}
}

