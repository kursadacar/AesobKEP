using System;

namespace ActiveUp.Net.Mail
{
	public class Converter
	{
		public static byte[] ToByteArray(int input)
		{
			byte b = (byte)(input >> 24);
			byte b2 = (byte)((input & 0xFF0000) >> 16);
			byte b3 = (byte)((input & 0xFF00) >> 8);
			byte b4 = (byte)((uint)input & 0xFFu);
			return new byte[4] { b, b2, b3, b4 };
		}

		public static ulong ToULong(byte[] input)
		{
			return ((ulong)input[0] << 56) + ((ulong)input[1] << 48) + ((ulong)input[2] << 40) + ((ulong)input[3] << 32) + ((ulong)input[4] << 24) + ((ulong)input[5] << 16) + ((ulong)input[6] << 8) + input[7];
		}

		public static short ToShort(byte[] input)
		{
			return (short)((input[0] << 8) + input[1]);
		}

		public static int ToInt(byte[] input)
		{
			return (input[0] << 24) + (input[1] << 16) + (input[2] << 8) + input[3];
		}

		public static long ToLong(byte[] input)
		{
			return (long)(((ulong)input[0] << 56) | ((ulong)input[1] << 48) | ((ulong)input[2] << 40) | ((ulong)input[3] << 32) | ((ulong)input[4] << 24) | ((ulong)input[5] << 16) | ((ulong)input[6] << 8) | input[7]);
		}

		public static DateTime UnixTimeStampToDateTime(int timeStamp)
		{
			return new DateTime(1970, 1, 1).AddSeconds(timeStamp);
		}
	}
}
