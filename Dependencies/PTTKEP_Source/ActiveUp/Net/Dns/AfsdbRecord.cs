namespace ActiveUp.Net.Dns
{
	internal class AfsdbRecord : IRecordData
	{
		private short subType;

		private string domain;

		public short SubType
		{
			get
			{
				return subType;
			}
		}

		public string Domain
		{
			get
			{
				return domain;
			}
		}

		public AfsdbRecord(DataBuffer buffer)
		{
			subType = buffer.ReadShortInt();
			domain = buffer.ReadDomainName();
		}

		public override string ToString()
		{
			return string.Format("SubType:{0} Domain:{1}", subType, domain);
		}
	}
}
