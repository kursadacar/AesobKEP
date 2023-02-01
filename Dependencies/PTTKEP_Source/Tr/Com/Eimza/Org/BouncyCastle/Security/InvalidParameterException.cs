using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Security
{
	[Serializable]
	internal class InvalidParameterException : KeyException
	{
		public InvalidParameterException()
		{
		}

		public InvalidParameterException(string message)
			: base(message)
		{
		}

		public InvalidParameterException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
