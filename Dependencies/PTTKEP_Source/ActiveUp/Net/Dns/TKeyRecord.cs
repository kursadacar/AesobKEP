namespace ActiveUp.Net.Dns
{
	internal class TKeyRecord : IRecordData
	{
		private string algorithm;

		private uint inception;

		private uint expiration;

		private ushort mode;

		private ushort error;

		private ushort keySize;

		private byte[] keyData;

		private ushort otherSize;

		private byte[] otherData;

		public string Algorithm
		{
			get
			{
				return algorithm;
			}
		}

		public uint Inception
		{
			get
			{
				return inception;
			}
		}

		public uint Expiration
		{
			get
			{
				return expiration;
			}
		}

		public ushort Mode
		{
			get
			{
				return mode;
			}
		}

		public ushort Error
		{
			get
			{
				return error;
			}
		}

		public byte[] KeyData
		{
			get
			{
				return keyData;
			}
		}

		public byte[] OtherData
		{
			get
			{
				return otherData;
			}
		}

		public TKeyRecord(DataBuffer buffer)
		{
			algorithm = buffer.ReadDomainName();
			inception = buffer.ReadUInt();
			expiration = buffer.ReadUInt();
			mode = buffer.ReadShortUInt();
			error = buffer.ReadShortUInt();
			keySize = buffer.ReadShortUInt();
			keyData = buffer.ReadBytes(keySize);
			otherSize = buffer.ReadShortUInt();
			otherData = buffer.ReadBytes(otherSize);
		}

		public override string ToString()
		{
			return string.Format("Algorithm:{0} Inception:{1} Expiration:{2} Mode:{3} Error:{4} \nKey Data:{5} \nOther Data:{6} ", algorithm, inception, expiration, mode, error, keyData, otherData);
		}
	}
}
