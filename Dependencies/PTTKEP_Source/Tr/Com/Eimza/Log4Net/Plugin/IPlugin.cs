using Tr.Com.Eimza.Log4Net.Repository;

namespace Tr.Com.Eimza.Log4Net.Plugin
{
	public interface IPlugin
	{
		string Name { get; }

		void Attach(ILoggerRepository repository);

		void Shutdown();
	}
}
