namespace ActiveUp.Net.Dns
{
	internal class PXRecord : PrefAndDomain
	{
		private string x400Domain;

		public string X400Domain
		{
			get
			{
				return x400Domain;
			}
		}

		public PXRecord(DataBuffer buffer)
			: base(buffer)
		{
			x400Domain = buffer.ReadDomainName();
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
