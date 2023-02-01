using System;
using System.Runtime.Remoting;
using Tr.Com.Eimza.Log4Net.Appender;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Repository;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Plugin
{
	public class RemoteLoggingServerPlugin : PluginSkeleton
	{
		private class RemoteLoggingSinkImpl : MarshalByRefObject, RemotingAppender.IRemoteLoggingSink
		{
			private readonly ILoggerRepository m_repository;

			public RemoteLoggingSinkImpl(ILoggerRepository repository)
			{
				m_repository = repository;
			}

			public void LogEvents(LoggingEvent[] events)
			{
				if (events == null)
				{
					return;
				}
				foreach (LoggingEvent loggingEvent in events)
				{
					if (loggingEvent != null)
					{
						m_repository.Log(loggingEvent);
					}
				}
			}

			public override object InitializeLifetimeService()
			{
				return null;
			}
		}

		private RemoteLoggingSinkImpl m_sink;

		private string m_sinkUri;

		public virtual string SinkUri
		{
			get
			{
				return m_sinkUri;
			}
			set
			{
				m_sinkUri = value;
			}
		}

		public RemoteLoggingServerPlugin()
			: base("RemoteLoggingServerPlugin:Unset URI")
		{
		}

		public RemoteLoggingServerPlugin(string sinkUri)
			: base("RemoteLoggingServerPlugin:" + sinkUri)
		{
			m_sinkUri = sinkUri;
		}

		public override void Attach(ILoggerRepository repository)
		{
			base.Attach(repository);
			m_sink = new RemoteLoggingSinkImpl(repository);
			try
			{
				RemotingServices.Marshal(m_sink, m_sinkUri, typeof(RemotingAppender.IRemoteLoggingSink));
			}
			catch (Exception exception)
			{
				LogLog.Error("RemoteLoggingServerPlugin: Failed to Marshal remoting sink", exception);
			}
		}

		public override void Shutdown()
		{
			RemotingServices.Disconnect(m_sink);
			m_sink = null;
			base.Shutdown();
		}
	}
}
