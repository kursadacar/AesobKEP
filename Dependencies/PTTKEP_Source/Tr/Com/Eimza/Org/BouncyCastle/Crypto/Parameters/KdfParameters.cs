namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class KdfParameters : IDerivationParameters
	{
		private byte[] iv;

		private byte[] shared;

		public KdfParameters(byte[] shared, byte[] iv)
		{
			this.shared = shared;
			this.iv = iv;
		}

		public byte[] GetSharedSecret()
		{
			return shared;
		}

		public byte[] GetIV()
		{
			return iv;
		}
	}
}
