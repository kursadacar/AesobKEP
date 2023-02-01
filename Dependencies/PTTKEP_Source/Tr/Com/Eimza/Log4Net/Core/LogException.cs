using System;
using System.Runtime.Serialization;

namespace Tr.Com.Eimza.Log4Net.Core
{
	[Serializable]
	public class LogException : ApplicationException
	{
		public LogException()
		{
		}

		public LogException(string message)
			: base(message)
		{
		}

		public LogException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected LogException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
