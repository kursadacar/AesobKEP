namespace ActiveUp.Net.Dns
{
	internal class MRRecord : DomainNameOnly
	{
		public string ForwardingAddress
		{
			get
			{
				return base.Domain;
			}
		}

		public MRRecord(DataBuffer buffer)
			: base(buffer)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
