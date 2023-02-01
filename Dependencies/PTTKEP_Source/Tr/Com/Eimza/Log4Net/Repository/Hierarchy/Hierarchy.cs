using System;
using System.Collections;
using System.Xml;
using Tr.Com.Eimza.Log4Net.Appender;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Repository.Hierarchy
{
	public class Hierarchy : LoggerRepositorySkeleton, IBasicRepositoryConfigurator, IXmlRepositoryConfigurator
	{
		internal class LevelEntry
		{
			private int m_levelValue = -1;

			private string m_levelName;

			private string m_levelDisplayName;

			public int Value
			{
				get
				{
					return m_levelValue;
				}
				set
				{
					m_levelValue = value;
				}
			}

			public string Name
			{
				get
				{
					return m_levelName;
				}
				set
				{
					m_levelName = value;
				}
			}

			public string DisplayName
			{
				get
				{
					return m_levelDisplayName;
				}
				set
				{
					m_levelDisplayName = value;
				}
			}

			public override string ToString()
			{
				return "LevelEntry(Value=" + m_levelValue + ", Name=" + m_levelName + ", DisplayName=" + m_levelDisplayName + ")";
			}
		}

		internal class PropertyEntry
		{
			private string m_key;

			private object m_value;

			public string Key
			{
				get
				{
					return m_key;
				}
				set
				{
					m_key = value;
				}
			}

			public object Value
			{
				get
				{
					return m_value;
				}
				set
				{
					m_value = value;
				}
			}

			public override string ToString()
			{
				string[] obj = new string[5] { "PropertyEntry(Key=", m_key, ", Value=", null, null };
				object value = m_value;
				obj[3] = ((value != null) ? value.ToString() : null);
				obj[4] = ")";
				return string.Concat(obj);
			}
		}

		private ILoggerFactory m_defaultFactory;

		private Hashtable m_ht;

		private Logger m_root;

		private bool m_emittedNoAppenderWarning;

		public bool EmittedNoAppenderWarning
		{
			get
			{
				return m_emittedNoAppenderWarning;
			}
			set
			{
				m_emittedNoAppenderWarning = value;
			}
		}

		public Logger Root
		{
			get
			{
				if (m_root == null)
				{
					lock (this)
					{
						if (m_root == null)
						{
							Logger logger = m_defaultFactory.CreateLogger(null);
							logger.Hierarchy = this;
							m_root = logger;
						}
					}
				}
				return m_root;
			}
		}

		public ILoggerFactory LoggerFactory
		{
			get
			{
				return m_defaultFactory;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				m_defaultFactory = value;
			}
		}

		public event LoggerCreationEventHandler LoggerCreatedEvent
		{
			add
			{
				m_loggerCreatedEvent += value;
			}
			remove
			{
				m_loggerCreatedEvent -= value;
			}
		}

		private event LoggerCreationEventHandler m_loggerCreatedEvent;

		public Hierarchy()
			: this(new DefaultLoggerFactory())
		{
		}

		public Hierarchy(PropertiesDictionary properties)
			: this(properties, new DefaultLoggerFactory())
		{
		}

		public Hierarchy(ILoggerFactory loggerFactory)
			: this(new PropertiesDictionary(), loggerFactory)
		{
		}

		public Hierarchy(PropertiesDictionary properties, ILoggerFactory loggerFactory)
			: base(properties)
		{
			if (loggerFactory == null)
			{
				throw new ArgumentNullException("loggerFactory");
			}
			m_defaultFactory = loggerFactory;
			m_ht = Hashtable.Synchronized(new Hashtable());
		}

		public override ILogger Exists(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return m_ht[new LoggerKey(name)] as Logger;
		}

		public override ILogger[] GetCurrentLoggers()
		{
			ArrayList arrayList = new ArrayList(m_ht.Count);
			foreach (object value in m_ht.Values)
			{
				if (value is Logger)
				{
					arrayList.Add(value);
				}
			}
			return (Logger[])arrayList.ToArray(typeof(Logger));
		}

		public override ILogger GetLogger(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return GetLogger(name, m_defaultFactory);
		}

		public override void Shutdown()
		{
			LogLog.Debug("Hierarchy: Shutdown called on Hierarchy [" + Name + "]");
			Root.CloseNestedAppenders();
			lock (m_ht)
			{
				ILogger[] currentLoggers = GetCurrentLoggers();
				ILogger[] array = currentLoggers;
				for (int i = 0; i < array.Length; i++)
				{
					((Logger)array[i]).CloseNestedAppenders();
				}
				Root.RemoveAllAppenders();
				array = currentLoggers;
				for (int i = 0; i < array.Length; i++)
				{
					((Logger)array[i]).RemoveAllAppenders();
				}
			}
			base.Shutdown();
		}

		public override void ResetConfiguration()
		{
			Root.Level = Level.Debug;
			Threshold = Level.All;
			lock (m_ht)
			{
				Shutdown();
				ILogger[] currentLoggers = GetCurrentLoggers();
				for (int i = 0; i < currentLoggers.Length; i++)
				{
					Logger obj = (Logger)currentLoggers[i];
					obj.Level = null;
					obj.Additivity = true;
				}
			}
			base.ResetConfiguration();
			OnConfigurationChanged(null);
		}

		public override void Log(LoggingEvent logEvent)
		{
			if (logEvent == null)
			{
				throw new ArgumentNullException("logEvent");
			}
			GetLogger(logEvent.LoggerName, m_defaultFactory).Log(logEvent);
		}

		public override IAppender[] GetAppenders()
		{
			ArrayList arrayList = new ArrayList();
			CollectAppenders(arrayList, Root);
			ILogger[] currentLoggers = GetCurrentLoggers();
			for (int i = 0; i < currentLoggers.Length; i++)
			{
				Logger container = (Logger)currentLoggers[i];
				CollectAppenders(arrayList, container);
			}
			return (IAppender[])arrayList.ToArray(typeof(IAppender));
		}

		private static void CollectAppender(ArrayList appenderList, IAppender appender)
		{
			if (!appenderList.Contains(appender))
			{
				appenderList.Add(appender);
				IAppenderAttachable appenderAttachable = appender as IAppenderAttachable;
				if (appenderAttachable != null)
				{
					CollectAppenders(appenderList, appenderAttachable);
				}
			}
		}

		private static void CollectAppenders(ArrayList appenderList, IAppenderAttachable container)
		{
			AppenderCollection.IAppenderCollectionEnumerator enumerator = container.Appenders.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IAppender current = enumerator.Current;
					CollectAppender(appenderList, current);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		void IBasicRepositoryConfigurator.Configure(IAppender appender)
		{
			BasicRepositoryConfigure(appender);
		}

		protected void BasicRepositoryConfigure(IAppender appender)
		{
			Root.AddAppender(appender);
			Configured = true;
			OnConfigurationChanged(null);
		}

		void IXmlRepositoryConfigurator.Configure(XmlElement element)
		{
			XmlRepositoryConfigure(element);
		}

		protected void XmlRepositoryConfigure(XmlElement element)
		{
			new XmlHierarchyConfigurator(this).Configure(element);
			Configured = true;
			OnConfigurationChanged(null);
		}

		public bool IsDisabled(Level level)
		{
			if ((object)level == null)
			{
				throw new ArgumentNullException("level");
			}
			if (Configured)
			{
				return Threshold > level;
			}
			return true;
		}

		public void Clear()
		{
			m_ht.Clear();
		}

		public Logger GetLogger(string name, ILoggerFactory factory)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			LoggerKey key = new LoggerKey(name);
			lock (m_ht)
			{
				object obj = m_ht[key];
				if (obj == null)
				{
					Logger logger = factory.CreateLogger(name);
					logger.Hierarchy = this;
					m_ht[key] = logger;
					UpdateParents(logger);
					OnLoggerCreationEvent(logger);
					return logger;
				}
				Logger logger2 = obj as Logger;
				if (logger2 != null)
				{
					return logger2;
				}
				ProvisionNode provisionNode = obj as ProvisionNode;
				if (provisionNode != null)
				{
					Logger logger = factory.CreateLogger(name);
					logger.Hierarchy = this;
					m_ht[key] = logger;
					UpdateChildren(provisionNode, logger);
					UpdateParents(logger);
					OnLoggerCreationEvent(logger);
					return logger;
				}
				return null;
			}
		}

		protected virtual void OnLoggerCreationEvent(Logger logger)
		{
			LoggerCreationEventHandler loggerCreatedEvent = this.m_loggerCreatedEvent;
			if (loggerCreatedEvent != null)
			{
				loggerCreatedEvent(this, new LoggerCreationEventArgs(logger));
			}
		}

		private void UpdateParents(Logger log)
		{
			string name = log.Name;
			int length = name.Length;
			bool flag = false;
			for (int num = name.LastIndexOf('.', length - 1); num >= 0; num = name.LastIndexOf('.', num - 1))
			{
				LoggerKey key = new LoggerKey(name.Substring(0, num));
				object obj = m_ht[key];
				if (obj == null)
				{
					ProvisionNode value = new ProvisionNode(log);
					m_ht[key] = value;
				}
				else
				{
					Logger logger = obj as Logger;
					if (logger != null)
					{
						flag = true;
						log.Parent = logger;
						break;
					}
					ProvisionNode provisionNode = obj as ProvisionNode;
					if (provisionNode != null)
					{
						provisionNode.Add(log);
					}
					else
					{
						Type type = obj.GetType();
						LogLog.Error("Hierarchy: Unexpected object type [" + (((object)type != null) ? type.ToString() : null) + "] in ht.", new LogException());
					}
				}
			}
			if (!flag)
			{
				log.Parent = Root;
			}
		}

		private void UpdateChildren(ProvisionNode pn, Logger log)
		{
			for (int i = 0; i < pn.Count; i++)
			{
				Logger logger = (Logger)pn[i];
				if (!logger.Parent.Name.StartsWith(log.Name))
				{
					log.Parent = logger.Parent;
					logger.Parent = log;
				}
			}
		}

		internal void AddLevel(LevelEntry levelEntry)
		{
			if (levelEntry == null)
			{
				throw new ArgumentNullException("levelEntry");
			}
			if (levelEntry.Name == null)
			{
				throw new ArgumentNullException("levelEntry.Name");
			}
			if (levelEntry.Value == -1)
			{
				Level level = LevelMap[levelEntry.Name];
				if (level == null)
				{
					throw new InvalidOperationException("Cannot redefine level [" + levelEntry.Name + "] because it is not defined in the LevelMap. To define the level supply the level value.");
				}
				levelEntry.Value = level.Value;
			}
			LevelMap.Add(levelEntry.Name, levelEntry.Value, levelEntry.DisplayName);
		}

		internal void AddProperty(PropertyEntry propertyEntry)
		{
			if (propertyEntry == null)
			{
				throw new ArgumentNullException("propertyEntry");
			}
			if (propertyEntry.Key == null)
			{
				throw new ArgumentNullException("propertyEntry.Key");
			}
			base.Properties[propertyEntry.Key] = propertyEntry.Value;
		}
	}
}
