using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tsp
{
	[Serializable]
	internal class TspException : Exception
	{
		public TspException()
		{
		}

		public TspException(string message)
			: base(message)
		{
		}

		public TspException(string message, Exception e)
			: base(message, e)
		{
		}
	}
}
