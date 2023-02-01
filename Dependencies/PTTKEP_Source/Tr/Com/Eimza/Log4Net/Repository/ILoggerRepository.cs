using Tr.Com.Eimza.Log4Net.Appender;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.ObjectRenderer;
using Tr.Com.Eimza.Log4Net.Plugin;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Repository
{
	public interface ILoggerRepository
	{
		string Name { get; set; }

		RendererMap RendererMap { get; }

		PluginMap PluginMap { get; }

		LevelMap LevelMap { get; }

		Level Threshold { get; set; }

		bool Configured { get; set; }

		PropertiesDictionary Properties { get; }

		event LoggerRepositoryShutdownEventHandler ShutdownEvent;

		event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset;

		event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged;

		ILogger Exists(string name);

		ILogger[] GetCurrentLoggers();

		ILogger GetLogger(string name);

		void Shutdown();

		void ResetConfiguration();

		void Log(LoggingEvent logEvent);

		IAppender[] GetAppenders();
	}
}
