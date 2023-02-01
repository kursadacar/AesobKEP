using System;
using System.Collections;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class MemoryAppender : AppenderSkeleton
	{
		protected ArrayList m_eventsList;

		protected FixFlags m_fixFlags = FixFlags.All;

		[Obsolete("Use Fix property")]
		public virtual bool OnlyFixPartialEventData
		{
			get
			{
				return Fix == FixFlags.Partial;
			}
			set
			{
				if (value)
				{
					Fix = FixFlags.Partial;
				}
				else
				{
					Fix = FixFlags.All;
				}
			}
		}

		public virtual FixFlags Fix
		{
			get
			{
				return m_fixFlags;
			}
			set
			{
				m_fixFlags = value;
			}
		}

		public MemoryAppender()
		{
			m_eventsList = new ArrayList();
		}

		public virtual LoggingEvent[] GetEvents()
		{
			return (LoggingEvent[])m_eventsList.ToArray(typeof(LoggingEvent));
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			loggingEvent.Fix = Fix;
			m_eventsList.Add(loggingEvent);
		}

		public virtual void Clear()
		{
			m_eventsList.Clear();
		}
	}
}
