using System;
using System.Linq;
using System.Text;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
	internal static class StringUtil
	{
		public static string FromByteArray(byte[] ba)
		{
			StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
			for (int i = 0; i < ba.Length; i++)
			{
				stringBuilder.Append(ba[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		public static byte[] ToByteArray(string hex)
		{
			return (from x in Enumerable.Range(0, hex.Length)
				where x % 2 == 0
				select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
		}

		public static string ToString(byte[] ba)
		{
			return ToString(ba, 0, ba.Length);
		}

		public static string ToString(byte[] ba, int offset, int length)
		{
			return BitConverter.ToString(ba, offset, length).Replace("-", string.Empty);
		}
	}
}
