using System;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class BufferingForwardingAppender : BufferingAppenderSkeleton, IAppenderAttachable
	{
		private AppenderAttachedImpl m_appenderAttachedImpl;

		public virtual AppenderCollection Appenders
		{
			get
			{
				lock (this)
				{
					if (m_appenderAttachedImpl == null)
					{
						return AppenderCollection.EmptyCollection;
					}
					return m_appenderAttachedImpl.Appenders;
				}
			}
		}

		protected override void OnClose()
		{
			lock (this)
			{
				base.OnClose();
				if (m_appenderAttachedImpl != null)
				{
					m_appenderAttachedImpl.RemoveAllAppenders();
				}
			}
		}

		protected override void SendBuffer(LoggingEvent[] events)
		{
			if (m_appenderAttachedImpl != null)
			{
				m_appenderAttachedImpl.AppendLoopOnAppenders(events);
			}
		}

		public virtual void AddAppender(IAppender newAppender)
		{
			if (newAppender == null)
			{
				throw new ArgumentNullException("newAppender");
			}
			lock (this)
			{
				if (m_appenderAttachedImpl == null)
				{
					m_appenderAttachedImpl = new AppenderAttachedImpl();
				}
				m_appenderAttachedImpl.AddAppender(newAppender);
			}
		}

		public virtual IAppender GetAppender(string name)
		{
			lock (this)
			{
				if (m_appenderAttachedImpl == null || name == null)
				{
					return null;
				}
				return m_appenderAttachedImpl.GetAppender(name);
			}
		}

		public virtual void RemoveAllAppenders()
		{
			lock (this)
			{
				if (m_appenderAttachedImpl != null)
				{
					m_appenderAttachedImpl.RemoveAllAppenders();
					m_appenderAttachedImpl = null;
				}
			}
		}

		public virtual IAppender RemoveAppender(IAppender appender)
		{
			lock (this)
			{
				if (appender != null && m_appenderAttachedImpl != null)
				{
					return m_appenderAttachedImpl.RemoveAppender(appender);
				}
			}
			return null;
		}

		public virtual IAppender RemoveAppender(string name)
		{
			lock (this)
			{
				if (name != null && m_appenderAttachedImpl != null)
				{
					return m_appenderAttachedImpl.RemoveAppender(name);
				}
			}
			return null;
		}
	}
}
