namespace ActiveUp.Net.Dns
{
	internal class SrvRecord : IRecordData
	{
		private int priority;

		private ushort weight;

		private ushort port;

		private string domain;

		public int Priority
		{
			get
			{
				return priority;
			}
		}

		public ushort Weight
		{
			get
			{
				return weight;
			}
		}

		public ushort Port
		{
			get
			{
				return port;
			}
		}

		public string Domain
		{
			get
			{
				return domain;
			}
		}

		public SrvRecord(DataBuffer buffer)
		{
			priority = buffer.ReadShortInt();
			weight = buffer.ReadShortUInt();
			port = buffer.ReadShortUInt();
			domain = buffer.ReadDomainName();
		}

		public override string ToString()
		{
			return string.Format("Priority:{0} Weight:{1}  Port:{2} Domain:{3}", priority, weight, port, domain);
		}
	}
}
