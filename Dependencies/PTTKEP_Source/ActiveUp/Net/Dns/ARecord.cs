using System.Net;

namespace ActiveUp.Net.Dns
{
	internal class ARecord : IRecordData
	{
		private IPAddress ipAddress;

		public IPAddress IpAddress
		{
			get
			{
				return ipAddress;
			}
		}

		public ARecord(DataBuffer buffer)
		{
			byte[] address = buffer.ReadBytes(4);
			ipAddress = new IPAddress(address);
		}

		public override string ToString()
		{
			return "IP Address: " + ipAddress.ToString();
		}
	}
}
