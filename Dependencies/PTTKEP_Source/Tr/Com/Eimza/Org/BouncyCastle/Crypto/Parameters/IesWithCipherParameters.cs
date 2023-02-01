namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class IesWithCipherParameters : IesParameters
	{
		private int cipherKeySize;

		public int CipherKeySize
		{
			get
			{
				return cipherKeySize;
			}
		}

		public IesWithCipherParameters(byte[] derivation, byte[] encoding, int macKeySize, int cipherKeySize)
			: base(derivation, encoding, macKeySize)
		{
			this.cipherKeySize = cipherKeySize;
		}
	}
}
