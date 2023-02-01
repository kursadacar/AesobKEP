using System.Net;

namespace ActiveUp.Net.Dns
{
	internal class WksRecord : IRecordData
	{
		private IPAddress ipAddress;

		private byte protocol;

		private byte[] services;

		public IPAddress IpAddress
		{
			get
			{
				return ipAddress;
			}
		}

		public byte Protocol
		{
			get
			{
				return protocol;
			}
		}

		public byte[] Services
		{
			get
			{
				return services;
			}
		}

		public WksRecord(DataBuffer buffer, int length)
		{
			ipAddress = buffer.ReadIPAddress();
			protocol = buffer.ReadByte();
			services = new byte[length - 5];
			for (int i = 0; i < length - 5; i++)
			{
				services[i] = buffer.ReadByte();
			}
		}

		public override string ToString()
		{
			return string.Format("IP Address:{0} Protocol:{1} Services:{2}", ipAddress, protocol, services);
		}
	}
}
