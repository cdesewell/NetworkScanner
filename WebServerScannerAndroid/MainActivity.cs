using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;

namespace WebServerScannerAndroid
{
	[Activity (Label = "WebServerScanner Android", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		AddressArrayAdapter _adapter;
		ListView _addressListView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			_addressListView = FindViewById<ListView> (Resource.Id.addressList);
			_adapter = new AddressArrayAdapter (this);
			_addressListView.Adapter = _adapter;
			_addressListView.ItemClick += LaunchBrowser;

			var button = FindViewById<Button> (Resource.Id.refreshButton);
			button.Click += RefreshListView;
		}
	
		private void RefreshListView(object sender, EventArgs args)
		{
			_adapter.RefreshListViewArray ();
		}

		private void LaunchBrowser(object sender, AdapterView.ItemClickEventArgs args)
		{
			var address = _adapter.GetItem (args.Position);
			var uri = Android.Net.Uri.Parse ("http://" + address);
			var intent = new Intent (Intent.ActionView, uri);
			StartActivity (intent);
		}
	}
}


