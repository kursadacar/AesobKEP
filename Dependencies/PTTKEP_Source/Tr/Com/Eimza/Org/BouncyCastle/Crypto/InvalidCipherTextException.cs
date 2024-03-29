using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	[Serializable]
	internal class InvalidCipherTextException : CryptoException
	{
		public InvalidCipherTextException()
		{
		}

		public InvalidCipherTextException(string message)
			: base(message)
		{
		}

		public InvalidCipherTextException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
