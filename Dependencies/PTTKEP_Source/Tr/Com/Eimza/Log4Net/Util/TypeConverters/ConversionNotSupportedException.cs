using System;
using System.Runtime.Serialization;

namespace Tr.Com.Eimza.Log4Net.Util.TypeConverters
{
	[Serializable]
	public class ConversionNotSupportedException : ApplicationException
	{
		public ConversionNotSupportedException()
		{
		}

		public ConversionNotSupportedException(string message)
			: base(message)
		{
		}

		public ConversionNotSupportedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ConversionNotSupportedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public static ConversionNotSupportedException Create(Type destinationType, object sourceValue)
		{
			return Create(destinationType, sourceValue, null);
		}

		public static ConversionNotSupportedException Create(Type destinationType, object sourceValue, Exception innerException)
		{
			if (sourceValue == null)
			{
				return new ConversionNotSupportedException("Cannot convert value [null] to type [" + (((object)destinationType != null) ? destinationType.ToString() : null) + "]", innerException);
			}
			string[] obj = new string[7] { "Cannot convert from type [", null, null, null, null, null, null };
			Type type = sourceValue.GetType();
			obj[1] = (((object)type != null) ? type.ToString() : null);
			obj[2] = "] value [";
			obj[3] = ((sourceValue != null) ? sourceValue.ToString() : null);
			obj[4] = "] to type [";
			obj[5] = (((object)destinationType != null) ? destinationType.ToString() : null);
			obj[6] = "]";
			return new ConversionNotSupportedException(string.Concat(obj), innerException);
		}
	}
}
