using System;
using System.Collections;
using System.Reflection;
using Tr.Com.Eimza.Log4Net.Repository;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Core
{
	public class CompactRepositorySelector : IRepositorySelector
	{
		private const string DefaultRepositoryName = "log4net-default-repository";

		private readonly Hashtable m_name2repositoryMap = new Hashtable();

		private readonly Type m_defaultRepositoryType;

		private event LoggerRepositoryCreationEventHandler m_loggerRepositoryCreatedEvent;

		public event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent
		{
			add
			{
				m_loggerRepositoryCreatedEvent += value;
			}
			remove
			{
				m_loggerRepositoryCreatedEvent -= value;
			}
		}

		public CompactRepositorySelector(Type defaultRepositoryType)
		{
			if ((object)defaultRepositoryType == null)
			{
				throw new ArgumentNullException("defaultRepositoryType");
			}
			if (!typeof(ILoggerRepository).IsAssignableFrom(defaultRepositoryType))
			{
				throw SystemInfo.CreateArgumentOutOfRangeException("defaultRepositoryType", defaultRepositoryType, "Parameter: defaultRepositoryType, Value: [" + (((object)defaultRepositoryType != null) ? defaultRepositoryType.ToString() : null) + "] out of range. Argument must implement the ILoggerRepository interface");
			}
			m_defaultRepositoryType = defaultRepositoryType;
			Type defaultRepositoryType2 = m_defaultRepositoryType;
			LogLog.Debug("CompactRepositorySelector: defaultRepositoryType [" + (((object)defaultRepositoryType2 != null) ? defaultRepositoryType2.ToString() : null) + "]");
		}

		public ILoggerRepository GetRepository(Assembly assembly)
		{
			return CreateRepository(assembly, m_defaultRepositoryType);
		}

		public ILoggerRepository GetRepository(string repositoryName)
		{
			if (repositoryName == null)
			{
				throw new ArgumentNullException("repositoryName");
			}
			lock (this)
			{
				ILoggerRepository obj = m_name2repositoryMap[repositoryName] as ILoggerRepository;
				if (obj == null)
				{
					throw new LogException("Repository [" + repositoryName + "] is NOT defined.");
				}
				return obj;
			}
		}

		public ILoggerRepository CreateRepository(Assembly assembly, Type repositoryType)
		{
			if ((object)repositoryType == null)
			{
				repositoryType = m_defaultRepositoryType;
			}
			lock (this)
			{
				ILoggerRepository loggerRepository = m_name2repositoryMap["log4net-default-repository"] as ILoggerRepository;
				if (loggerRepository == null)
				{
					loggerRepository = CreateRepository("log4net-default-repository", repositoryType);
				}
				return loggerRepository;
			}
		}

		public ILoggerRepository CreateRepository(string repositoryName, Type repositoryType)
		{
			if (repositoryName == null)
			{
				throw new ArgumentNullException("repositoryName");
			}
			if ((object)repositoryType == null)
			{
				repositoryType = m_defaultRepositoryType;
			}
			lock (this)
			{
				ILoggerRepository loggerRepository = null;
				loggerRepository = m_name2repositoryMap[repositoryName] as ILoggerRepository;
				if (loggerRepository != null)
				{
					throw new LogException("Repository [" + repositoryName + "] is already defined. Repositories cannot be redefined.");
				}
				string[] obj = new string[5] { "DefaultRepositorySelector: Creating repository [", repositoryName, "] using type [", null, null };
				Type type = repositoryType;
				obj[3] = (((object)type != null) ? type.ToString() : null);
				obj[4] = "]";
				LogLog.Debug(string.Concat(obj));
				loggerRepository = (ILoggerRepository)Activator.CreateInstance(repositoryType);
				loggerRepository.Name = repositoryName;
				m_name2repositoryMap[repositoryName] = loggerRepository;
				OnLoggerRepositoryCreatedEvent(loggerRepository);
				return loggerRepository;
			}
		}

		public bool ExistsRepository(string repositoryName)
		{
			lock (this)
			{
				return m_name2repositoryMap.ContainsKey(repositoryName);
			}
		}

		public ILoggerRepository[] GetAllRepositories()
		{
			lock (this)
			{
				ICollection values = m_name2repositoryMap.Values;
				ILoggerRepository[] array = new ILoggerRepository[values.Count];
				values.CopyTo(array, 0);
				return array;
			}
		}

		protected virtual void OnLoggerRepositoryCreatedEvent(ILoggerRepository repository)
		{
			LoggerRepositoryCreationEventHandler loggerRepositoryCreatedEvent = this.m_loggerRepositoryCreatedEvent;
			if (loggerRepositoryCreatedEvent != null)
			{
				loggerRepositoryCreatedEvent(this, new LoggerRepositoryCreationEventArgs(repository));
			}
		}
	}
}
