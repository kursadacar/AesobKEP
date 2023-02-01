using System;

namespace ActiveUp.Net.Dns
{
	internal class NsapRecord : IRecordData
	{
		public NsapRecord(DataBuffer buffer, int length)
		{
			buffer.Position += length;
			throw new NotImplementedException("Experimental Record Type Unable to Implement");
		}
	}
}
