using System;

namespace Tr.Com.Eimza.Log4Net.Util.TypeConverters
{
	internal class BooleanConverter : IConvertFrom
	{
		public bool CanConvertFrom(Type sourceType)
		{
			return (object)sourceType == typeof(string);
		}

		public object ConvertFrom(object source)
		{
			string text = source as string;
			if (text != null)
			{
				return bool.Parse(text);
			}
			throw ConversionNotSupportedException.Create(typeof(bool), source);
		}
	}
}
