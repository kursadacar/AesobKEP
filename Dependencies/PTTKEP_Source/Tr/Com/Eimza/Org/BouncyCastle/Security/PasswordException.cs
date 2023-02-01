using System;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Security
{
	[Serializable]
	internal class PasswordException : IOException
	{
		public PasswordException(string message)
			: base(message)
		{
		}

		public PasswordException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
