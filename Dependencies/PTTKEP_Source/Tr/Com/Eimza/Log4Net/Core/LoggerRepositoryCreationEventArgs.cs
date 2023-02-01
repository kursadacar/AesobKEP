using System;
using Tr.Com.Eimza.Log4Net.Repository;

namespace Tr.Com.Eimza.Log4Net.Core
{
	public class LoggerRepositoryCreationEventArgs : EventArgs
	{
		private ILoggerRepository m_repository;

		public ILoggerRepository LoggerRepository
		{
			get
			{
				return m_repository;
			}
		}

		public LoggerRepositoryCreationEventArgs(ILoggerRepository repository)
		{
			m_repository = repository;
		}
	}
}
