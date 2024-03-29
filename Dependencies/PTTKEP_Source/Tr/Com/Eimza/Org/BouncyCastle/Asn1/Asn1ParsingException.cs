using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	[Serializable]
	internal class Asn1ParsingException : InvalidOperationException
	{
		public Asn1ParsingException()
		{
		}

		public Asn1ParsingException(string message)
			: base(message)
		{
		}

		public Asn1ParsingException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
