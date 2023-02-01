using System;

namespace Tr.Com.Eimza.Log4Net.Repository.Hierarchy
{
	public class LoggerCreationEventArgs : EventArgs
	{
		private Logger m_log;

		public Logger Logger
		{
			get
			{
				return m_log;
			}
		}

		public LoggerCreationEventArgs(Logger log)
		{
			m_log = log;
		}
	}
}
