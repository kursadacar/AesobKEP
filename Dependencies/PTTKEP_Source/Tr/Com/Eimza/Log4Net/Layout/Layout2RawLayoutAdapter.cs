using System.Globalization;
using System.IO;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Layout
{
	public class Layout2RawLayoutAdapter : IRawLayout
	{
		private ILayout m_layout;

		public Layout2RawLayoutAdapter(ILayout layout)
		{
			m_layout = layout;
		}

		public virtual object Format(LoggingEvent loggingEvent)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			m_layout.Format(stringWriter, loggingEvent);
			return stringWriter.ToString();
		}
	}
}
