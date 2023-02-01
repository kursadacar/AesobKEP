using System.IO;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Layout.Pattern
{
	internal sealed class LineLocationPatternConverter : PatternLayoutConverter
	{
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(loggingEvent.LocationInformation.LineNumber);
		}
	}
}
