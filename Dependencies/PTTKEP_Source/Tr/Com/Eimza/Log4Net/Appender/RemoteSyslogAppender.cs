using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Layout;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class RemoteSyslogAppender : UdpAppender
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

		private const int DefaultSyslogPort = 514;

		private SyslogFacility m_facility = SyslogFacility.User;

		private PatternLayout m_identity;

		private LevelMapping m_levelMapping = new LevelMapping();

		public PatternLayout Identity
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

		public RemoteSyslogAppender()
		{
			base.RemotePort = 514;
			base.RemoteAddress = IPAddress.Parse("127.0.0.1");
			base.Encoding = Encoding.ASCII;
		}

		public void AddMapping(LevelSeverity mapping)
		{
			m_levelMapping.Add(mapping);
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			try
			{
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				int value = GeneratePriority(m_facility, GetSeverity(loggingEvent.Level));
				stringWriter.Write('<');
				stringWriter.Write(value);
				stringWriter.Write('>');
				if (m_identity != null)
				{
					m_identity.Format(stringWriter, loggingEvent);
				}
				else
				{
					stringWriter.Write(loggingEvent.Domain);
				}
				stringWriter.Write(": ");
				RenderLoggingEvent(stringWriter, loggingEvent);
				string text = stringWriter.ToString();
				byte[] bytes = base.Encoding.GetBytes(text.ToCharArray());
				base.Client.Send(bytes, bytes.Length, base.RemoteEndPoint);
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Unable to send logging event to remote syslog " + base.RemoteAddress.ToString() + " on port " + base.RemotePort + ".", e, ErrorCode.WriteFailure);
			}
		}

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			m_levelMapping.ActivateOptions();
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

		public static int GeneratePriority(SyslogFacility facility, SyslogSeverity severity)
		{
			if (facility < SyslogFacility.Kernel || facility > SyslogFacility.Local7)
			{
				throw new ArgumentException("SyslogFacility out of range", "facility");
			}
			if (severity < SyslogSeverity.Emergency || severity > SyslogSeverity.Debug)
			{
				throw new ArgumentException("SyslogSeverity out of range", "severity");
			}
			return (int)((int)facility * 8 + severity);
		}
	}
}
