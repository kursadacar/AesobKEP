using System;
using System.IO;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Layout.Pattern
{
	internal class UtcDatePatternConverter : DatePatternConverter
	{
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			try
			{
				m_dateFormatter.FormatDate(loggingEvent.TimeStamp.ToUniversalTime(), writer);
			}
			catch (Exception exception)
			{
				LogLog.Error("UtcDatePatternConverter: Error occurred while converting date.", exception);
			}
		}
	}
}
