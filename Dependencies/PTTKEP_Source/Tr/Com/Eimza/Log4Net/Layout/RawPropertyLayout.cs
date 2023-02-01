using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Layout
{
	public class RawPropertyLayout : IRawLayout
	{
		private string m_key;

		public string Key
		{
			get
			{
				return m_key;
			}
			set
			{
				m_key = value;
			}
		}

		public virtual object Format(LoggingEvent loggingEvent)
		{
			return loggingEvent.LookupProperty(m_key);
		}
	}
}
