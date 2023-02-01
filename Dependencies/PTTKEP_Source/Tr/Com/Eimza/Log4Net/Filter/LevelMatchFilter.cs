using System;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Filter
{
	public class LevelMatchFilter : FilterSkeleton
	{
		private bool m_acceptOnMatch = true;

		private Level m_levelToMatch;

		public bool AcceptOnMatch
		{
			get
			{
				return m_acceptOnMatch;
			}
			set
			{
				m_acceptOnMatch = value;
			}
		}

		public Level LevelToMatch
		{
			get
			{
				return m_levelToMatch;
			}
			set
			{
				m_levelToMatch = value;
			}
		}

		public override FilterDecision Decide(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			if (m_levelToMatch != null && m_levelToMatch == loggingEvent.Level)
			{
				if (!m_acceptOnMatch)
				{
					return FilterDecision.Deny;
				}
				return FilterDecision.Accept;
			}
			return FilterDecision.Neutral;
		}
	}
}
