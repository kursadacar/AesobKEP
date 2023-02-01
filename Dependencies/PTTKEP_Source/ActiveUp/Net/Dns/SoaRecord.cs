namespace ActiveUp.Net.Dns
{
	internal class SoaRecord : IRecordData
	{
		private string primaryNameServer;

		private string responsibleMailAddress;

		private int serial;

		private int refresh;

		private int retry;

		private int expire;

		private int defaultTtl;

		public string PrimaryNameServer
		{
			get
			{
				return primaryNameServer;
			}
		}

		public string ResponsibleMailAddress
		{
			get
			{
				return responsibleMailAddress;
			}
		}

		public int Serial
		{
			get
			{
				return serial;
			}
		}

		public int Refresh
		{
			get
			{
				return refresh;
			}
		}

		public int Retry
		{
			get
			{
				return retry;
			}
		}

		public int Expire
		{
			get
			{
				return expire;
			}
		}

		public int DefaultTtl
		{
			get
			{
				return defaultTtl;
			}
		}

		public SoaRecord(DataBuffer buffer)
		{
			primaryNameServer = buffer.ReadDomainName();
			responsibleMailAddress = buffer.ReadDomainName();
			serial = buffer.ReadInt();
			refresh = buffer.ReadInt();
			retry = buffer.ReadInt();
			expire = buffer.ReadInt();
			defaultTtl = buffer.ReadInt();
		}

		public override string ToString()
		{
			return string.Format("Primary Name Server:{0} Responsible Name Address:{1} Serial:{2} Refresh:{3} Retry:{4} Expire:{5} Default TTL:{6}", primaryNameServer, responsibleMailAddress, serial, refresh, retry, expire, defaultTtl);
		}
	}
}
