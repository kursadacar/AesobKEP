namespace ActiveUp.Net.Dns
{
	internal class KeyRecord : IRecordData
	{
		private short flags;

		private byte protocol;

		private byte algorithm;

		private byte[] publicKey;

		public short Flags
		{
			get
			{
				return flags;
			}
		}

		public byte Protocol
		{
			get
			{
				return protocol;
			}
		}

		public byte Algorithm
		{
			get
			{
				return algorithm;
			}
		}

		public byte[] PublicKey
		{
			get
			{
				return publicKey;
			}
		}

		public KeyRecord(DataBuffer buffer, int length)
		{
			flags = buffer.ReadShortInt();
			protocol = buffer.ReadByte();
			algorithm = buffer.ReadByte();
			publicKey = buffer.ReadBytes(length - 4);
		}

		public override string ToString()
		{
			return string.Format("Flags:{0} Protocol:{1} Algorithm:{2} Public Key:{3}", flags, protocol, algorithm, publicKey);
		}
	}
}
