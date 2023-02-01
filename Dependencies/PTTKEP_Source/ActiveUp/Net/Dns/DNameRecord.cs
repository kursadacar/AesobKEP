namespace ActiveUp.Net.Dns
{
	internal class DNameRecord : DomainNameOnly
	{
		public string DomainName
		{
			get
			{
				return base.Domain;
			}
		}

		public DNameRecord(DataBuffer buffer)
			: base(buffer)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
