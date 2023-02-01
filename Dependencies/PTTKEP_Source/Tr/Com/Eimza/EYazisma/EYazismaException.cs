using System;

namespace Tr.Com.Eimza.EYazisma
{
	public class EYazismaException : Exception
	{
		public EYazismaException(string message)
			: base(message)
		{
		}

		public EYazismaException(string message, Exception exception)
			: base(message, exception)
		{
		}

		public EYazismaException(Exception exception)
			: base(null, exception)
		{
		}
	}
}
