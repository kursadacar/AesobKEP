namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg
{
	internal class CompressedDataPacket : InputStreamPacket
	{
		private readonly CompressionAlgorithmTag algorithm;

		public CompressionAlgorithmTag Algorithm
		{
			get
			{
				return algorithm;
			}
		}

		internal CompressedDataPacket(BcpgInputStream bcpgIn)
			: base(bcpgIn)
		{
			algorithm = (CompressionAlgorithmTag)bcpgIn.ReadByte();
		}
	}
}
