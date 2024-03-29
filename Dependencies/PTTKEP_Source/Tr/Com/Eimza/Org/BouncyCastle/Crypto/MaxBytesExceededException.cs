using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	[Serializable]
	internal class MaxBytesExceededException : CryptoException
	{
		public MaxBytesExceededException()
		{
		}

		public MaxBytesExceededException(string message)
			: base(message)
		{
		}

		public MaxBytesExceededException(string message, Exception e)
			: base(message, e)
		{
		}
	}
}
