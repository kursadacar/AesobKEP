namespace ActiveUp.Net.Dns
{
	internal class X25Record : TextOnly
	{
		public string PsdnAddress
		{
			get
			{
				return base.Text;
			}
		}

		public X25Record(DataBuffer buffer)
			: base(buffer)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
