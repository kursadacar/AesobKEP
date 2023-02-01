using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public interface IBulkAppender : IAppender
	{
		void DoAppend(LoggingEvent[] loggingEvents);
	}
}
