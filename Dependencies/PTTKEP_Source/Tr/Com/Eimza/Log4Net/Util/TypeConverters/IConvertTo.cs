using System;

namespace Tr.Com.Eimza.Log4Net.Util.TypeConverters
{
	public interface IConvertTo
	{
		bool CanConvertTo(Type targetType);

		object ConvertTo(object source, Type targetType);
	}
}
