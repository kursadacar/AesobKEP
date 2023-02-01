namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class AeadParameters : ICipherParameters
	{
		private readonly byte[] associatedText;

		private readonly byte[] nonce;

		private readonly KeyParameter key;

		private readonly int macSize;

		public virtual KeyParameter Key
		{
			get
			{
				return key;
			}
		}

		public virtual int MacSize
		{
			get
			{
				return macSize;
			}
		}

		public AeadParameters(KeyParameter key, int macSize, byte[] nonce)
			: this(key, macSize, nonce, null)
		{
		}

		public AeadParameters(KeyParameter key, int macSize, byte[] nonce, byte[] associatedText)
		{
			this.key = key;
			this.nonce = nonce;
			this.macSize = macSize;
			this.associatedText = associatedText;
		}

		public virtual byte[] GetAssociatedText()
		{
			return associatedText;
		}

		public virtual byte[] GetNonce()
		{
			return nonce;
		}
	}
}
