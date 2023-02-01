using System;
using System.Text;

namespace Tr.Com.Eimza.Log4Net.Util.TypeConverters
{
	internal class EncodingConverter : IConvertFrom
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
				return Encoding.GetEncoding(text);
			}
			throw ConversionNotSupportedException.Create(typeof(Encoding), source);
		}
	}
}
