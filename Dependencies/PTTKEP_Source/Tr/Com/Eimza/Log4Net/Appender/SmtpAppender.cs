using System;
using System.Globalization;
using System.IO;
using System.Web.Mail;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class SmtpAppender : BufferingAppenderSkeleton
	{
		public enum SmtpAuthentication
		{
			None,
			Basic,
			Ntlm
		}

		private string m_to;

		private string m_from;

		private string m_subject;

		private string m_smtpHost;

		private SmtpAuthentication m_authentication;

		private string m_username;

		private string m_password;

		private int m_port = 25;

		private MailPriority m_mailPriority;

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

		public string SmtpHost
		{
			get
			{
				return m_smtpHost;
			}
			set
			{
				m_smtpHost = value;
			}
		}

		[Obsolete("Use the BufferingAppenderSkeleton Fix methods")]
		public bool LocationInfo
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public SmtpAuthentication Authentication
		{
			get
			{
				return m_authentication;
			}
			set
			{
				m_authentication = value;
			}
		}

		public string Username
		{
			get
			{
				return m_username;
			}
			set
			{
				m_username = value;
			}
		}

		public string Password
		{
			get
			{
				return m_password;
			}
			set
			{
				m_password = value;
			}
		}

		public int Port
		{
			get
			{
				return m_port;
			}
			set
			{
				m_port = value;
			}
		}

		public MailPriority Priority
		{
			get
			{
				return m_mailPriority;
			}
			set
			{
				m_mailPriority = value;
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
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				string header = Layout.Header;
				if (header != null)
				{
					stringWriter.Write(header);
				}
				for (int i = 0; i < events.Length; i++)
				{
					RenderLoggingEvent(stringWriter, events[i]);
				}
				header = Layout.Footer;
				if (header != null)
				{
					stringWriter.Write(header);
				}
				SendEmail(stringWriter.ToString());
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Error occurred while sending e-mail notification.", e);
			}
		}

		protected virtual void SendEmail(string messageBody)
		{
			MailMessage message = new MailMessage
			{
				Body = messageBody,
				From = m_from,
				To = m_to,
				Subject = m_subject,
				Priority = m_mailPriority
			};
			if (m_authentication != 0)
			{
				ErrorHandler.Error("SmtpAppender: Authentication is only supported on the MS .NET 1.1 or MS .NET 2.0 builds of log4net");
			}
			if (m_port != 25)
			{
				ErrorHandler.Error("SmtpAppender: Server Port is only supported on the MS .NET 1.1 or MS .NET 2.0 builds of log4net");
			}
			if (m_smtpHost != null && m_smtpHost.Length > 0)
			{
				SmtpMail.SmtpServer = m_smtpHost;
			}
			SmtpMail.Send(message);
		}
	}
}
