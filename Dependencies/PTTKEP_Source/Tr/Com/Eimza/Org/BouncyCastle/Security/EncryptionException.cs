using System;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Security
{
	[Serializable]
	internal class EncryptionException : IOException
	{
		public EncryptionException(string message)
			: base(message)
		{
		}

		public EncryptionException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
