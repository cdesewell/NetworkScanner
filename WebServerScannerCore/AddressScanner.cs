using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace WebServerScannerCore
{
	public class AddressScanner
	{
		public List<string> Addresses {
			get;
			private set;
		}

		int scanCount;
		public event EventHandler NewEndpointFound;

		public AddressScanner ()
		{
			Addresses = new List<string>();
		}

		public void Scan()
		{
			if (scanCount < 255 & scanCount > 0) {
				return;
			} else {
				Addresses.Clear ();
				NewEndpointFound(this,EventArgs.Empty);
			}

			scanCount = 0;
			var gateway = "192.168.0.1";//GetGatewayAddress();
			int port = 80;
			var addressPrefix = gateway.Split('.');

			for (int i = 2; i <= 255; i++)
			{
				string address = addressPrefix[0] + "." + addressPrefix[1] + "." + addressPrefix[2] + "." + i;
				Task.Run(() => AttemptConnection(address, port));
			}				
		}

		private async Task AttemptConnection(string address,int port)
		{
			TcpClient tcpClient = new TcpClient();
			try
			{
				tcpClient.SendTimeout = 1;
				await tcpClient.ConnectAsync(address, port);

				Addresses.Add(address);
				NewEndpointFound(this,EventArgs.Empty);
				scanCount += 1;

			}
			catch (Exception)
			{
				scanCount += 1;
			}

		}

		private string GetGatewayAddress()
		{
			foreach (var router in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (router.OperationalStatus == OperationalStatus.Up)
				{
					var addresses = router.GetIPProperties().GatewayAddresses.Select(a => a.Address).ToList();
					foreach (var address in addresses)
					{
						return address.ToString();
					}
				}
			}
			return null;
		}
 
	}
}