using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Xml;
using Tr.Com.Eimza.Log4Net.Repository;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Config
{
	public sealed class XmlConfigurator
	{
		private sealed class ConfigureAndWatchHandler
		{
			private FileInfo m_configFile;

			private ILoggerRepository m_repository;

			private Timer m_timer;

			private const int TimeoutMillis = 500;

			internal static void StartWatching(ILoggerRepository repository, FileInfo configFile)
			{
				new ConfigureAndWatchHandler(repository, configFile);
			}

			private ConfigureAndWatchHandler(ILoggerRepository repository, FileInfo configFile)
			{
				m_repository = repository;
				m_configFile = configFile;
				FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
				fileSystemWatcher.Path = m_configFile.DirectoryName;
				fileSystemWatcher.Filter = m_configFile.Name;
				fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
				fileSystemWatcher.Changed += ConfigureAndWatchHandler_OnChanged;
				fileSystemWatcher.Created += ConfigureAndWatchHandler_OnChanged;
				fileSystemWatcher.Deleted += ConfigureAndWatchHandler_OnChanged;
				fileSystemWatcher.Renamed += ConfigureAndWatchHandler_OnRenamed;
				fileSystemWatcher.EnableRaisingEvents = true;
				m_timer = new Timer(OnWatchedFileChange, null, -1, -1);
			}

			private void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e)
			{
				LogLog.Debug("ConfigureAndWatchHandler: " + e.ChangeType.ToString() + " [" + m_configFile.FullName + "]");
				m_timer.Change(500, -1);
			}

			private void ConfigureAndWatchHandler_OnRenamed(object source, RenamedEventArgs e)
			{
				LogLog.Debug("ConfigureAndWatchHandler: " + e.ChangeType.ToString() + " [" + m_configFile.FullName + "]");
				m_timer.Change(500, -1);
			}

			private void OnWatchedFileChange(object state)
			{
				Configure(m_repository, m_configFile);
			}
		}

		private XmlConfigurator()
		{
		}

		public static void Configure()
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));
		}

		public static void Configure(ILoggerRepository repository)
		{
			LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using .config file section");
			try
			{
				LogLog.Debug("XmlConfigurator: Application config file is [" + SystemInfo.ConfigurationFileLocation + "]");
			}
			catch
			{
				LogLog.Debug("XmlConfigurator: Application config file location unknown");
			}
			try
			{
				XmlElement xmlElement = null;
				xmlElement = ConfigurationSettings.GetConfig("log4net") as XmlElement;
				if (xmlElement == null)
				{
					LogLog.Error("XmlConfigurator: Failed to find configuration section 'log4net' in the application's .config file. Check your .config file for the <log4net> and <configSections> elements. The configuration section should look like: <section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,log4net\" />");
				}
				else
				{
					ConfigureFromXml(repository, xmlElement);
				}
			}
			catch (ConfigurationException ex)
			{
				if (ex.BareMessage.IndexOf("Unrecognized element") >= 0)
				{
					LogLog.Error("XmlConfigurator: Failed to parse config file. Check your .config file is well formed XML.", ex);
					return;
				}
				string text = "<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler," + Assembly.GetExecutingAssembly().FullName + "\" />";
				LogLog.Error("XmlConfigurator: Failed to parse config file. Is the <configSections> specified as: " + text, ex);
			}
		}

		public static void Configure(XmlElement element)
		{
			ConfigureFromXml(LogManager.GetRepository(Assembly.GetCallingAssembly()), element);
		}

		public static void Configure(ILoggerRepository repository, XmlElement element)
		{
			LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using XML element");
			ConfigureFromXml(repository, element);
		}

		public static void Configure(FileInfo configFile)
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configFile);
		}

		public static void Configure(Uri configUri)
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configUri);
		}

		public static void Configure(Stream configStream)
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configStream);
		}

		public static void Configure(ILoggerRepository repository, FileInfo configFile)
		{
			string[] obj = new string[5] { "XmlConfigurator: configuring repository [", repository.Name, "] using file [", null, null };
			obj[3] = ((configFile != null) ? configFile.ToString() : null);
			obj[4] = "]";
			LogLog.Debug(string.Concat(obj));
			if (configFile == null)
			{
				LogLog.Error("XmlConfigurator: Configure called with null 'configFile' parameter");
			}
			else if (File.Exists(configFile.FullName))
			{
				FileStream fileStream = null;
				int num = 5;
				while (--num >= 0)
				{
					try
					{
						fileStream = configFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
					}
					catch (IOException exception)
					{
						if (num == 0)
						{
							LogLog.Error("XmlConfigurator: Failed to open XML config file [" + configFile.Name + "]", exception);
							fileStream = null;
						}
						Thread.Sleep(250);
						continue;
					}
					break;
				}
				if (fileStream != null)
				{
					try
					{
						Configure(repository, fileStream);
					}
					finally
					{
						fileStream.Close();
					}
				}
			}
			else
			{
				LogLog.Debug("XmlConfigurator: config file [" + configFile.FullName + "] not found. Configuration unchanged.");
			}
		}

		public static void Configure(ILoggerRepository repository, Uri configUri)
		{
			string[] obj = new string[5] { "XmlConfigurator: configuring repository [", repository.Name, "] using URI [", null, null };
			obj[3] = (((object)configUri != null) ? configUri.ToString() : null);
			obj[4] = "]";
			LogLog.Debug(string.Concat(obj));
			if (configUri == null)
			{
				LogLog.Error("XmlConfigurator: Configure called with null 'configUri' parameter");
				return;
			}
			if (configUri.IsFile)
			{
				Configure(repository, new FileInfo(configUri.LocalPath));
				return;
			}
			WebRequest webRequest = null;
			try
			{
				webRequest = WebRequest.Create(configUri);
			}
			catch (Exception exception)
			{
				LogLog.Error("XmlConfigurator: Failed to create WebRequest for URI [" + (((object)configUri != null) ? configUri.ToString() : null) + "]", exception);
			}
			if (webRequest == null)
			{
				return;
			}
			try
			{
				webRequest.Credentials = CredentialCache.DefaultCredentials;
			}
			catch
			{
			}
			try
			{
				WebResponse response = webRequest.GetResponse();
				if (response == null)
				{
					return;
				}
				try
				{
					using (Stream configStream = response.GetResponseStream())
					{
						Configure(repository, configStream);
					}
				}
				finally
				{
					response.Close();
				}
			}
			catch (Exception exception2)
			{
				LogLog.Error("XmlConfigurator: Failed to request config from URI [" + (((object)configUri != null) ? configUri.ToString() : null) + "]", exception2);
			}
		}

		public static void Configure(ILoggerRepository repository, Stream configStream)
		{
			LogLog.Debug("XmlConfigurator: configuring repository [" + repository.Name + "] using stream");
			if (configStream == null)
			{
				LogLog.Error("XmlConfigurator: Configure called with null 'configStream' parameter");
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				XmlValidatingReader xmlValidatingReader = new XmlValidatingReader(new XmlTextReader(configStream));
				xmlValidatingReader.ValidationType = ValidationType.None;
				xmlValidatingReader.EntityHandling = EntityHandling.ExpandEntities;
				xmlDocument.Load(xmlValidatingReader);
			}
			catch (Exception exception)
			{
				LogLog.Error("XmlConfigurator: Error while loading XML configuration", exception);
				xmlDocument = null;
			}
			if (xmlDocument != null)
			{
				LogLog.Debug("XmlConfigurator: loading XML configuration");
				XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("log4net");
				if (elementsByTagName.Count == 0)
				{
					LogLog.Debug("XmlConfigurator: XML configuration does not contain a <log4net> element. Configuration Aborted.");
				}
				else if (elementsByTagName.Count > 1)
				{
					LogLog.Error("XmlConfigurator: XML configuration contains [" + elementsByTagName.Count + "] <log4net> elements. Only one is allowed. Configuration Aborted.");
				}
				else
				{
					ConfigureFromXml(repository, elementsByTagName[0] as XmlElement);
				}
			}
		}

		public static void ConfigureAndWatch(FileInfo configFile)
		{
			ConfigureAndWatch(LogManager.GetRepository(Assembly.GetCallingAssembly()), configFile);
		}

		public static void ConfigureAndWatch(ILoggerRepository repository, FileInfo configFile)
		{
			string[] obj = new string[5] { "XmlConfigurator: configuring repository [", repository.Name, "] using file [", null, null };
			obj[3] = ((configFile != null) ? configFile.ToString() : null);
			obj[4] = "] watching for file updates";
			LogLog.Debug(string.Concat(obj));
			if (configFile == null)
			{
				LogLog.Error("XmlConfigurator: ConfigureAndWatch called with null 'configFile' parameter");
				return;
			}
			Configure(repository, configFile);
			try
			{
				ConfigureAndWatchHandler.StartWatching(repository, configFile);
			}
			catch (Exception exception)
			{
				LogLog.Error("XmlConfigurator: Failed to initialize configuration file watcher for file [" + configFile.FullName + "]", exception);
			}
		}

		private static void ConfigureFromXml(ILoggerRepository repository, XmlElement element)
		{
			if (element == null)
			{
				LogLog.Error("XmlConfigurator: ConfigureFromXml called with null 'element' parameter");
				return;
			}
			if (repository == null)
			{
				LogLog.Error("XmlConfigurator: ConfigureFromXml called with null 'repository' parameter");
				return;
			}
			LogLog.Debug("XmlConfigurator: Configuring Repository [" + repository.Name + "]");
			IXmlRepositoryConfigurator xmlRepositoryConfigurator = repository as IXmlRepositoryConfigurator;
			if (xmlRepositoryConfigurator == null)
			{
				LogLog.Warn("XmlConfigurator: Repository [" + ((repository != null) ? repository.ToString() : null) + "] does not support the XmlConfigurator");
				return;
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement element2 = (XmlElement)xmlDocument.AppendChild(xmlDocument.ImportNode(element, true));
			xmlRepositoryConfigurator.Configure(element2);
		}
	}
}
