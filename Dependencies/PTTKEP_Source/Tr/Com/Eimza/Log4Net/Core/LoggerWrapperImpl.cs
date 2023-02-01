namespace Tr.Com.Eimza.Log4Net.Core
{
	public abstract class LoggerWrapperImpl : ILoggerWrapper
	{
		private readonly ILogger m_logger;

		public virtual ILogger Logger
		{
			get
			{
				return m_logger;
			}
		}

		protected LoggerWrapperImpl(ILogger logger)
		{
			m_logger = logger;
		}
	}
}
