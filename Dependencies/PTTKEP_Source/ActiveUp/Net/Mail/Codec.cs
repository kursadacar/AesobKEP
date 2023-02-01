using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ActiveUp.Net.Common.Rfc2047;

namespace ActiveUp.Net.Mail
{
	public abstract class Codec
	{
		public const string CrLf = "\r\n";

		private static readonly Regex WhiteSpace = new Regex("(\\?=)(\\s*)(=\\?)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		public static string GetUniqueString()
		{
			return Process.GetCurrentProcess().Id + DateTime.Now.ToString("yyMMddhhmmss") + DateTime.Now.Millisecond + new Random().GetHashCode();
		}

		public static string ToQuotedPrintable(string input, string fromCharset)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (input != null)
			{
				byte[] bytes = GetEncoding(fromCharset).GetBytes(input);
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				for (num = 0; num < bytes.Length; num++)
				{
					if (num2 == 0 && num + 73 - num3 < bytes.Length)
					{
						while (bytes[num + 73 - num3] == 46)
						{
							num2++;
							num3++;
							if (num2 == 72 || num + 73 - num3 >= bytes.Length)
							{
								break;
							}
						}
						num3 = 0;
					}
					byte b = bytes[num];
					if ((b < 33 || b == 61 || b > 126) && b != 32)
					{
						stringBuilder.Append("=" + b.ToString("X").PadLeft(2, '0'));
					}
					else
					{
						stringBuilder.Append((char)b);
					}
					if (num2 >= 72)
					{
						stringBuilder.Append("=\r\n");
						num2 = 0;
					}
					else
					{
						num2++;
					}
				}
			}
			return stringBuilder.ToString();
		}

		public static string RFC2047Encode(string input, string charset)
		{
			return Rfc2047Codec.Encode(input, charset);
		}

		public static Encoding GetEncoding(string encodingName)
		{
			Encoding encoding = null;
			try
			{
				return Encoding.GetEncoding(encodingName);
			}
			catch
			{
				try
				{
					if (encodingName.ToUpper() == "UTF8")
					{
						encodingName = "UTF-8";
					}
					else if (encodingName.StartsWith("ISO") && char.IsDigit(encodingName, 3))
					{
						encodingName = encodingName.Insert(3, "-");
					}
					encodingName = encodingName.Replace("_", "-").ToUpper();
					return Encoding.GetEncoding(encodingName);
				}
				catch
				{
					return Encoding.GetEncoding("iso-8859-1");
				}
			}
		}

		public static string RFC2047Decode(string input)
		{
			return Rfc2047Codec.Decode(input);
		}

