using System;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO
{
	[Serializable]
	internal class StreamOverflowException : IOException
	{
		public StreamOverflowException()
		{
		}

		public StreamOverflowException(string message)
			: base(message)
		{
		}

		public StreamOverflowException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
