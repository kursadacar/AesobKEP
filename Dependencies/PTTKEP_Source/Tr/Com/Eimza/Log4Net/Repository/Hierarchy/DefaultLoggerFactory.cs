using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Repository.Hierarchy
{
	internal class DefaultLoggerFactory : ILoggerFactory
	{
		internal sealed class LoggerImpl : Logger
		{
			internal LoggerImpl(string name)
				: base(name)
			{
			}
		}

		internal DefaultLoggerFactory()
		{
		}

		public Logger CreateLogger(string name)
		{
			if (name == null)
			{
				return new RootLogger(Level.Debug);
			}
			return new LoggerImpl(name);
		}
	}
}
