using System;
using System.Text;

namespace Tr.Com.Eimza.Pkcs11.C
{
    internal static class ConvertUtils
	{
		public static byte[] UIntToBytes(uint value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			int num = UnmanagedMemory.SizeOf(typeof(uint));
			if (num != bytes.Length)
			{
				throw new Exception(string.Format("Unmanaged size of uint ({0}) does not match the length of produced byte array ({1})", num, bytes.Length));
			}
			return bytes;
		}

		public static uint BytesToUInt(byte[] value)
		{
			if (value == null || value.Length != UnmanagedMemory.SizeOf(typeof(uint)))
			{
				throw new Exception("Unable to convert bytes to uint");
			}
			return BitConverter.ToUInt32(value, 0);
		}

		public static byte[] ULongToBytes(ulong value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			int num = UnmanagedMemory.SizeOf(typeof(ulong));
			if (num != bytes.Length)
			{
				throw new Exception(string.Format("Unmanaged size of ulong ({0}) does not match the length of produced byte array ({1})", num, bytes.Length));
			}
			return bytes;
		}

		public static ulong BytesToULong(byte[] value)
		{
			if (value == null || value.Length != UnmanagedMemory.SizeOf(typeof(ulong)))
			{
				throw new Exception("Unable to convert bytes to ulong");
			}
			return BitConverter.ToUInt64(value, 0);
		}

		public static byte[] BoolToBytes(bool value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			int num = 1;
			if (num != bytes.Length)
			{
				throw new Exception(string.Format("Unmanaged size of bool ({0}) does not match the length of produced byte array ({1})", num, bytes.Length));
			}
			return bytes;
		}

		public static bool BytesToBool(byte[] value)
		{
			if (value == null || value.Length != 1)
			{
				throw new Exception("Unable to convert bytes to bool");
			}
			return BitConverter.ToBoolean(value, 0);
		}

		public static byte[] Utf8StringToBytes(string value)
		{
			if (value != null)
			{
				return Encoding.UTF8.GetBytes(value);
			}
			return null;
		}

		public static byte[] Utf8StringToBytes(string value, int outputLength, byte paddingByte)
		{
			if (outputLength < 1)
			{
				throw new ArgumentException("Value has to be positive number", "outputLength");
			}
			byte[] array = new byte[outputLength];
			for (int i = 0; i < outputLength; i++)
			{
				array[i] = paddingByte;
			}
			if (value != null)
			{
				byte[] array2 = Utf8StringToBytes(value);
				if (array2.Length > outputLength)
				{
					Array.Copy(array2, 0, array, 0, outputLength);
				}
				else
				{
					Array.Copy(array2, 0, array, 0, array2.Length);
				}
			}
			return array;
		}

		public static string BytesToUtf8String(byte[] value)
		{
			if (value != null)
			{
				return Encoding.UTF8.GetString(value);
			}
			return null;
		}

		public static string BytesToUtf8String(byte[] value, bool trimEnd)
		{
			string text = BytesToUtf8String(value);
			if (value != null && trimEnd)
			{
				text = text.TrimEnd(null);
			}
			return text;
		}

		public static string BytesToUtf8String(byte[] value, int index, int count)
		{
			if (value != null)
			{
				return Encoding.UTF8.GetString(value, index, count);
			}
			return null;
		}

		public static string CkVersionToString(Tr.Com.Eimza.Pkcs11.L4.CK_VERSION ck_version)
		{
			return string.Format("{0}.{1}", ck_version.Major[0], ck_version.Minor[0]);
		}

		public static string CkVersionToString(Tr.Com.Eimza.Pkcs11.L8.CK_VERSION ck_version)
		{
			return string.Format("{0}.{1}", ck_version.Major[0], ck_version.Minor[0]);
		}

		public static DateTime? UtcTimeStringToDateTime(string utcTime)
		{
			DateTime? result = null;
			if (!string.IsNullOrEmpty(utcTime) && utcTime.Length == 16)
			{
				int year = int.Parse(utcTime.Substring(0, 4));
				int month = int.Parse(utcTime.Substring(4, 2));
				int day = int.Parse(utcTime.Substring(6, 2));
				int hour = int.Parse(utcTime.Substring(8, 2));
				int minute = int.Parse(utcTime.Substring(10, 2));
				int second = int.Parse(utcTime.Substring(12, 2));
				int millisecond = int.Parse(utcTime.Substring(14, 2));
				result = new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Utc);
			}
			return result;
		}

		public static string BytesToHexString(byte[] value)
		{
			return BitConverter.ToString(value).Replace("-", "");
		}

		public static byte[] HexStringToBytes(string value)
		{
			byte[] array = new byte[value.Length / 2];
			for (int i = 0; i < value.Length; i += 2)
			{
				array[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);
			}
			return array;
		}

		public static string BytesToBase64String(byte[] value)
		{
			return Convert.ToBase64String(value);
		}

		public static byte[] Base64StringToBytes(string value)
		{
			return Convert.FromBase64String(value);
		}
	}
}
