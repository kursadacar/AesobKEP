using System;
using System.Security;
using Tr.Com.Eimza.Log4Net.Appender;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Repository.Hierarchy
{
	public abstract class Logger : IAppenderAttachable, ILogger
	{
		private static readonly Type ThisDeclaringType = typeof(Logger);

		private readonly string m_name;

		private Level m_level;

		private Logger m_parent;

		private Hierarchy m_hierarchy;

		private AppenderAttachedImpl m_appenderAttachedImpl;

		private bool m_additive = true;

		private readonly ReaderWriterLock m_appenderLock = new ReaderWriterLock();

		public virtual Logger Parent
		{
			get
			{
				return m_parent;
			}
			set
			{
				m_parent = value;
			}
		}

		public virtual bool Additivity
		{
			get
			{
				return m_additive;
			}
			set
			{
				m_additive = value;
			}
		}

		public virtual Level EffectiveLevel
		{
			get
			{
				for (Logger logger = this; logger != null; logger = logger.m_parent)
				{
					Level level = logger.m_level;
					if ((object)level != null)
					{
						return level;
					}
				}
				return null;
			}
		}

		public virtual Hierarchy Hierarchy
		{
			get
			{
				return m_hierarchy;
			}
			set
			{
				m_hierarchy = value;
			}
		}

		public virtual Level Level
		{
			get
			{
				return m_level;
			}
			set
			{
				m_level = value;
			}
		}

		public virtual AppenderCollection Appenders
		{
			get
			{
				m_appenderLock.AcquireReaderLock();
				try
				{
					if (m_appenderAttachedImpl == null)
					{
						return AppenderCollection.EmptyCollection;
					}
					return m_appenderAttachedImpl.Appenders;
				}
				finally
				{
					m_appenderLock.ReleaseReaderLock();
				}
			}
		}

		public virtual string Name
		{
			get
			{
				return m_name;
			}
		}

		public ILoggerRepository Repository
		{
			get
			{
				return m_hierarchy;
			}
		}

		protected Logger(string name)
		{
			m_name = string.Intern(name);
		}

		public virtual void AddAppender(IAppender newAppender)
		{
			if (newAppender == null)
			{
				throw new ArgumentNullException("newAppender");
			}
			m_appenderLock.AcquireWriterLock();
			try
			{
				if (m_appenderAttachedImpl == null)
				{
					m_appenderAttachedImpl = new AppenderAttachedImpl();
				}
				m_appenderAttachedImpl.AddAppender(newAppender);
			}
			finally
			{
				m_appenderLock.ReleaseWriterLock();
			}
		}

		public virtual IAppender GetAppender(string name)
		{
			m_appenderLock.AcquireReaderLock();
			try
			{
				if (m_appenderAttachedImpl == null || name == null)
				{
					return null;
				}
				return m_appenderAttachedImpl.GetAppender(name);
			}
			finally
			{
				m_appenderLock.ReleaseReaderLock();
			}
		}

		public virtual void RemoveAllAppenders()
		{
			m_appenderLock.AcquireWriterLock();
			try
			{
				if (m_appenderAttachedImpl != null)
				{
					m_appenderAttachedImpl.RemoveAllAppenders();
					m_appenderAttachedImpl = null;
				}
			}
			finally
			{
				m_appenderLock.ReleaseWriterLock();
			}
		}

		public virtual IAppender RemoveAppender(IAppender appender)
		{
			m_appenderLock.AcquireWriterLock();
			try
			{
				if (appender != null && m_appenderAttachedImpl != null)
				{
					return m_appenderAttachedImpl.RemoveAppender(appender);
				}
			}
			finally
			{
				m_appenderLock.ReleaseWriterLock();
			}
			return null;
		}

		public virtual IAppender RemoveAppender(string name)
		{
			m_appenderLock.AcquireWriterLock();
			try
			{
				if (name != null && m_appenderAttachedImpl != null)
				{
					return m_appenderAttachedImpl.RemoveAppender(name);
				}
			}
			finally
			{
				m_appenderLock.ReleaseWriterLock();
			}
			return null;
		}

		public virtual void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception)
		{
			try
			{
				if (IsEnabledFor(level))
				{
					ForcedLog(((object)callerStackBoundaryDeclaringType != null) ? callerStackBoundaryDeclaringType : ThisDeclaringType, level, message, exception);
				}
			}
			catch (Exception exception2)
			{
				LogLog.Error("Log: Exception while logging", exception2);
			}
			catch
			{
				LogLog.Error("Log: Exception while logging");
			}
		}

		public virtual void Log(LoggingEvent logEvent)
		{
			try
			{
				if (logEvent != null && IsEnabledFor(logEvent.Level))
				{
					ForcedLog(logEvent);
				}
			}
			catch (Exception exception)
			{
				LogLog.Error("Log: Exception while logging", exception);
			}
			catch
			{
				LogLog.Error("Log: Exception while logging");
			}
		}

		public virtual bool IsEnabledFor(Level level)
		{
			try
			{
				if (level != null)
				{
					if (m_hierarchy.IsDisabled(level))
					{
						return false;
					}
					return level >= EffectiveLevel;
				}
			}
			catch (Exception exception)
			{
				LogLog.Error("Log: Exception while logging", exception);
			}
			catch
			{
				LogLog.Error("Log: Exception while logging");
			}
			return false;
		}

		protected virtual void CallAppenders(LoggingEvent loggingEvent)
		{
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			int num = 0;
			for (Logger logger = this; logger != null; logger = logger.m_parent)
			{
				if (logger.m_appenderAttachedImpl != null)
				{
					logger.m_appenderLock.AcquireReaderLock();
					try
					{
						if (logger.m_appenderAttachedImpl != null)
						{
							num += logger.m_appenderAttachedImpl.AppendLoopOnAppenders(loggingEvent);
						}
					}
					finally
					{
						logger.m_appenderLock.ReleaseReaderLock();
					}
				}
				if (!logger.m_additive)
				{
					break;
				}
			}
			if (!m_hierarchy.EmittedNoAppenderWarning && num == 0)
			{
				LogLog.Debug("Logger: No appenders could be found for logger [" + Name + "] repository [" + Repository.Name + "]");
				LogLog.Debug("Logger: Please initialize the log4net system properly.");
				try
				{
					LogLog.Debug("Logger:    Current AppDomain context information: ");
					LogLog.Debug("Logger:       BaseDirectory   : " + SystemInfo.ApplicationBaseDirectory);
					LogLog.Debug("Logger:       FriendlyName    : " + AppDomain.CurrentDomain.FriendlyName);
					LogLog.Debug("Logger:       DynamicDirectory: " + AppDomain.CurrentDomain.DynamicDirectory);
				}
				catch (SecurityException)
				{
				}
				m_hierarchy.EmittedNoAppenderWarning = true;
			}
		}

		public virtual void CloseNestedAppenders()
		{
			m_appenderLock.AcquireWriterLock();
			try
			{
				if (m_appenderAttachedImpl == null)
				{
					return;
				}
				AppenderCollection.IAppenderCollectionEnumerator enumerator = m_appenderAttachedImpl.Appenders.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IAppender current = enumerator.Current;
						if (current is IAppenderAttachable)
						{
							current.Close();
						}
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
			finally
			{
				m_appenderLock.ReleaseWriterLock();
			}
		}

		public virtual void Log(Level level, object message, Exception exception)
		{
			if (IsEnabledFor(level))
			{
				ForcedLog(ThisDeclaringType, level, message, exception);
			}
		}

		protected virtual void ForcedLog(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception)
		{
			CallAppenders(new LoggingEvent(callerStackBoundaryDeclaringType, Hierarchy, Name, level, message, exception));
		}

		protected virtual void ForcedLog(LoggingEvent logEvent)
		{
			logEvent.EnsureRepository(Hierarchy);
			CallAppenders(logEvent);
		}
	}
}
