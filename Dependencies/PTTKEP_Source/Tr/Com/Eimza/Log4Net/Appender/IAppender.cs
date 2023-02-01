using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public interface IAppender
	{
		string Name { get; set; }

		void Close();

		void DoAppend(LoggingEvent loggingEvent);
	}
}
