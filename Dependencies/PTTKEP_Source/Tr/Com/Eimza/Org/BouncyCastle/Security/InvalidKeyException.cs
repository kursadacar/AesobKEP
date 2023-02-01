using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Security
{
	[Serializable]
	internal class InvalidKeyException : KeyException
	{
		public InvalidKeyException()
		{
		}

		public InvalidKeyException(string message)
			: base(message)
		{
		}

		public InvalidKeyException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
