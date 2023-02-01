using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	[Serializable]
	internal class PgpDataValidationException : PgpException
	{
		public PgpDataValidationException()
		{
		}

		public PgpDataValidationException(string message)
			: base(message)
		{
		}

		public PgpDataValidationException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
