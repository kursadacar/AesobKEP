using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	[Serializable]
	internal class PgpKeyValidationException : PgpException
	{
		public PgpKeyValidationException()
		{
		}

		public PgpKeyValidationException(string message)
			: base(message)
		{
		}

		public PgpKeyValidationException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
