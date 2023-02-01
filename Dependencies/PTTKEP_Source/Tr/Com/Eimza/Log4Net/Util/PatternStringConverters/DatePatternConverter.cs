using System;
using System.Globalization;
using System.IO;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.DateFormatter;

namespace Tr.Com.Eimza.Log4Net.Util.PatternStringConverters
{
	internal class DatePatternConverter : PatternConverter, IOptionHandler
	{
		protected IDateFormatter m_dateFormatter;

		public void ActivateOptions()
		{
			string text = Option;
			if (text == null)
			{
				text = "ISO8601";
			}
			if (string.Compare(text, "ISO8601", true, CultureInfo.InvariantCulture) == 0)
			{
				m_dateFormatter = new Iso8601DateFormatter();
				return;
			}
			if (string.Compare(text, "ABSOLUTE", true, CultureInfo.InvariantCulture) == 0)
			{
				m_dateFormatter = new AbsoluteTimeDateFormatter();
				return;
			}
			if (string.Compare(text, "DATE", true, CultureInfo.InvariantCulture) == 0)
			{
				m_dateFormatter = new DateTimeDateFormatter();
				return;
			}
			try
			{
				m_dateFormatter = new SimpleDateFormatter(text);
			}
			catch (Exception exception)
			{
				LogLog.Error("DatePatternConverter: Could not instantiate SimpleDateFormatter with [" + text + "]", exception);
				m_dateFormatter = new Iso8601DateFormatter();
			}
		}

		protected override void Convert(TextWriter writer, object state)
		{
			try
			{
				m_dateFormatter.FormatDate(DateTime.Now, writer);
			}
			catch (Exception exception)
			{
				LogLog.Error("DatePatternConverter: Error occurred while converting date.", exception);
			}
		}
	}
}
