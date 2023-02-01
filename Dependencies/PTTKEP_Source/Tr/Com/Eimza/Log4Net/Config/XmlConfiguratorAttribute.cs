using System;
using System.IO;
using System.Reflection;
using Tr.Com.Eimza.Log4Net.Repository;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Config
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly)]
	public class XmlConfiguratorAttribute : ConfiguratorAttribute
	{
		private string m_configFile;

		private string m_configFileExtension;

		private bool m_configureAndWatch;

		public string ConfigFile
		{
			get
			{
				return m_configFile;
			}
			set
			{
				m_configFile = value;
			}
		}

		public string ConfigFileExtension
		{
			get
			{
				return m_configFileExtension;
			}
			set
			{
				m_configFileExtension = value;
			}
		}

		public bool Watch
		{
			get
			{
				return m_configureAndWatch;
			}
			set
			{
				m_configureAndWatch = value;
			}
		}

		public XmlConfiguratorAttribute()
			: base(0)
		{
		}

		public override void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository)
		{
			string text = null;
			try
			{
				text = SystemInfo.ApplicationBaseDirectory;
			}
			catch
			{
			}
			if (text == null || new Uri(text).IsFile)
			{
				ConfigureFromFile(sourceAssembly, targetRepository);
			}
			else
			{
				ConfigureFromUri(sourceAssembly, targetRepository);
			}
		}

		private void ConfigureFromFile(Assembly sourceAssembly, ILoggerRepository targetRepository)
		{
			string text = null;
			if (m_configFile == null || m_configFile.Length == 0)
			{
				if (m_configFileExtension == null || m_configFileExtension.Length == 0)
				{
					try
					{
						text = SystemInfo.ConfigurationFileLocation;
					}
					catch (Exception exception)
					{
						LogLog.Error("XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when ConfigFile and ConfigFileExtension properties are not set.", exception);
					}
				}
				else
				{
					if (m_configFileExtension[0] != '.')
					{
						m_configFileExtension = "." + m_configFileExtension;
					}
					string text2 = null;
					try
					{
						text2 = SystemInfo.ApplicationBaseDirectory;
					}
					catch (Exception exception2)
					{
						LogLog.Error("XmlConfiguratorAttribute: Exception getting ApplicationBaseDirectory. Must be able to resolve ApplicationBaseDirectory and AssemblyFileName when ConfigFileExtension property is set.", exception2);
					}
					if (text2 != null)
					{
						text = Path.Combine(text2, SystemInfo.AssemblyFileName(sourceAssembly) + m_configFileExtension);
					}
				}
			}
			else
			{
				string text3 = null;
				try
				{
					text3 = SystemInfo.ApplicationBaseDirectory;
				}
				catch (Exception exception3)
				{
					LogLog.Warn("XmlConfiguratorAttribute: Exception getting ApplicationBaseDirectory. ConfigFile property path [" + m_configFile + "] will be treated as an absolute path.", exception3);
				}
				text = ((text3 == null) ? m_configFile : Path.Combine(text3, m_configFile));
			}
			if (text != null)
			{
				ConfigureFromFile(targetRepository, new FileInfo(text));
			}
		}

		private void ConfigureFromFile(ILoggerRepository targetRepository, FileInfo configFile)
		{
			if (m_configureAndWatch)
			{
				XmlConfigurator.ConfigureAndWatch(targetRepository, configFile);
			}
			else
			{
				XmlConfigurator.Configure(targetRepository, configFile);
			}
		}

		private void ConfigureFromUri(Assembly sourceAssembly, ILoggerRepository targetRepository)
		{
			Uri uri = null;
			if (m_configFile == null || m_configFile.Length == 0)
			{
				if (m_configFileExtension == null || m_configFileExtension.Length == 0)
				{
					string text = null;
					try
					{
						text = SystemInfo.ConfigurationFileLocation;
					}
					catch (Exception exception)
					{
						LogLog.Error("XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when ConfigFile and ConfigFileExtension properties are not set.", exception);
					}
					if (text != null)
					{
						uri = new Uri(text);
					}
				}
				else
				{
					if (m_configFileExtension[0] != '.')
					{
						m_configFileExtension = "." + m_configFileExtension;
					}
					string text2 = null;
					try
					{
						text2 = SystemInfo.ConfigurationFileLocation;
					}
					catch (Exception exception2)
					{
						LogLog.Error("XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when the ConfigFile property are not set.", exception2);
					}
					if (text2 != null)
					{
						UriBuilder uriBuilder = new UriBuilder(new Uri(text2));
						string text3 = uriBuilder.Path;
						int num = text3.LastIndexOf(".");
						if (num >= 0)
						{
							text3 = text3.Substring(0, num);
						}
						text3 += m_configFileExtension;
						uriBuilder.Path = text3;
						uri = uriBuilder.Uri;
					}
				}
			}
			else
			{
				string text4 = null;
				try
				{
					text4 = SystemInfo.ApplicationBaseDirectory;
				}
				catch (Exception exception3)
				{
					LogLog.Warn("XmlConfiguratorAttribute: Exception getting ApplicationBaseDirectory. ConfigFile property path [" + m_configFile + "] will be treated as an absolute URI.", exception3);
				}
				uri = ((text4 == null) ? new Uri(m_configFile) : new Uri(new Uri(text4), m_configFile));
			}
			if (!(uri != null))
			{
				return;
			}
			if (uri.IsFile)
			{
				ConfigureFromFile(targetRepository, new FileInfo(uri.LocalPath));
				return;
			}
			if (m_configureAndWatch)
			{
				LogLog.Warn("XmlConfiguratorAttribute: Unable to watch config file loaded from a URI");
			}
			XmlConfigurator.Configure(targetRepository, uri);
		}
	}
}
