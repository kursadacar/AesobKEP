using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util.TypeConverters;

namespace Tr.Com.Eimza.Log4Net.Layout
{
	[TypeConverter(typeof(RawLayoutConverter))]
	public interface IRawLayout
	{
		object Format(LoggingEvent loggingEvent);
	}
}
