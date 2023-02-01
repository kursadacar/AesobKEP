using System;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace Tr.Com.Eimza.Smime
{
	internal static class SMailTransferEncoder
	{
		private const int MAX_CHARS_PER_LINE = 76;

		private static readonly string[] quotedPrintableChars = new string[256]
		{
			"=00", "=01", "=02", "=03", "=04", "=05", "=06", "=07", "=08", "\t",
			"=0A", "=0B", "=0C", "=0D", "=0E", "=0F", "=10", "=11", "=12", "=13",
			"=14", "=15", "=16", "=17", "=18", "=19", "=1A", "=1B", "=1C", "=1D",
			"=1E", "=1F", " ", "!", "\"", "#", "$", "%", "&", "'",
			"(", ")", "*", "+", ",", "-", ".", "/", "0", "1",
			"2", "3", "4", "5", "6", "7", "8", "9", ":", ";",
			"<", "=3D", ">", "?", "@", "A", "B", "C", "D", "E",
			"F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
			"P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y",
			"Z", "[", "\\", "]", "^", "_", "`", "a", "b", "c",
			"d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
			"n", "o", "p", "q", "r", "s", "t", "u", "v", "w",
			"x", "y", "z", "{", "|", "}", "~", "=7F", "=80", "=81",
			"=82", "=83", "=84", "=85", "=86", "=87", "=88", "=89", "=8A", "=8B",
			"=8C", "=8D", "=8E", "=8F", "=90", "=91", "=92", "=93", "=94", "=95",
			"=96", "=97", "=98", "=99", "=9A", "=9B", "=9C", "=9D", "=9E", "=9F",
			"=A0", "=A1", "=A2", "=A3", "=A4", "=A5", "=A6", "=A7", "=A8", "=A9",
			"=AA", "=AB", "=AC", "=AD", "=AE", "=AF", "=B0", "=B1", "=B2", "=B3",
			"=B4", "=B5", "=B6", "=B7", "=B8", "=B9", "=BA", "=BB", "=BC", "=BD",
			"=BE", "=BF", "=C0", "=C1", "=C2", "=C3", "=C4", "=C5", "=C6", "=C7",
			"=C8", "=C9", "=CA", "=CB", "=CC", "=CD", "=CE", "=CF", "=D0", "=D1",
			"=D2", "=D3", "=D4", "=D5", "=D6", "=D7", "=D8", "=D9", "=DA", "=DB",
			"=DC", "=DD", "=DE", "=DF", "=E0", "=E1", "=E2", "=E3", "=E4", "=E5",
			"=E6", "=E7", "=E8", "=E9", "=EA", "=EB", "=EC", "=ED", "=EE", "=EF",
			"=F0", "=F1", "=F2", "=F3", "=F4", "=F5", "=F6", "=F7", "=F8", "=F9",
			"=FA", "=FB", "=FC", "=FD", "=FE", "=FF"
		};

		private static readonly char[] especials = new char[16]
		{
			'(', ')', '<', '>', '@', ',', ';', ':', '<', '>',
			'/', '[', ']', '?', '.', '='
		};

		private static char[] hex = new char[16]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};

		internal static string GetTransferEncodingName(SMailTransferEncoding encoding)
		{
			switch (encoding)
			{
			case SMailTransferEncoding.SevenBit:
				return "7bit";
			case SMailTransferEncoding.QuotedPrintable:
				return "quoted-printable";
			case SMailTransferEncoding.Base64:
				return "base64";
			default:
				throw new ArgumentOutOfRangeException("encoding");
			}
		}

		internal static TransferEncoding ConvertTransferEncoding(SMailTransferEncoding encoding)
		{
			switch (encoding)
			{
			case SMailTransferEncoding.SevenBit:
				return TransferEncoding.SevenBit;
			case SMailTransferEncoding.QuotedPrintable:
				return TransferEncoding.QuotedPrintable;
			case SMailTransferEncoding.Base64:
				return TransferEncoding.Base64;
			default:
				throw new ArgumentOutOfRangeException("encoding");
			}
		}

		public static string NormalizeLinefeeds(string s)
		{
			s = Regex.Replace(s, "\\r(?!\\n)", "\r\n");
			s = Regex.Replace(s, "(?<!\\r)\\n", "\r\n");
			return s;
		}

		public static string ToBase64(byte[] bytes)
		{
			return Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
		}

		public static string ToQuotedPrintable(byte[] bytes, bool encodeNewlines)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				if (IsNewline(bytes, i) && !encodeNewlines)
				{
					if (i > 0 && IsWhitespace(bytes[i - 1]))
					{
						stringBuilder.Length--;
						if (bytes[i - 1] == 32)
						{
							stringBuilder.Append("=20");
						}
						else if (bytes[i - 1] == 9)
						{
							stringBuilder.Append("=09");
						}
					}
					i++;
					stringBuilder.Append("\r\n");
					num = 0;
					continue;
				}
				if (num >= 72)
				{
					stringBuilder.Append("=\r\n");
					num = 0;
					i--;
					continue;
				}
				if (IsWhitespace(bytes[i]))
				{
					stringBuilder.Append(quotedPrintableChars[bytes[i]]);
					num += quotedPrintableChars[bytes[i]].Length;
					continue;
				}
				int bytesRead;
				int num2 = CharactersUntilNextWhitespace(bytes, i, encodeNewlines, out bytesRead);
				if (num2 > 72)
				{
					if (num != 0)
					{
						stringBuilder.Append("=\r\n");
						num = 0;
					}
					for (int j = 0; j < bytesRead; j++)
					{
						stringBuilder.Append(quotedPrintableChars[bytes[i + j]]);
						num += quotedPrintableChars[bytes[i + j]].Length;
						if (num >= 72)
						{
							stringBuilder.Append("=\r\n");
							num = 0;
						}
					}
					if (bytesRead > 0)
					{
						i += bytesRead - 1;
					}
				}
				else if (num2 + num >= 72)
				{
					stringBuilder.Append("=\r\n");
					num = 0;
					for (int k = 0; k < bytesRead; k++)
					{
						stringBuilder.Append(quotedPrintableChars[bytes[i + k]]);
						num += quotedPrintableChars[bytes[i + k]].Length;
					}
					if (bytesRead > 0)
					{
						i += bytesRead - 1;
					}
				}
				else
				{
					for (int l = 0; l < bytesRead; l++)
					{
						stringBuilder.Append(quotedPrintableChars[bytes[i + l]]);
						num += quotedPrintableChars[bytes[i + l]].Length;
					}
					if (bytesRead > 0)
					{
						i += bytesRead - 1;
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				switch (stringBuilder[stringBuilder.Length - 1])
				{
				case ' ':
					stringBuilder.Length--;
					stringBuilder.Append("=20");
					break;
				case '\t':
					stringBuilder.Length--;
					stringBuilder.Append("=09");
					break;
				}
			}
			return stringBuilder.ToString();
		}

		private static bool IsNewline(byte[] bytes, int currentPosition)
		{
			if (currentPosition < bytes.Length - 1 && bytes[currentPosition] == 13)
			{
				return bytes[currentPosition + 1] == 10;
			}
			return false;
		}

		private static bool IsWhitespace(byte character)
		{
			if (character != 9)
			{
				return character == 32;
			}
			return true;
		}

		private static int CharactersUntilNextWhitespace(byte[] bytes, int currentPosition, bool encodeNewlines, out int bytesRead)
		{
			int num = 0;
			bytesRead = 0;
			while (currentPosition < bytes.Length && !IsWhitespace(bytes[currentPosition]) && (encodeNewlines || !IsNewline(bytes, currentPosition)) && num <= 76)
			{
				bytesRead++;
				num += quotedPrintableChars[bytes[currentPosition]].Length;
				currentPosition++;
			}
			return num;
		}

		internal static string WrapIfEspecialsExist(string s)
		{
			s = s.Replace("\"", "\\\"");
			if (s.IndexOfAny(especials) >= 0)
			{
				return "\"" + s + "\"";
			}
			return s;
		}

		internal static string To2047(byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in bytes)
			{
				if (b < 33 || b > 126 || b == 63 || b == 61 || b == 95)
				{
					stringBuilder.Append('=');
					stringBuilder.Append(hex[(b >> 4) & 0xF]);
					stringBuilder.Append(hex[b & 0xF]);
				}
				else
				{
					stringBuilder.Append((char)b);
				}
			}
			return stringBuilder.ToString();
		}

		internal static string EncodeSubjectRFC2047(string s, Encoding enc)
		{
			if (s == null || Encoding.ASCII.Equals(enc))
			{
				return s;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] >= '\u0080')
				{
					string text = To2047(enc.GetBytes(s));
					return "=?" + enc.HeaderName + "?Q?" + text + "?=";
				}
			}
			return s;
		}
	}
}
