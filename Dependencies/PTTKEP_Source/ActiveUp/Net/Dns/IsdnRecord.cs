namespace ActiveUp.Net.Dns
{
	internal class IsdnRecord : IRecordData
	{
		private string isdnAddress;

		private string subAddress;

		public string IsdnAddress
		{
			get
			{
				return isdnAddress;
			}
		}

		public string SubAddress
		{
			get
			{
				return subAddress;
			}
		}

		public IsdnRecord(DataBuffer buffer)
		{
			isdnAddress = buffer.ReadCharString();
			subAddress = buffer.ReadCharString();
		}

		public override string ToString()
		{
			return string.Format("ISDN Address:{0} Sub Address:{1}", isdnAddress, subAddress);
		}
	}
}
