using System;
using System.Reflection;
using Tr.Com.Eimza.Log4Net.Repository;

namespace Tr.Com.Eimza.Log4Net.Core
{
	public interface IRepositorySelector
	{
		event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent;

		ILoggerRepository GetRepository(Assembly assembly);

		ILoggerRepository GetRepository(string repositoryName);

		ILoggerRepository CreateRepository(Assembly assembly, Type repositoryType);

		ILoggerRepository CreateRepository(string repositoryName, Type repositoryType);

		bool ExistsRepository(string repositoryName);

		ILoggerRepository[] GetAllRepositories();
	}
}
