namespace ActiveUp.Net.Dns
{
	internal class TxtRecord : TextOnly
	{
		public new string Text
		{
			get
			{
				return base.Text;
			}
		}

		public TxtRecord(DataBuffer buffer, int length)
			: base(buffer, length)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
