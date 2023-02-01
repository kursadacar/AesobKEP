using System.Net;

namespace ActiveUp.Net.Dns
{
	internal class AAAARecord : IRecordData
	{
		private IPAddress ipAddress;

		public IPAddress IpAddress
		{
			get
			{
				return ipAddress;
			}
		}

		public AAAARecord(DataBuffer buffer)
		{
			ipAddress = buffer.ReadIPv6Address();
		}

		public override string ToString()
		{
			return "IP Address: " + ipAddress.ToString();
		}
	}
}
