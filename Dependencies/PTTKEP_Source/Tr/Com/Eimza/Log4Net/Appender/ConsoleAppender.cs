using System;
using System.Globalization;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Layout;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class ConsoleAppender : AppenderSkeleton
	{
		public const string ConsoleOut = "Console.Out";

		public const string ConsoleError = "Console.Error";

		private bool m_writeToErrorStream;

		public virtual string Target
		{
			get
			{
				if (!m_writeToErrorStream)
				{
					return "Console.Out";
				}
				return "Console.Error";
			}
			set
			{
				string strB = value.Trim();
				if (string.Compare("Console.Error", strB, true, CultureInfo.InvariantCulture) == 0)
				{
					m_writeToErrorStream = true;
				}
				else
				{
					m_writeToErrorStream = false;
				}
			}
		}

		protected override bool RequiresLayout
		{
			get
			{
				return true;
			}
		}

		public ConsoleAppender()
		{
		}

		[Obsolete("Instead use the default constructor and set the Layout property")]
		public ConsoleAppender(ILayout layout)
			: this(layout, false)
		{
		}

		[Obsolete("Instead use the default constructor and set the Layout & Target properties")]
		public ConsoleAppender(ILayout layout, bool writeToErrorStream)
		{
			Layout = layout;
			m_writeToErrorStream = writeToErrorStream;
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			if (m_writeToErrorStream)
			{
				Console.Error.Write(RenderLoggingEvent(loggingEvent));
			}
			else
			{
				Console.Write(RenderLoggingEvent(loggingEvent));
			}
		}
	}
}
