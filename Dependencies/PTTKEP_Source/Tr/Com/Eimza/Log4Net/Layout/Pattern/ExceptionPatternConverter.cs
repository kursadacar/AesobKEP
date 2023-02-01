using System.IO;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Layout.Pattern
{
	internal sealed class ExceptionPatternConverter : PatternLayoutConverter
	{
		public ExceptionPatternConverter()
		{
			IgnoresException = false;
		}

		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			string exceptionString = loggingEvent.GetExceptionString();
			if (exceptionString != null && exceptionString.Length > 0)
			{
				writer.WriteLine(exceptionString);
			}
		}
	}
}
