using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ocsp
{
	[Serializable]
	internal class OcspException : Exception
	{
		public OcspException()
		{
		}

		public OcspException(string message)
			: base(message)
		{
		}

		public OcspException(string message, Exception e)
			: base(message, e)
		{
		}
	}
}