		public static string FromQuotedPrintable(string input, string toCharset)
		{
			ArrayList arrayList = new ArrayList();
			byte[] array = new byte[0];
			try
			{
				input = input.Replace("=\r\n", "") + "=3D=3D";
				int num = 0;
				while (num <= input.Length - 3)
				{
					if (input[num] == '=' && input[num + 1] != '=')
					{
						try
						{
							arrayList.Add(Convert.ToByte(int.Parse(string.Concat(input[num + 1], input[num + 2]), NumberStyles.HexNumber)));
							num += 3;
						}
						catch (Exception)
						{
							arrayList.Add((byte)input[num]);
							num++;
						}
					}
					else
					{
						arrayList.Add((byte)input[num]);
						num++;
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				array = new byte[arrayList.Count];
				for (int i = 0; i < arrayList.Count; i++)
				{
					array[i] = (byte)arrayList[i];
				}
			}
			return GetEncoding(toCharset).GetString(array, 0, array.Length).TrimEnd('=');
		}

		public static string GetFieldName(string input)
		{
			switch (input)
			{
			case "content-id":
				return "Content-ID";
			case "message-id":
				return "Message-ID";
			case "content-md5":
				return "Content-HexMD5Digest";
			case "mime-version":
				return "MIME-Version";
			default:
				return Capitalize(input);
			}
		}

		internal static string Capitalize(string input)
		{
			string empty = string.Empty;
			empty = input.Split('-').Aggregate(empty, (string current, string str) => current + str[0].ToString().ToUpper() + str.Substring(1) + "-");
			return empty.TrimEnd('-');
		}

		public static string Wrap(string input, int totalchars)
		{
			totalchars -= 3;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (num = 0; num + totalchars < input.Length; num += totalchars)
			{
				stringBuilder.Append("\r\n" + input.Substring(num, totalchars));
			}
			return (stringBuilder.ToString() + "\r\n" + input.Substring(num, input.Length - num)).TrimStart('\r', '\n');
		}

		public static string GetCRCBase64(string base64input)
		{
			byte[] array = Convert.FromBase64String(base64input);
			long num = 11994318L;
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				num ^= (long)((ulong)b << 16);
				for (int j = 0; j < 8; j++)
				{
					num <<= 1;
					if ((num & 0x1000000) == 16777216)
					{
						num ^= 0x1864CFB;
					}
				}
			}
			byte b2 = (byte)(num >> 16);
			byte b3 = (byte)((num & 0xFF00) >> 8);
			byte b4 = (byte)(num & 0xFF);
			return Convert.ToBase64String(new byte[3] { b2, b3, b4 });
		}

		public static string GetCRCBase64(byte[] input)
		{
			return GetCRCBase64(Convert.ToBase64String(input));
		}

		public static string ToRadix64(byte[] input)
		{
			return Convert.ToBase64String(input) + "\r\n=" + GetCRCBase64(input);
		}

		public static byte[] FromRadix64(string input)
		{
			string text = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
			input = input.TrimEnd('=');
			int num = input.Length - input.Length % 4;
			byte[] array = new byte[input.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (byte)text.IndexOf(input[i]);
			}
			List<byte> list = new List<byte>();
			for (int j = 0; j < num / 4; j++)
			{
				list.Add((byte)((array[j * 4] << 2) + (array[j * 4 + 1] >> 4)));
				list.Add((byte)(((array[j * 4 + 1] & 0xF) << 4) + (array[j * 4 + 2] >> 2)));
				list.Add((byte)(((array[j * 4 + 2] & 3) << 6) + array[j * 4 + 3]));
			}
			if (input.Length % 4 == 3)
			{
				list.Add((byte)((array[array.Length - 3] << 2) + (array[array.Length - 3 + 1] >> 4)));
				list.Add((byte)(((array[array.Length - 3 + 1] & 0xF) << 4) + (array[array.Length - 3 + 2] >> 2)));
			}
			if (input.Length % 4 == 2)
			{
				list.Add((byte)((array[array.Length - 2] << 2) + (array[array.Length - 2 + 1] >> 4)));
			}
			byte[] array2 = new byte[list.Count];
			list.CopyTo(array2);
			return array2;
		}

		public static string ToBitString(byte input)
		{
			return string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Empty + (((input & 0x80) == 128) ? "1" : "0"), ((input & 0x40) == 64) ? "1" : "0"), ((input & 0x20) == 32) ? "1" : "0"), ((input & 0x10) == 16) ? "1" : "0"), ((input & 8) == 8) ? "1" : "0"), ((input & 4) == 4) ? "1" : "0"), ((input & 2) == 2) ? "1" : "0"), ((input & 1) == 1) ? "1" : "0");
		}

		public static string ToBitString(short input)
		{
			return string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Empty + (((input & 0x8000) == 32768) ? "1" : "0"), ((input & 0x4000) == 16384) ? "1" : "0"), ((input & 0x2000) == 8192) ? "1" : "0"), ((input & 0x1000) == 4096) ? "1" : "0"), ((input & 0x800) == 2048) ? "1" : "0"), ((input & 0x400) == 1024) ? "1" : "0"), ((input & 0x200) == 512) ? "1" : "0"), ((input & 0x100) == 256) ? "1" : "0"), ((input & 0x80) == 128) ? "1" : "0"), ((input & 0x40) == 64) ? "1" : "0"), ((input & 0x20) == 32) ? "1" : "0"), ((input & 0x10) == 16) ? "1" : "0"), ((input & 8) == 8) ? "1" : "0"), ((input & 4) == 4) ? "1" : "0"), ((input & 2) == 2) ? "1" : "0"), ((input & 1) == 1) ? "1" : "0");
		}

		internal static byte FromBitString(string input)
		{
			byte b = 0;
			if (input[7].Equals('1'))
			{
				b = (byte)(b + 1);
			}
			if (input[6].Equals('1'))
			{
				b = (byte)(b + 2);
			}
			if (input[5].Equals('1'))
			{
				b = (byte)(b + 4);
			}
			if (input[4].Equals('1'))
			{
				b = (byte)(b + 8);
			}
			if (input[3].Equals('1'))
			{
				b = (byte)(b + 16);
			}
			if (input[2].Equals('1'))
			{
				b = (byte)(b + 32);
			}
			if (input[1].Equals('1'))
			{
				b = (byte)(b + 64);
			}
			if (input[0].Equals('1'))
			{
				b = (byte)(b + 128);
			}
			return b;
		}
	}
}
