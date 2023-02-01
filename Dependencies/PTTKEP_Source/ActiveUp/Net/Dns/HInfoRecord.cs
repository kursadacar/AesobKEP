namespace ActiveUp.Net.Dns
{
	internal class HInfoRecord : TextOnly
	{
		public string Cpu
		{
			get
			{
				if (base.Count > 0)
				{
					return base.Strings[0];
				}
				return "Unknown";
			}
		}

		public string Os
		{
			get
			{
				if (base.Count > 1)
				{
					return base.Strings[1];
				}
				return "Unknown";
			}
		}

		public HInfoRecord(DataBuffer buffer, int length)
			: base(buffer, length)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
