namespace ActiveUp.Net.Dns
{
	internal class CNameRecord : DomainNameOnly
	{
		public new string Domain
		{
			get
			{
				return base.Domain;
			}
		}

		public CNameRecord(DataBuffer buffer)
			: base(buffer)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
