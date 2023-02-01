using System;
using System.Diagnostics;
using System.Threading;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Layout;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class EventLogAppender : AppenderSkeleton
	{
		public class Level2EventLogEntryType : LevelMappingEntry
		{
			private EventLogEntryType m_entryType;

			public EventLogEntryType EventLogEntryType
			{
				get
				{
					return m_entryType;
				}
				set
				{
					m_entryType = value;
				}
			}
		}

		private string m_logName;

		private string m_applicationName;

		private string m_machineName;

		private LevelMapping m_levelMapping = new LevelMapping();

		private SecurityContext m_securityContext;

		public string LogName
		{
			get
			{
				return m_logName;
			}
			set
			{
				m_logName = value;
			}
		}

		public string ApplicationName
		{
			get
			{
				return m_applicationName;
			}
			set
			{
				m_applicationName = value;
			}
		}

		public string MachineName
		{
			get
			{
				return m_machineName;
			}
			set
			{
			}
		}

		public SecurityContext SecurityContext
		{
			get
			{
				return m_securityContext;
			}
			set
			{
				m_securityContext = value;
			}
		}

		protected override bool RequiresLayout
		{
			get
			{
				return true;
			}
		}

		public EventLogAppender()
		{
			m_applicationName = Thread.GetDomain().FriendlyName;
			m_logName = "Application";
			m_machineName = ".";
		}

		[Obsolete("Instead use the default constructor and set the Layout property")]
		public EventLogAppender(ILayout layout)
			: this()
		{
			Layout = layout;
		}

		public void AddMapping(Level2EventLogEntryType mapping)
		{
			m_levelMapping.Add(mapping);
		}

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			if (m_securityContext == null)
			{
				m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
			}
			bool flag = false;
			string text = null;
			using (SecurityContext.Impersonate(this))
			{
				flag = EventLog.SourceExists(m_applicationName);
				if (flag)
				{
					text = EventLog.LogNameFromSourceName(m_applicationName, m_machineName);
				}
			}
			if (flag && text != m_logName)
			{
				LogLog.Debug("EventLogAppender: Changing event source [" + m_applicationName + "] from log [" + text + "] to log [" + m_logName + "]");
			}
			else if (!flag)
			{
				LogLog.Debug("EventLogAppender: Creating event source Source [" + m_applicationName + "] in log " + m_logName + "]");
			}
			string text2 = null;
			using (SecurityContext.Impersonate(this))
			{
				if (flag && text != m_logName)
				{
					EventLog.DeleteEventSource(m_applicationName, m_machineName);
					CreateEventSource(m_applicationName, m_logName, m_machineName);
					text2 = EventLog.LogNameFromSourceName(m_applicationName, m_machineName);
				}
				else if (!flag)
				{
					CreateEventSource(m_applicationName, m_logName, m_machineName);
					text2 = EventLog.LogNameFromSourceName(m_applicationName, m_machineName);
				}
			}
			m_levelMapping.ActivateOptions();
			LogLog.Debug("EventLogAppender: Source [" + m_applicationName + "] is registered to log [" + text2 + "]");
		}

		private static void CreateEventSource(string source, string logName, string machineName)
		{
			EventLog.CreateEventSource(source, logName, machineName);
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			int eventID = 0;
			object obj = loggingEvent.LookupProperty("EventID");
			if (obj != null)
			{
				if (obj is int)
				{
					eventID = (int)obj;
				}
				else
				{
					string text = obj as string;
					if (text != null && text.Length > 0)
					{
						int val;
						if (SystemInfo.TryParse(text, out val))
						{
							eventID = val;
						}
						else
						{
							ErrorHandler.Error("Unable to parse event ID property [" + text + "].");
						}
					}
				}
			}
			try
			{
				string text2 = RenderLoggingEvent(loggingEvent);
				if (text2.Length > 32000)
				{
					text2 = text2.Substring(0, 32000);
				}
				EventLogEntryType entryType = GetEntryType(loggingEvent.Level);
				using (SecurityContext.Impersonate(this))
				{
					EventLog.WriteEntry(m_applicationName, text2, entryType, eventID);
				}
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Unable to write to event log [" + m_logName + "] using source [" + m_applicationName + "]", e);
			}
		}

		protected virtual EventLogEntryType GetEntryType(Level level)
		{
			Level2EventLogEntryType level2EventLogEntryType = m_levelMapping.Lookup(level) as Level2EventLogEntryType;
			if (level2EventLogEntryType != null)
			{
				return level2EventLogEntryType.EventLogEntryType;
			}
			if (level >= Level.Error)
			{
				return EventLogEntryType.Error;
			}
			if (level == Level.Warn)
			{
				return EventLogEntryType.Warning;
			}
			return EventLogEntryType.Information;
		}
	}
}
