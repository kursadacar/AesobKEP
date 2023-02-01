using System;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.OpenSsl
{
	[Serializable]
	internal class PemException : IOException
	{
		public PemException(string message)
			: base(message)
		{
		}

		public PemException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
