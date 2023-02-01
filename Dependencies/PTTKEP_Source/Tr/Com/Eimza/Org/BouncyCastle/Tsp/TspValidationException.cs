using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tsp
{
	[Serializable]
	internal class TspValidationException : TspException
	{
		private readonly int failureCode;

		public int FailureCode
		{
			get
			{
				return failureCode;
			}
		}

		public TspValidationException(string message)
			: base(message)
		{
			failureCode = -1;
		}

		public TspValidationException(string message, int failureCode)
			: base(message)
		{
			this.failureCode = failureCode;
		}
	}
}
