using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Security
{
	[Serializable]
	internal class NoSuchAlgorithmException : GeneralSecurityException
	{
		public NoSuchAlgorithmException()
		{
		}

		public NoSuchAlgorithmException(string message)
			: base(message)
		{
		}

		public NoSuchAlgorithmException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
