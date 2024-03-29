using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO.Pem
{
	[Serializable]
	internal class PemGenerationException : Exception
	{
		public PemGenerationException()
		{
		}

		public PemGenerationException(string message)
			: base(message)
		{
		}

		public PemGenerationException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
