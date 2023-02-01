namespace ActiveUp.Net.Dns
{
	internal class MGRecord : DomainNameOnly
	{
		public string MailGroupDomain
		{
			get
			{
				return base.Domain;
			}
		}

		public MGRecord(DataBuffer buffer)
			: base(buffer)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
