using System;
using System.Collections;
using System.Net;
using System.Text;
using Tr.Com.Eimza.Log4Net.Layout;

namespace Tr.Com.Eimza.Log4Net.Util.TypeConverters
{
	public sealed class ConverterRegistry
	{
		private static Hashtable s_type2converter;

		private ConverterRegistry()
		{
		}

		static ConverterRegistry()
		{
			s_type2converter = new Hashtable();
			AddConverter(typeof(bool), typeof(BooleanConverter));
			AddConverter(typeof(Encoding), typeof(EncodingConverter));
			AddConverter(typeof(Type), typeof(TypeConverter));
			AddConverter(typeof(PatternLayout), typeof(PatternLayoutConverter));
			AddConverter(typeof(PatternString), typeof(PatternStringConverter));
			AddConverter(typeof(IPAddress), typeof(IPAddressConverter));
		}

		public static void AddConverter(Type destinationType, object converter)
		{
			if ((object)destinationType != null && converter != null)
			{
				lock (s_type2converter)
				{
					s_type2converter[destinationType] = converter;
				}
			}
		}

		public static void AddConverter(Type destinationType, Type converterType)
		{
			AddConverter(destinationType, CreateConverterInstance(converterType));
		}

		public static IConvertTo GetConvertTo(Type sourceType, Type destinationType)
		{
			lock (s_type2converter)
			{
				IConvertTo convertTo = s_type2converter[sourceType] as IConvertTo;
				if (convertTo == null)
				{
					convertTo = GetConverterFromAttribute(sourceType) as IConvertTo;
					if (convertTo != null)
					{
						s_type2converter[sourceType] = convertTo;
					}
				}
				return convertTo;
			}
		}

		public static IConvertFrom GetConvertFrom(Type destinationType)
		{
			lock (s_type2converter)
			{
				IConvertFrom convertFrom = s_type2converter[destinationType] as IConvertFrom;
				if (convertFrom == null)
				{
					convertFrom = GetConverterFromAttribute(destinationType) as IConvertFrom;
					if (convertFrom != null)
					{
						s_type2converter[destinationType] = convertFrom;
					}
				}
				return convertFrom;
			}
		}

		private static object GetConverterFromAttribute(Type destinationType)
		{
			object[] customAttributes = destinationType.GetCustomAttributes(typeof(TypeConverterAttribute), true);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				TypeConverterAttribute typeConverterAttribute = customAttributes[0] as TypeConverterAttribute;
				if (typeConverterAttribute != null)
				{
					return CreateConverterInstance(SystemInfo.GetTypeFromString(destinationType, typeConverterAttribute.ConverterTypeName, false, true));
				}
			}
			return null;
		}

		private static object CreateConverterInstance(Type converterType)
		{
			if ((object)converterType == null)
			{
				throw new ArgumentNullException("converterType", "CreateConverterInstance cannot create instance, converterType is null");
			}
			if (typeof(IConvertFrom).IsAssignableFrom(converterType) || typeof(IConvertTo).IsAssignableFrom(converterType))
			{
				try
				{
					return Activator.CreateInstance(converterType);
				}
				catch (Exception exception)
				{
					LogLog.Error("ConverterRegistry: Cannot CreateConverterInstance of type [" + converterType.FullName + "], Exception in call to Activator.CreateInstance", exception);
				}
			}
			else
			{
				LogLog.Error("ConverterRegistry: Cannot CreateConverterInstance of type [" + converterType.FullName + "], type does not implement IConvertFrom or IConvertTo");
			}
			return null;
		}
	}
}
