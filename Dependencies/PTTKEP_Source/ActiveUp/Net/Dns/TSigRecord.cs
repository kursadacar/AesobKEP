namespace ActiveUp.Net.Dns
{
	internal class TSigRecord : IRecordData
	{
		private string algorithm;

		private long timeSigned;

		private ushort fudge;

		private ushort macSize;

		private byte[] mac;

		private ushort originalId;

		private ushort error;

		private ushort otherLen;

		private byte[] otherData;

		public string Algorithm
		{
			get
			{
				return algorithm;
			}
		}

		public long TimeSigned
		{
			get
			{
				return timeSigned;
			}
		}

		public ushort Fudge
		{
			get
			{
				return fudge;
			}
		}

		public byte[] Mac
		{
			get
			{
				return mac;
			}
		}

		public ushort OriginalId
		{
			get
			{
				return originalId;
			}
		}

		public ushort Error
		{
			get
			{
				return error;
			}
		}

		public byte[] OtherData
		{
			get
			{
				return otherData;
			}
		}

		public TSigRecord(DataBuffer buffer)
		{
			algorithm = buffer.ReadDomainName();
			timeSigned = buffer.ReadLongInt();
			fudge = buffer.ReadShortUInt();
			macSize = buffer.ReadShortUInt();
			mac = buffer.ReadBytes(macSize);
			originalId = buffer.ReadShortUInt();
			error = buffer.ReadShortUInt();
			otherLen = buffer.ReadShortUInt();
			otherData = buffer.ReadBytes(otherLen);
		}

		public override string ToString()
		{
			return string.Format("Algorithm:{0} Signed Time:{1} Fudge Factor:{2} Mac:{3} Original ID:{4} Error:{5}\nOther Data:{6}", algorithm, timeSigned, fudge, mac, originalId, error, otherData);
		}
	}
}
