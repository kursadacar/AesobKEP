namespace ActiveUp.Net.Dns
{
	internal class MBRecord : DomainNameOnly
	{
		public string AdminMailboxDomain
		{
			get
			{
				return base.Domain;
			}
		}

		public MBRecord(DataBuffer buffer)
			: base(buffer)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
