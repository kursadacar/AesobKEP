using System;

namespace Tr.Com.Eimza.Log4Net.Util.TypeConverters
{
	public interface IConvertFrom
	{
		bool CanConvertFrom(Type sourceType);

		object ConvertFrom(object source);
	}
}
