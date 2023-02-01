using System.IO;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Layout.Pattern
{
	internal abstract class NamedPatternConverter : PatternLayoutConverter, IOptionHandler
	{
		protected int m_precision;

		public void ActivateOptions()
		{
			m_precision = 0;
			if (Option == null)
			{
				return;
			}
			string text = Option.Trim();
			if (text.Length <= 0)
			{
				return;
			}
			int val;
			if (SystemInfo.TryParse(text, out val))
			{
				if (val <= 0)
				{
					LogLog.Error("NamedPatternConverter: Precision option (" + text + ") isn't a positive integer.");
				}
				else
				{
					m_precision = val;
				}
			}
			else
			{
				LogLog.Error("NamedPatternConverter: Precision option \"" + text + "\" not a decimal integer.");
			}
		}

		protected abstract string GetFullyQualifiedName(LoggingEvent loggingEvent);

		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			string fullyQualifiedName = GetFullyQualifiedName(loggingEvent);
			if (m_precision <= 0)
			{
				writer.Write(fullyQualifiedName);
				return;
			}
			int length = fullyQualifiedName.Length;
			int num = length - 1;
			for (int num2 = m_precision; num2 > 0; num2--)
			{
				num = fullyQualifiedName.LastIndexOf('.', num - 1);
				if (num == -1)
				{
					writer.Write(fullyQualifiedName);
					return;
				}
			}
			writer.Write(fullyQualifiedName.Substring(num + 1, length - num - 1));
		}
	}
}
