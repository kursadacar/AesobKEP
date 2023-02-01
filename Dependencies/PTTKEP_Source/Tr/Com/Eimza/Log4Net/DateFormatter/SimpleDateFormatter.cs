using System;
using System.Globalization;
using System.IO;

namespace Tr.Com.Eimza.Log4Net.DateFormatter
{
	public class SimpleDateFormatter : IDateFormatter
	{
		private readonly string m_formatString;

		public SimpleDateFormatter(string format)
		{
			m_formatString = format;
		}

		public virtual void FormatDate(DateTime dateToFormat, TextWriter writer)
		{
			writer.Write(dateToFormat.ToString(m_formatString, DateTimeFormatInfo.InvariantInfo));
		}
	}
}
