namespace Tr.Com.Eimza.Log4Net.Core
{
	public interface ITriggeringEventEvaluator
	{
		bool IsTriggeringEvent(LoggingEvent loggingEvent);
	}
}
