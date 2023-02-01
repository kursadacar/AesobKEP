using System;
using Tr.Com.Eimza.Log4Net.Layout;

namespace Tr.Com.Eimza.Log4Net.Util.TypeConverters
{
	internal class PatternLayoutConverter : IConvertFrom
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
				return new PatternLayout(text);
			}
			throw ConversionNotSupportedException.Create(typeof(PatternLayout), source);
		}
	}
}
