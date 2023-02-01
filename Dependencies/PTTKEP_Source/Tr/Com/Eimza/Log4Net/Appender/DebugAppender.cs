#define DEBUG
using System;
using System.Diagnostics;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Layout;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class DebugAppender : AppenderSkeleton
	{
		private bool m_immediateFlush = true;

		public bool ImmediateFlush
		{
			get
			{
				return m_immediateFlush;
			}
			set
			{
				m_immediateFlush = value;
			}
		}

		protected override bool RequiresLayout
		{
			get
			{
				return true;
			}
		}

		public DebugAppender()
		{
		}

		[Obsolete("Instead use the default constructor and set the Layout property")]
		public DebugAppender(ILayout layout)
		{
			Layout = layout;
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			Debug.Write(RenderLoggingEvent(loggingEvent), loggingEvent.LoggerName);
			if (m_immediateFlush)
			{
				Debug.Flush();
			}
		}
	}
}
