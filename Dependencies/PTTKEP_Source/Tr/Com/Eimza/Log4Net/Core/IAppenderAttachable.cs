using Tr.Com.Eimza.Log4Net.Appender;

namespace Tr.Com.Eimza.Log4Net.Core
{
	public interface IAppenderAttachable
	{
		AppenderCollection Appenders { get; }

		void AddAppender(IAppender appender);

		IAppender GetAppender(string name);

		void RemoveAllAppenders();

		IAppender RemoveAppender(IAppender appender);

		IAppender RemoveAppender(string name);
	}
}
