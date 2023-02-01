using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Filter
{
	public abstract class FilterSkeleton : IFilter, IOptionHandler
	{
		private IFilter m_next;

		public IFilter Next
		{
			get
			{
				return m_next;
			}
			set
			{
				m_next = value;
			}
		}

		public virtual void ActivateOptions()
		{
		}

		public abstract FilterDecision Decide(LoggingEvent loggingEvent);
	}
}
