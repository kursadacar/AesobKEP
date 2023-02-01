namespace ActiveUp.Net.Dns
{
	internal class DomainNameOnly : IRecordData
	{
		private string domain;

		protected string Domain
		{
			get
			{
				return domain;
			}
		}

		public DomainNameOnly(DataBuffer buffer)
		{
			domain = buffer.ReadDomainName();
		}

		public override string ToString()
		{
			return "Domain: " + Domain;
		}
	}
}
