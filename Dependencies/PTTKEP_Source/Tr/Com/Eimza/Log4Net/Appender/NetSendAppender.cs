using System;
using System.Runtime.InteropServices;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class NetSendAppender : AppenderSkeleton
	{
		private string m_server;

		private string m_sender;

		private string m_recipient;

		private SecurityContext m_securityContext;

		public string Sender
		{
			get
			{
				return m_sender;
			}
			set
			{
				m_sender = value;
			}
		}

		public string Recipient
		{
			get
			{
				return m_recipient;
			}
			set
			{
				m_recipient = value;
			}
		}

		public string Server
		{
			get
			{
				return m_server;
			}
			set
			{
				m_server = value;
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

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			if (Recipient == null)
			{
				throw new ArgumentNullException("Recipient", "The required property 'Recipient' was not specified.");
			}
			if (m_securityContext == null)
			{
				m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
			}
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			NativeError nativeError = null;
			string text = RenderLoggingEvent(loggingEvent);
			using (m_securityContext.Impersonate(this))
			{
				int num = NetMessageBufferSend(Server, Recipient, Sender, text, text.Length * Marshal.SystemDefaultCharSize);
				if (num != 0)
				{
					nativeError = NativeError.GetError(num);
				}
			}
			if (nativeError != null)
			{
				ErrorHandler.Error(nativeError.ToString() + " (Params: Server=" + Server + ", Recipient=" + Recipient + ", Sender=" + Sender + ")");
			}
		}

		[DllImport("netapi32.dll", SetLastError = true)]
		protected static extern int NetMessageBufferSend([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string msgName, [MarshalAs(UnmanagedType.LPWStr)] string fromName, [MarshalAs(UnmanagedType.LPWStr)] string buffer, int bufferSize);
	}
}
