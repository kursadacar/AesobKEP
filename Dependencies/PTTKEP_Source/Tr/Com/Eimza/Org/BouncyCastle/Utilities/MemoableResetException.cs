using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities
{
	internal class MemoableResetException : InvalidCastException
	{
		public MemoableResetException(string msg)
			: base(msg)
		{
		}
	}
}
