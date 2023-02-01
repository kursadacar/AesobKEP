namespace ActiveUp.Net.Dns
{
	internal class NSRecord : DomainNameOnly
	{
		public string NSDomain
		{
			get
			{
				return base.Domain;
			}
		}

		public NSRecord(DataBuffer buffer)
			: base(buffer)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
