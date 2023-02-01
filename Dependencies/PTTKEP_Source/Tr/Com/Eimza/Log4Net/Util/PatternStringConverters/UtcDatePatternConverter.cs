using System;
using System.IO;

namespace Tr.Com.Eimza.Log4Net.Util.PatternStringConverters
{
	internal class UtcDatePatternConverter : DatePatternConverter
	{
		protected override void Convert(TextWriter writer, object state)
		{
			try
			{
				m_dateFormatter.FormatDate(DateTime.UtcNow, writer);
			}
			catch (Exception exception)
			{
				LogLog.Error("UtcDatePatternConverter: Error occurred while converting date.", exception);
			}
		}
	}
}
