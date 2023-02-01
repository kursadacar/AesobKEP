using System;
using System.Reflection;
using System.Security;
using System.Text;
using Tr.Com.Eimza.Log4Net.Repository;
using Tr.Com.Eimza.Log4Net.Repository.Hierarchy;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Core
{
	public sealed class LoggerManager
	{
		private static IRepositorySelector s_repositorySelector;

		public static IRepositorySelector RepositorySelector
		{
			get
			{
				return s_repositorySelector;
			}
			set
			{
				s_repositorySelector = value;
			}
		}

		private LoggerManager()
		{
		}

		static LoggerManager()
		{
			try
			{
				RegisterAppDomainEvents();
			}
			catch (SecurityException)
			{
				LogLog.Debug("LoggerManager: Security Exception (ControlAppDomain LinkDemand) while trying to register Shutdown handler with the AppDomain. LoggerManager.Shutdown() will not be called automatically when the AppDomain exits. It must be called programmatically.");
			}
			LogLog.Debug(GetVersionInfo());
			string appSetting = SystemInfo.GetAppSetting("log4net.RepositorySelector");
			if (appSetting != null && appSetting.Length > 0)
			{
				Type type = null;
				try
				{
					type = SystemInfo.GetTypeFromString(appSetting, false, true);
				}
				catch (Exception exception)
				{
					LogLog.Error("LoggerManager: Exception while resolving RepositorySelector Type [" + appSetting + "]", exception);
				}
				if ((object)type != null)
				{
					object obj = null;
					try
					{
						obj = Activator.CreateInstance(type);
					}
					catch (Exception exception2)
					{
						LogLog.Error("LoggerManager: Exception while creating RepositorySelector [" + type.FullName + "]", exception2);
					}
					if (obj != null && obj is IRepositorySelector)
					{
						s_repositorySelector = (IRepositorySelector)obj;
					}
					else
					{
						LogLog.Error("LoggerManager: RepositorySelector Type [" + type.FullName + "] is not an IRepositorySelector");
					}
				}
			}
			if (s_repositorySelector == null)
			{
				s_repositorySelector = new DefaultRepositorySelector(typeof(Hierarchy));
			}
		}

		private static void RegisterAppDomainEvents()
		{
			AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
			AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
		}

		[Obsolete("Use GetRepository instead of GetLoggerRepository")]
		public static ILoggerRepository GetLoggerRepository(string repository)
		{
			return GetRepository(repository);
		}

		[Obsolete("Use GetRepository instead of GetLoggerRepository")]
		public static ILoggerRepository GetLoggerRepository(Assembly repositoryAssembly)
		{
			return GetRepository(repositoryAssembly);
		}

		public static ILoggerRepository GetRepository(string repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			return RepositorySelector.GetRepository(repository);
		}

		public static ILoggerRepository GetRepository(Assembly repositoryAssembly)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			return RepositorySelector.GetRepository(repositoryAssembly);
		}

		public static ILogger Exists(string repository, string name)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return RepositorySelector.GetRepository(repository).Exists(name);
		}

		public static ILogger Exists(Assembly repositoryAssembly, string name)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return RepositorySelector.GetRepository(repositoryAssembly).Exists(name);
		}

		public static ILogger[] GetCurrentLoggers(string repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			return RepositorySelector.GetRepository(repository).GetCurrentLoggers();
		}

		public static ILogger[] GetCurrentLoggers(Assembly repositoryAssembly)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			return RepositorySelector.GetRepository(repositoryAssembly).GetCurrentLoggers();
		}

		public static ILogger GetLogger(string repository, string name)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return RepositorySelector.GetRepository(repository).GetLogger(name);
		}

		public static ILogger GetLogger(Assembly repositoryAssembly, string name)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return RepositorySelector.GetRepository(repositoryAssembly).GetLogger(name);
		}

		public static ILogger GetLogger(string repository, Type type)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			return RepositorySelector.GetRepository(repository).GetLogger(type.FullName);
		}

		public static ILogger GetLogger(Assembly repositoryAssembly, Type type)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			return RepositorySelector.GetRepository(repositoryAssembly).GetLogger(type.FullName);
		}

		public static void Shutdown()
		{
			ILoggerRepository[] allRepositories = GetAllRepositories();
			for (int i = 0; i < allRepositories.Length; i++)
			{
				allRepositories[i].Shutdown();
			}
		}

		public static void ShutdownRepository(string repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			RepositorySelector.GetRepository(repository).Shutdown();
		}

		public static void ShutdownRepository(Assembly repositoryAssembly)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			RepositorySelector.GetRepository(repositoryAssembly).Shutdown();
		}

		public static void ResetConfiguration(string repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			RepositorySelector.GetRepository(repository).ResetConfiguration();
		}

		public static void ResetConfiguration(Assembly repositoryAssembly)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			RepositorySelector.GetRepository(repositoryAssembly).ResetConfiguration();
		}

		[Obsolete("Use CreateRepository instead of CreateDomain")]
		public static ILoggerRepository CreateDomain(string repository)
		{
			return CreateRepository(repository);
		}

		public static ILoggerRepository CreateRepository(string repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			return RepositorySelector.CreateRepository(repository, null);
		}

		[Obsolete("Use CreateRepository instead of CreateDomain")]
		public static ILoggerRepository CreateDomain(string repository, Type repositoryType)
		{
			return CreateRepository(repository, repositoryType);
		}

		public static ILoggerRepository CreateRepository(string repository, Type repositoryType)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			if ((object)repositoryType == null)
			{
				throw new ArgumentNullException("repositoryType");
			}
			return RepositorySelector.CreateRepository(repository, repositoryType);
		}

		[Obsolete("Use CreateRepository instead of CreateDomain")]
		public static ILoggerRepository CreateDomain(Assembly repositoryAssembly, Type repositoryType)
		{
			return CreateRepository(repositoryAssembly, repositoryType);
		}

		public static ILoggerRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType)
		{
			if ((object)repositoryAssembly == null)
			{
				throw new ArgumentNullException("repositoryAssembly");
			}
			if ((object)repositoryType == null)
			{
				throw new ArgumentNullException("repositoryType");
			}
			return RepositorySelector.CreateRepository(repositoryAssembly, repositoryType);
		}

		public static ILoggerRepository[] GetAllRepositories()
		{
			return RepositorySelector.GetAllRepositories();
		}

		private static string GetVersionInfo()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			stringBuilder.Append("log4net assembly [").Append(executingAssembly.FullName).Append("]. ");
			stringBuilder.Append("Loaded from [").Append(SystemInfo.AssemblyLocationInfo(executingAssembly)).Append("]. ");
			stringBuilder.Append("(.NET Runtime [").Append(Environment.Version.ToString()).Append("]");
			stringBuilder.Append(" on ").Append(Environment.OSVersion.ToString());
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		private static void OnDomainUnload(object sender, EventArgs e)
		{
			Shutdown();
		}

		private static void OnProcessExit(object sender, EventArgs e)
		{
			Shutdown();
		}
	}
}
