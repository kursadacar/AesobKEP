namespace ActiveUp.Net.Dns
{
	internal class SigRecord : IRecordData
	{
		private short coveredType;

		private byte algorithm;

		private byte numLabels;

		private uint expiration;

		private uint inception;

		private short keyTag;

		private string signer;

		public short CoveredType
		{
			get
			{
				return coveredType;
			}
		}

		public byte Algorithm
		{
			get
			{
				return algorithm;
			}
		}

		public byte NumLabels
		{
			get
			{
				return numLabels;
			}
		}

		public uint Expiration
		{
			get
			{
				return expiration;
			}
		}

		public uint Inception
		{
			get
			{
				return inception;
			}
		}

		public short KeyTag
		{
			get
			{
				return keyTag;
			}
		}

		public string Signer
		{
			get
			{
				return signer;
			}
		}

		public SigRecord(DataBuffer buffer, int length)
		{
			int position = buffer.Position;
			coveredType = buffer.ReadShortInt();
			algorithm = buffer.ReadByte();
			numLabels = buffer.ReadByte();
			expiration = buffer.ReadUInt();
			inception = buffer.ReadUInt();
			keyTag = buffer.ReadShortInt();
			signer = buffer.ReadDomainName();
			buffer.Position = position - length;
		}

		public override string ToString()
		{
			return string.Format("Covered Type:{0} Algorithm:{1} Number of Labels:{2} Expiration:{3} Inception:{4} Key Tag:{5} Signer:{6}", coveredType, algorithm, numLabels, expiration, inception, keyTag, signer);
		}
	}
}
