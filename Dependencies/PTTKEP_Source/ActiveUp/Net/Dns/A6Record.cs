using System.Net;

namespace ActiveUp.Net.Dns
{
	internal class A6Record : IRecordData
	{
		private int prefixLength = -1;

		private IPAddress ipAddress;

		private string domain;

		public int PrefixLength
		{
			get
			{
				return prefixLength;
			}
		}

		public IPAddress IpAddress
		{
			get
			{
				return ipAddress;
			}
		}

		public string Domain
		{
			get
			{
				return domain;
			}
		}

		public A6Record(DataBuffer buffer)
		{
			prefixLength = buffer.ReadByte();
			if (prefixLength == 0)
			{
				ipAddress = buffer.ReadIPv6Address();
				return;
			}
			if (prefixLength == 128)
			{
				domain = buffer.ReadDomainName();
				return;
			}
			ipAddress = buffer.ReadIPv6Address();
			domain = buffer.ReadDomainName();
		}

		public override string ToString()
		{
			return string.Format("Prefix Length:{0} IP Address:{1} Domain:{2}", prefixLength, ipAddress, domain);
		}
	}
}
