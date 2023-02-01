using System;
using Tr.Com.Eimza.Log4Net.Repository;

namespace Tr.Com.Eimza.Log4Net.Core
{
	public interface ILogger
	{
		string Name { get; }

		ILoggerRepository Repository { get; }

		void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception);

		void Log(LoggingEvent logEvent);

		bool IsEnabledFor(Level level);
	}
}
