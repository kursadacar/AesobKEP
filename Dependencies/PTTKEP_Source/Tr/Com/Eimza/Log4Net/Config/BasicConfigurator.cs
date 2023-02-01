using System.Reflection;
using Tr.Com.Eimza.Log4Net.Appender;
using Tr.Com.Eimza.Log4Net.Layout;
using Tr.Com.Eimza.Log4Net.Repository;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Config
{
	public sealed class BasicConfigurator
	{
		private BasicConfigurator()
		{
		}

		public static void Configure()
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));
		}

		public static void Configure(IAppender appender)
		{
			Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()), appender);
		}

		public static void Configure(ILoggerRepository repository)
		{
			PatternLayout patternLayout = new PatternLayout();
			patternLayout.ConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline";
			patternLayout.ActivateOptions();
			ConsoleAppender consoleAppender = new ConsoleAppender();
			consoleAppender.Layout = patternLayout;
			consoleAppender.ActivateOptions();
			Configure(repository, consoleAppender);
		}

		public static void Configure(ILoggerRepository repository, IAppender appender)
		{
			IBasicRepositoryConfigurator basicRepositoryConfigurator = repository as IBasicRepositoryConfigurator;
			if (basicRepositoryConfigurator != null)
			{
				basicRepositoryConfigurator.Configure(appender);
			}
			else
			{
				LogLog.Warn("BasicConfigurator: Repository [" + ((repository != null) ? repository.ToString() : null) + "] does not support the BasicConfigurator");
			}
		}
	}
}
