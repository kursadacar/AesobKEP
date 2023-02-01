using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Security
{
	[Serializable]
	internal class SecurityUtilityException : Exception
	{
		public SecurityUtilityException()
		{
		}

		public SecurityUtilityException(string message)
			: base(message)
		{
		}

		public SecurityUtilityException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
