using System;
using System.IO;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class SmtpPickupDirAppender : BufferingAppenderSkeleton
	{
		private string m_to;

		private string m_from;

		private string m_subject;

		private string m_pickupDir;

		private SecurityContext m_securityContext;

		public string To
		{
			get
			{
				return m_to;
			}
			set
			{
				m_to = value;
			}
		}

		public string From
		{
			get
			{
				return m_from;
			}
			set
			{
				m_from = value;
			}
		}

		public string Subject
		{
			get
			{
				return m_subject;
			}
			set
			{
				m_subject = value;
			}
		}

		public string PickupDir
		{
			get
			{
				return m_pickupDir;
			}
			set
			{
				m_pickupDir = value;
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

		protected override void SendBuffer(LoggingEvent[] events)
		{
			try
			{
				string text = null;
				StreamWriter streamWriter = null;
				using (SecurityContext.Impersonate(this))
				{
					text = Path.Combine(m_pickupDir, SystemInfo.NewGuid().ToString("N"));
					streamWriter = File.CreateText(text);
				}
				if (streamWriter == null)
				{
					ErrorHandler.Error("Failed to create output file for writing [" + text + "]", null, ErrorCode.FileOpenFailure);
					return;
				}
				using (streamWriter)
				{
					streamWriter.WriteLine("To: " + m_to);
					streamWriter.WriteLine("From: " + m_from);
					streamWriter.WriteLine("Subject: " + m_subject);
					streamWriter.WriteLine("");
					string header = Layout.Header;
					if (header != null)
					{
						streamWriter.Write(header);
					}
					for (int i = 0; i < events.Length; i++)
					{
						RenderLoggingEvent(streamWriter, events[i]);
					}
					header = Layout.Footer;
					if (header != null)
					{
						streamWriter.Write(header);
					}
					streamWriter.WriteLine("");
					streamWriter.WriteLine(".");
				}
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Error occurred while sending e-mail notification.", e);
			}
		}

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			if (m_securityContext == null)
			{
				m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
			}
			using (SecurityContext.Impersonate(this))
			{
				m_pickupDir = ConvertToFullPath(m_pickupDir.Trim());
			}
		}

		protected static string ConvertToFullPath(string path)
		{
			return SystemInfo.ConvertToFullPath(path);
		}
	}
}
