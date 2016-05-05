using System;
using Android.Widget;
using WebServerScannerCore;
using Android.Views;
using Android.App;


namespace WebServerScannerAndroid
{
	public class AddressArrayAdapter : BaseAdapter
	{
		AddressScanner _radar;
		Activity _activity;

		public AddressArrayAdapter (Activity activity)
		{
			_activity = activity;
			_radar = new AddressScanner ();
			_radar.NewEndpointFound += TriggerListArrayChangedEvent;
		}

		public override int Count {
			get { return _radar.Addresses.Count; }
		}

		public override Java.Lang.Object GetItem (int position) {
			return _radar.Addresses[position];
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var addressView = _activity.LayoutInflater.Inflate (Resource.Layout.AddressListItem,parent,false);
			var textView = addressView.FindViewById<TextView> (Resource.Id.addressText);
			textView.Text = _radar.Addresses [position];
			return addressView;
		}

		public void RefreshListViewArray()
		{
			_radar.Scan ();
		}

		private void TriggerListArrayChangedEvent(object sender, EventArgs args)
		{
			this.NotifyDataSetChanged ();
		}
			
	}
}

