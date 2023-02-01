using System.IO;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Layout.Pattern
{
	internal sealed class ThreadPatternConverter : PatternLayoutConverter
	{
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(loggingEvent.ThreadName);
		}
	}
}
