namespace ActiveUp.Net.Dns
{
	internal class NullRecord : TextOnly
	{
		public new string Text
		{
			get
			{
				return base.Text;
			}
		}

		public NullRecord(DataBuffer buffer, int length)
			: base(buffer, length)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
