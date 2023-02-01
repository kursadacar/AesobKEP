namespace ActiveUp.Net.Dns
{
	public class PrefAndDomain : IRecordData
	{
		private int preference;

		private string domain;

		public int Preference
		{
			get
			{
				return preference;
			}
		}

		public string Domain
		{
			get
			{
				return domain;
			}
		}

		public PrefAndDomain(DataBuffer buffer)
		{
			preference = buffer.ReadBEShortInt();
			domain = buffer.ReadDomainName();
		}

		public override string ToString()
		{
			return string.Format("Preference:{0} Domain:{1}", preference, domain);
		}
	}
}
