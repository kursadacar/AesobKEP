using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Filter
{
	public interface IFilter : IOptionHandler
	{
		IFilter Next { get; set; }

		FilterDecision Decide(LoggingEvent loggingEvent);
	}
}
