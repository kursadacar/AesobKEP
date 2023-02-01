using System;
using System.Collections;
using System.Reflection;
using Tr.Com.Eimza.Log4Net.Config;
using Tr.Com.Eimza.Log4Net.Plugin;
using Tr.Com.Eimza.Log4Net.Repository;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Core
{
	public class DefaultRepositorySelector : IRepositorySelector
	{
		private const string DefaultRepositoryName = "log4net-default-repository";

		private readonly Hashtable m_name2repositoryMap = new Hashtable();

		private readonly Hashtable m_assembly2repositoryMap = new Hashtable();

		private readonly Hashtable m_alias2repositoryMap = new Hashtable();

		private readonly Type m_defaultRepositoryType;

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

		private event LoggerRepositoryCreationEventHandler m_loggerRepositoryCreatedEvent;

		public DefaultRepositorySelector(Type defaultRepositoryType)
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
			LogLog.Debug("DefaultRepositorySelector: defaultRepositoryType [" + (((object)defaultRepositoryType2 != null) ? defaultRepositoryType2.ToString() : null) + "]");
		}

		public ILoggerRepository GetRepository(Assembly repositoryAssembly)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			return CreateRepository(repositoryAssembly, m_defaultRepositoryType);
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

		public ILoggerRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType)
		{
			return CreateRepository(repositoryAssembly, repositoryType, "log4net-default-repository", true);
		}

		public ILoggerRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType, string repositoryName, bool readAssemblyAttributes)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			if ((object)repositoryType == null)
			{
				repositoryType = m_defaultRepositoryType;
			}
			lock (this)
			{
				ILoggerRepository loggerRepository = m_assembly2repositoryMap[repositoryAssembly] as ILoggerRepository;
				if (loggerRepository == null)
				{
					LogLog.Debug("DefaultRepositorySelector: Creating repository for assembly [" + (((object)repositoryAssembly != null) ? repositoryAssembly.ToString() : null) + "]");
					string repositoryName2 = repositoryName;
					Type repositoryType2 = repositoryType;
					if (readAssemblyAttributes)
					{
						GetInfoForAssembly(repositoryAssembly, ref repositoryName2, ref repositoryType2);
					}
					string[] obj = new string[7] { "DefaultRepositorySelector: Assembly [", null, null, null, null, null, null };
					obj[1] = (((object)repositoryAssembly != null) ? repositoryAssembly.ToString() : null);
					obj[2] = "] using repository [";
					obj[3] = repositoryName2;
					obj[4] = "] and repository type [";
					Type type = repositoryType2;
					obj[5] = (((object)type != null) ? type.ToString() : null);
					obj[6] = "]";
					LogLog.Debug(string.Concat(obj));
					loggerRepository = m_name2repositoryMap[repositoryName2] as ILoggerRepository;
					if (loggerRepository == null)
					{
						loggerRepository = CreateRepository(repositoryName2, repositoryType2);
						if (readAssemblyAttributes)
						{
							try
							{
								LoadAliases(repositoryAssembly, loggerRepository);
								LoadPlugins(repositoryAssembly, loggerRepository);
								ConfigureRepository(repositoryAssembly, loggerRepository);
							}
							catch (Exception exception)
							{
								LogLog.Error("DefaultRepositorySelector: Failed to configure repository [" + repositoryName2 + "] from assembly attributes.", exception);
							}
						}
					}
					else
					{
						LogLog.Debug("DefaultRepositorySelector: repository [" + repositoryName2 + "] already exists, using repository type [" + loggerRepository.GetType().FullName + "]");
						if (readAssemblyAttributes)
						{
							try
							{
								LoadPlugins(repositoryAssembly, loggerRepository);
							}
							catch (Exception exception2)
							{
								LogLog.Error("DefaultRepositorySelector: Failed to configure repository [" + repositoryName2 + "] from assembly attributes.", exception2);
							}
						}
					}
					m_assembly2repositoryMap[repositoryAssembly] = loggerRepository;
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
				ILoggerRepository loggerRepository2 = m_alias2repositoryMap[repositoryName] as ILoggerRepository;
				if (loggerRepository2 != null)
				{
					if ((object)loggerRepository2.GetType() == repositoryType)
					{
						LogLog.Debug("DefaultRepositorySelector: Aliasing repository [" + repositoryName + "] to existing repository [" + loggerRepository2.Name + "]");
						loggerRepository = loggerRepository2;
						m_name2repositoryMap[repositoryName] = loggerRepository;
					}
					else
					{
						LogLog.Error("DefaultRepositorySelector: Failed to alias repository [" + repositoryName + "] to existing repository [" + loggerRepository2.Name + "]. Requested repository type [" + repositoryType.FullName + "] is not compatible with existing type [" + loggerRepository2.GetType().FullName + "]");
					}
				}
				if (loggerRepository == null)
				{
					string[] obj = new string[5] { "DefaultRepositorySelector: Creating repository [", repositoryName, "] using type [", null, null };
					Type type = repositoryType;
					obj[3] = (((object)type != null) ? type.ToString() : null);
					obj[4] = "]";
					LogLog.Debug(string.Concat(obj));
					loggerRepository = (ILoggerRepository)Activator.CreateInstance(repositoryType);
					loggerRepository.Name = repositoryName;
					m_name2repositoryMap[repositoryName] = loggerRepository;
					OnLoggerRepositoryCreatedEvent(loggerRepository);
				}
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

		public void AliasRepository(string repositoryAlias, ILoggerRepository repositoryTarget)
		{
			if (repositoryAlias == null)
			{
				throw new ArgumentNullException("repositoryAlias");
			}
			if (repositoryTarget == null)
			{
				throw new ArgumentNullException("repositoryTarget");
			}
			lock (this)
			{
				if (m_alias2repositoryMap.Contains(repositoryAlias))
				{
					if (repositoryTarget != (ILoggerRepository)m_alias2repositoryMap[repositoryAlias])
					{
						throw new InvalidOperationException("Repository [" + repositoryAlias + "] is already aliased to repository [" + ((ILoggerRepository)m_alias2repositoryMap[repositoryAlias]).Name + "]. Aliases cannot be redefined.");
					}
				}
				else if (m_name2repositoryMap.Contains(repositoryAlias))
				{
					if (repositoryTarget != (ILoggerRepository)m_name2repositoryMap[repositoryAlias])
					{
						throw new InvalidOperationException("Repository [" + repositoryAlias + "] already exists and cannot be aliased to repository [" + repositoryTarget.Name + "].");
					}
				}
				else
				{
					m_alias2repositoryMap[repositoryAlias] = repositoryTarget;
				}
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

		private void GetInfoForAssembly(Assembly assembly, ref string repositoryName, ref Type repositoryType)
		{
			if ((object)assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			try
			{
				LogLog.Debug("DefaultRepositorySelector: Assembly [" + assembly.FullName + "] Loaded From [" + SystemInfo.AssemblyLocationInfo(assembly) + "]");
			}
			catch
			{
			}
			try
			{
				object[] customAttributes = Attribute.GetCustomAttributes(assembly, typeof(RepositoryAttribute), false);
				object[] array = customAttributes;
				if (array == null || array.Length == 0)
				{
					LogLog.Debug("DefaultRepositorySelector: Assembly [" + (((object)assembly != null) ? assembly.ToString() : null) + "] does not have a RepositoryAttribute specified.");
					return;
				}
				if (array.Length > 1)
				{
					LogLog.Error("DefaultRepositorySelector: Assembly [" + (((object)assembly != null) ? assembly.ToString() : null) + "] has multiple log4net.Config.RepositoryAttribute assembly attributes. Only using first occurrence.");
				}
				RepositoryAttribute repositoryAttribute = array[0] as RepositoryAttribute;
				if (repositoryAttribute == null)
				{
					LogLog.Error("DefaultRepositorySelector: Assembly [" + (((object)assembly != null) ? assembly.ToString() : null) + "] has a RepositoryAttribute but it does not!.");
					return;
				}
				if (repositoryAttribute.Name != null)
				{
					repositoryName = repositoryAttribute.Name;
				}
				if ((object)repositoryAttribute.RepositoryType != null)
				{
					if (typeof(ILoggerRepository).IsAssignableFrom(repositoryAttribute.RepositoryType))
					{
						repositoryType = repositoryAttribute.RepositoryType;
						return;
					}
					Type repositoryType2 = repositoryAttribute.RepositoryType;
					LogLog.Error("DefaultRepositorySelector: Repository Type [" + (((object)repositoryType2 != null) ? repositoryType2.ToString() : null) + "] must implement the ILoggerRepository interface.");
				}
			}
			catch (Exception exception)
			{
				LogLog.Error("DefaultRepositorySelector: Unhandled exception in GetInfoForAssembly", exception);
			}
		}

		private void ConfigureRepository(Assembly assembly, ILoggerRepository repository)
		{
			if ((object)assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			object[] customAttributes = Attribute.GetCustomAttributes(assembly, typeof(ConfiguratorAttribute), false);
			object[] array = customAttributes;
			if (array != null && array.Length != 0)
			{
				Array.Sort(array);
				customAttributes = array;
				for (int i = 0; i < customAttributes.Length; i++)
				{
					ConfiguratorAttribute configuratorAttribute = (ConfiguratorAttribute)customAttributes[i];
					if (configuratorAttribute != null)
					{
						try
						{
							configuratorAttribute.Configure(assembly, repository);
						}
						catch (Exception exception)
						{
							LogLog.Error("DefaultRepositorySelector: Exception calling [" + configuratorAttribute.GetType().FullName + "] .Configure method.", exception);
						}
					}
				}
			}
			if (!(repository.Name == "log4net-default-repository"))
			{
				return;
			}
			string appSetting = SystemInfo.GetAppSetting("log4net.Config");
			if (appSetting == null || appSetting.Length <= 0)
			{
				return;
			}
			string text = null;
			try
			{
				text = SystemInfo.ApplicationBaseDirectory;
			}
			catch (Exception exception2)
			{
				LogLog.Warn("DefaultRepositorySelector: Exception getting ApplicationBaseDirectory. appSettings log4net.Config path [" + appSetting + "] will be treated as an absolute URI", exception2);
			}
			Uri uri = null;
			try
			{
				uri = ((text == null) ? new Uri(appSetting) : new Uri(new Uri(text), appSetting));
			}
			catch (Exception exception3)
			{
				LogLog.Error("DefaultRepositorySelector: Exception while parsing log4net.Config file path [" + appSetting + "]", exception3);
			}
			if (uri != null)
			{
				LogLog.Debug("DefaultRepositorySelector: Loading configuration for default repository from AppSettings specified Config URI [" + uri.ToString() + "]");
				try
				{
					XmlConfigurator.Configure(repository, uri);
				}
				catch (Exception exception4)
				{
					Uri uri2 = uri;
					LogLog.Error("DefaultRepositorySelector: Exception calling XmlConfigurator.Configure method with ConfigUri [" + (((object)uri2 != null) ? uri2.ToString() : null) + "]", exception4);
				}
			}
		}

		private void LoadPlugins(Assembly assembly, ILoggerRepository repository)
		{
			if ((object)assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			object[] customAttributes = Attribute.GetCustomAttributes(assembly, typeof(PluginAttribute), false);
			object[] array = customAttributes;
			if (array == null || array.Length == 0)
			{
				return;
			}
			customAttributes = array;
			for (int i = 0; i < customAttributes.Length; i++)
			{
				IPluginFactory pluginFactory = (IPluginFactory)customAttributes[i];
				try
				{
					repository.PluginMap.Add(pluginFactory.CreatePlugin());
				}
				catch (Exception exception)
				{
					LogLog.Error("DefaultRepositorySelector: Failed to create plugin. Attribute [" + pluginFactory.ToString() + "]", exception);
				}
			}
		}

		private void LoadAliases(Assembly assembly, ILoggerRepository repository)
		{
			if ((object)assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			object[] customAttributes = Attribute.GetCustomAttributes(assembly, typeof(AliasRepositoryAttribute), false);
			object[] array = customAttributes;
			if (array == null || array.Length == 0)
			{
				return;
			}
			customAttributes = array;
			for (int i = 0; i < customAttributes.Length; i++)
			{
				AliasRepositoryAttribute aliasRepositoryAttribute = (AliasRepositoryAttribute)customAttributes[i];
				try
				{
					AliasRepository(aliasRepositoryAttribute.Name, repository);
				}
				catch (Exception exception)
				{
					LogLog.Error("DefaultRepositorySelector: Failed to alias repository [" + aliasRepositoryAttribute.Name + "]", exception);
				}
			}
		}
	}
}
