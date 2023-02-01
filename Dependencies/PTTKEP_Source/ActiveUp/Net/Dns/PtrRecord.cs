namespace ActiveUp.Net.Dns
{
	internal class PtrRecord : DomainNameOnly
	{
		public string PtrDomain
		{
			get
			{
				return base.Domain;
			}
		}

		public PtrRecord(DataBuffer buffer)
			: base(buffer)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
