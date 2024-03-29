using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	[Serializable]
	internal class CryptoException : Exception
	{
		public CryptoException()
		{
		}

		public CryptoException(string message)
			: base(message)
		{
		}

		public CryptoException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
