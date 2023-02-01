namespace ActiveUp.Net.Dns
{
	internal class DSRecord : IRecordData
	{
		private short key;

		private byte algorithm;

		private byte digestType;

		private byte[] digest;

		public short Key
		{
			get
			{
				return key;
			}
		}

		public byte Algorithm
		{
			get
			{
				return algorithm;
			}
		}

		public byte DigestType
		{
			get
			{
				return digestType;
			}
		}

		public byte[] Digest
		{
			get
			{
				return digest;
			}
		}

		public DSRecord(DataBuffer buffer, int length)
		{
			key = buffer.ReadShortInt();
			algorithm = buffer.ReadByte();
			digestType = buffer.ReadByte();
			digest = buffer.ReadBytes(length - 4);
		}

		public override string ToString()
		{
			return string.Format("Key:{0} Algorithm:{1} DigestType:{2} Digest:{3}", key, algorithm, digestType, digest);
		}
	}
}
