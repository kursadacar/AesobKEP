using Tr.Com.Eimza.Log4Net.Appender;

namespace Tr.Com.Eimza.Log4Net.Repository
{
	public interface IBasicRepositoryConfigurator
	{
		void Configure(IAppender appender);
	}
}
