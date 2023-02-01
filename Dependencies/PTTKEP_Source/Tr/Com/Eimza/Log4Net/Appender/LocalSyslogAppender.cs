using System;
using System.Runtime.InteropServices;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class LocalSyslogAppender : AppenderSkeleton
	{
		public enum SyslogSeverity
		{
			Emergency,
			Alert,
			Critical,
			Error,
			Warning,
			Notice,
			Informational,
			Debug
		}

		public enum SyslogFacility
		{
			Kernel,
			User,
			Mail,
			Daemons,
			Authorization,
			Syslog,
			Printer,
			News,
			Uucp,
			Clock,
			Authorization2,
			Ftp,
			Ntp,
			Audit,
			Alert,
			Clock2,
			Local0,
			Local1,
			Local2,
			Local3,
			Local4,
			Local5,
			Local6,
			Local7
		}

		public class LevelSeverity : LevelMappingEntry
		{
			private SyslogSeverity m_severity;

			public SyslogSeverity Severity
			{
				get
				{
					return m_severity;
				}
				set
				{
					m_severity = value;
				}
			}
		}

		private SyslogFacility m_facility = SyslogFacility.User;

		private string m_identity;

		private IntPtr m_handleToIdentity = IntPtr.Zero;

		private LevelMapping m_levelMapping = new LevelMapping();

		public string Identity
		{
			get
			{
				return m_identity;
			}
			set
			{
				m_identity = value;
			}
		}

		public SyslogFacility Facility
		{
			get
			{
				return m_facility;
			}
			set
			{
				m_facility = value;
			}
		}

		protected override bool RequiresLayout
		{
			get
			{
				return true;
			}
		}

		public void AddMapping(LevelSeverity mapping)
		{
			m_levelMapping.Add(mapping);
		}

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			m_levelMapping.ActivateOptions();
			string text = m_identity;
			if (text == null)
			{
				text = SystemInfo.ApplicationFriendlyName;
			}
			m_handleToIdentity = Marshal.StringToHGlobalAnsi(text);
			openlog(m_handleToIdentity, 1, m_facility);
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			int priority = GeneratePriority(m_facility, GetSeverity(loggingEvent.Level));
			string message = RenderLoggingEvent(loggingEvent);
			syslog(priority, "%s", message);
		}

		protected override void OnClose()
		{
			base.OnClose();
			try
			{
				closelog();
			}
			catch (DllNotFoundException)
			{
			}
			if (m_handleToIdentity != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(m_handleToIdentity);
			}
		}

		protected virtual SyslogSeverity GetSeverity(Level level)
		{
			LevelSeverity levelSeverity = m_levelMapping.Lookup(level) as LevelSeverity;
			if (levelSeverity != null)
			{
				return levelSeverity.Severity;
			}
			if (level >= Level.Alert)
			{
				return SyslogSeverity.Alert;
			}
			if (level >= Level.Critical)
			{
				return SyslogSeverity.Critical;
			}
			if (level >= Level.Error)
			{
				return SyslogSeverity.Error;
			}
			if (level >= Level.Warn)
			{
				return SyslogSeverity.Warning;
			}
			if (level >= Level.Notice)
			{
				return SyslogSeverity.Notice;
			}
			if (level >= Level.Info)
			{
				return SyslogSeverity.Informational;
			}
			return SyslogSeverity.Debug;
		}

		private static int GeneratePriority(SyslogFacility facility, SyslogSeverity severity)
		{
			return (int)((int)facility * 8 + severity);
		}

		[DllImport("libc")]
		private static extern void openlog(IntPtr ident, int option, SyslogFacility facility);

		[DllImport("libc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		private static extern void syslog(int priority, string format, string message);

		[DllImport("libc")]
		private static extern void closelog();
	}
}
