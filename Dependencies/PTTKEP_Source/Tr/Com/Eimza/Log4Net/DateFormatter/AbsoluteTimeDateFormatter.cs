using System;
using System.IO;
using System.Text;

namespace Tr.Com.Eimza.Log4Net.DateFormatter
{
	public class AbsoluteTimeDateFormatter : IDateFormatter
	{
		public const string AbsoluteTimeDateFormat = "ABSOLUTE";

		public const string DateAndTimeDateFormat = "DATE";

		public const string Iso8601TimeDateFormat = "ISO8601";

		private static long s_lastTimeToTheSecond = 0L;

		private static StringBuilder s_lastTimeBuf = new StringBuilder();

		private static string s_lastTimeString;

		protected virtual void FormatDateWithoutMillis(DateTime dateToFormat, StringBuilder buffer)
		{
			int hour = dateToFormat.Hour;
			if (hour < 10)
			{
				buffer.Append('0');
			}
			buffer.Append(hour);
			buffer.Append(':');
			int minute = dateToFormat.Minute;
			if (minute < 10)
			{
				buffer.Append('0');
			}
			buffer.Append(minute);
			buffer.Append(':');
			int second = dateToFormat.Second;
			if (second < 10)
			{
				buffer.Append('0');
			}
			buffer.Append(second);
		}

		public virtual void FormatDate(DateTime dateToFormat, TextWriter writer)
		{
			long num = dateToFormat.Ticks - dateToFormat.Ticks % 10000000;
			if (s_lastTimeToTheSecond != num)
			{
				lock (s_lastTimeBuf)
				{
					if (s_lastTimeToTheSecond != num)
					{
						s_lastTimeBuf.Length = 0;
						FormatDateWithoutMillis(dateToFormat, s_lastTimeBuf);
						s_lastTimeString = s_lastTimeBuf.ToString();
						s_lastTimeToTheSecond = num;
					}
				}
			}
			writer.Write(s_lastTimeString);
			writer.Write(',');
			int millisecond = dateToFormat.Millisecond;
			if (millisecond < 100)
			{
				writer.Write('0');
			}
			if (millisecond < 10)
			{
				writer.Write('0');
			}
			writer.Write(millisecond);
		}
	}
}
