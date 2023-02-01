using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	internal static class CkaUtils
	{
		public static CK_ATTRIBUTE CreateAttribute(CKA type)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type));
		}

		public static CK_ATTRIBUTE CreateAttribute(ulong type)
		{
			return _CreateAttribute(type, null);
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, ulong value)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type), value);
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, CKC value)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type), Convert.ToUInt64((uint)value));
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, CKK value)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type), Convert.ToUInt64((uint)value));
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, CKO value)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type), Convert.ToUInt64((uint)value));
		}

		public static CK_ATTRIBUTE CreateAttribute(ulong type, ulong value)
		{
			return _CreateAttribute(type, ConvertUtils.ULongToBytes(value));
		}

		public static void ConvertValue(ref CK_ATTRIBUTE attribute, out ulong value)
		{
			byte[] value2 = ConvertValue(ref attribute);
			value = ConvertUtils.BytesToULong(value2);
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, bool value)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type), value);
		}

		public static CK_ATTRIBUTE CreateAttribute(ulong type, bool value)
		{
			return _CreateAttribute(type, ConvertUtils.BoolToBytes(value));
		}

		public static void ConvertValue(ref CK_ATTRIBUTE attribute, out bool value)
		{
			byte[] value2 = ConvertValue(ref attribute);
			value = ConvertUtils.BytesToBool(value2);
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, string value)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type), value);
		}

		public static CK_ATTRIBUTE CreateAttribute(ulong type, string value)
		{
			return _CreateAttribute(type, ConvertUtils.Utf8StringToBytes(value));
		}

		public static void ConvertValue(ref CK_ATTRIBUTE attribute, out string value)
		{
			byte[] value2 = ConvertValue(ref attribute);
			value = ConvertUtils.BytesToUtf8String(value2);
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, byte[] value)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type), value);
		}

		public static CK_ATTRIBUTE CreateAttribute(ulong type, byte[] value)
		{
			return _CreateAttribute(type, value);
		}

		public static void ConvertValue(ref CK_ATTRIBUTE attribute, out byte[] value)
		{
			value = ConvertValue(ref attribute);
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, DateTime value)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type), value);
		}

		public static CK_ATTRIBUTE CreateAttribute(ulong type, DateTime value)
		{
			byte[] sourceArray = ConvertUtils.Utf8StringToBytes(value.Date.Year.ToString());
			byte[] sourceArray2 = ((value.Date.Month < 10) ? ConvertUtils.Utf8StringToBytes("0" + value.Date.Month) : ConvertUtils.Utf8StringToBytes(value.Date.Month.ToString()));
			byte[] sourceArray3 = ((value.Date.Day < 10) ? ConvertUtils.Utf8StringToBytes("0" + value.Date.Day) : ConvertUtils.Utf8StringToBytes(value.Date.Day.ToString()));
			byte[] array = new byte[8];
			Array.Copy(sourceArray, 0, array, 0, 4);
			Array.Copy(sourceArray2, 0, array, 4, 2);
			Array.Copy(sourceArray3, 0, array, 6, 2);
			return _CreateAttribute(type, array);
		}

		public static void ConvertValue(ref CK_ATTRIBUTE attribute, out DateTime? value)
		{
			byte[] array = ConvertValue(ref attribute);
			if (array.Length == 0)
			{
				value = null;
				return;
			}
			if (array == null || array.Length != 8)
			{
				throw new Exception("Unable to convert attribute value to DateTime");
			}
			string text = ConvertUtils.BytesToUtf8String(array, 0, 4);
			string text2 = ConvertUtils.BytesToUtf8String(array, 4, 2);
			string text3 = ConvertUtils.BytesToUtf8String(array, 6, 2);
			if (text == "0000" && text2 == "00" && text3 == "00")
			{
				value = null;
			}
			else
			{
				value = new DateTime(int.Parse(text), int.Parse(text2), int.Parse(text3), 0, 0, 0, DateTimeKind.Utc);
			}
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, CK_ATTRIBUTE[] value)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type), value);
		}

		public static CK_ATTRIBUTE CreateAttribute(ulong type, CK_ATTRIBUTE[] value)
		{
			CK_ATTRIBUTE result = default(CK_ATTRIBUTE);
			result.type = type;
			if (value != null && value.Length != 0)
			{
				int num = UnmanagedMemory.SizeOf(typeof(CK_ATTRIBUTE));
				result.value = UnmanagedMemory.Allocate(num * value.Length);
				for (int i = 0; i < value.Length; i++)
				{
					UnmanagedMemory.Write(new IntPtr(result.value.ToInt64() + i * num), value[i]);
				}
				result.valueLen = Convert.ToUInt64(num * value.Length);
			}
			else
			{
				result.value = IntPtr.Zero;
				result.valueLen = 0uL;
			}
			return result;
		}

		public static void ConvertValue(ref CK_ATTRIBUTE attribute, out CK_ATTRIBUTE[] value)
		{
			int num = UnmanagedMemory.SizeOf(typeof(CK_ATTRIBUTE));
			int num2 = Convert.ToInt32(attribute.valueLen) / num;
			if (Convert.ToInt32(attribute.valueLen) % num != 0)
			{
				throw new Exception("Unable to convert attribute value to attribute array");
			}
			if (num2 == 0)
			{
				value = null;
				return;
			}
			CK_ATTRIBUTE[] array = new CK_ATTRIBUTE[num2];
			for (int i = 0; i < num2; i++)
			{
				IntPtr memory = new IntPtr(attribute.value.ToInt64() + i * num);
				array[i] = (CK_ATTRIBUTE)UnmanagedMemory.Read(memory, typeof(CK_ATTRIBUTE));
			}
			value = array;
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, ulong[] value)
		{
			return CreateAttribute(Convert.ToUInt64((uint)type), value);
		}

		public static CK_ATTRIBUTE CreateAttribute(ulong type, ulong[] value)
		{
			CK_ATTRIBUTE result = default(CK_ATTRIBUTE);
			result.type = type;
			if (value != null && value.Length != 0)
			{
				int num = UnmanagedMemory.SizeOf(typeof(ulong));
				result.value = UnmanagedMemory.Allocate(num * value.Length);
				for (int i = 0; i < value.Length; i++)
				{
					UnmanagedMemory.Write(new IntPtr(result.value.ToInt64() + i * num), ConvertUtils.ULongToBytes(value[i]));
				}
				result.valueLen = Convert.ToUInt64(num * value.Length);
			}
			else
			{
				result.value = IntPtr.Zero;
				result.valueLen = 0uL;
			}
			return result;
		}

		public static void ConvertValue(ref CK_ATTRIBUTE attribute, out ulong[] value)
		{
			int num = UnmanagedMemory.SizeOf(typeof(ulong));
			int num2 = Convert.ToInt32(attribute.valueLen) / num;
			if (Convert.ToInt32(attribute.valueLen) % num != 0)
			{
				throw new Exception("Unable to convert attribute value to ulong array");
			}
			if (num2 == 0)
			{
				value = null;
				return;
			}
			ulong[] array = new ulong[num2];
			for (int i = 0; i < num2; i++)
			{
				IntPtr memory = new IntPtr(attribute.value.ToInt64() + i * num);
				array[i] = ConvertUtils.BytesToULong(UnmanagedMemory.Read(memory, num));
			}
			value = array;
		}

		public static CK_ATTRIBUTE CreateAttribute(CKA type, CKM[] value)
		{
			ulong[] array = null;
			if (value != null)
			{
				array = new ulong[value.Length];
				for (int i = 0; i < value.Length; i++)
				{
					array[i] = Convert.ToUInt64((uint)value[i]);
				}
			}
			return CreateAttribute(type, array);
		}

		public static CK_ATTRIBUTE CreateAttribute(ulong type, CKM[] value)
		{
			ulong[] array = null;
			if (value != null)
			{
				array = new ulong[value.Length];
				for (int i = 0; i < value.Length; i++)
				{
					array[i] = Convert.ToUInt64((uint)value[i]);
				}
			}
			return CreateAttribute(type, array);
		}

		public static void ConvertValue(ref CK_ATTRIBUTE attribute, out CKM[] value)
		{
			ulong[] value2 = null;
			ConvertValue(ref attribute, out value2);
			CKM[] array = null;
			if (value2 != null)
			{
				array = new CKM[value2.Length];
				for (int i = 0; i < value2.Length; i++)
				{
					array[i] = (CKM)Convert.ToUInt32(value2[i]);
				}
			}
			value = array;
		}

		private static CK_ATTRIBUTE _CreateAttribute(ulong type, byte[] value)
		{
			CK_ATTRIBUTE result = default(CK_ATTRIBUTE);
			result.type = type;
			if (value != null)
			{
				result.value = UnmanagedMemory.Allocate(value.Length);
				UnmanagedMemory.Write(result.value, value);
				result.valueLen = Convert.ToUInt64(value.Length);
			}
			else
			{
				result.value = IntPtr.Zero;
				result.valueLen = 0uL;
			}
			return result;
		}

		private static byte[] ConvertValue(ref CK_ATTRIBUTE attribute)
		{
			byte[] result = null;
			if (attribute.value != IntPtr.Zero)
			{
				result = UnmanagedMemory.Read(attribute.value, Convert.ToInt32(attribute.valueLen));
			}
			return result;
		}
	}
}
