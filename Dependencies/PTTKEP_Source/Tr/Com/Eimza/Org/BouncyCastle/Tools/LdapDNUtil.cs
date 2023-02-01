using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
	internal static class LdapDNUtil
	{
		private static bool IsRfc = true;

		public static string Normalize(string dn)
		{
			if (dn == null || dn.Equals(""))
			{
				return "";
			}
			try
			{
				string text = DnCheck(dn);
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4;
				while ((num4 = text.IndexOf(",", num3)) >= 0)
				{
					num2 += DnIndex(text, num3, num4);
					if (num4 > 0 && text[num4 - 1] != '\\' && num2 % 2 != 1)
					{
						stringBuilder.Append(CheckNormalize(text.Substring(num, num4 - num).Trim()) + ",");
						num = num4 + 1;
						num2 = 0;
					}
					num3 = num4 + 1;
				}
				stringBuilder.Append(CheckNormalize(ReplaceDnChar(text.Substring(num))));
				return stringBuilder.ToString();
			}
			catch (IOException)
			{
				return dn;
			}
		}

		public static string Rfc2253toXmlDsig(string dn)
		{
			IsRfc = true;
			return CheckRfcChar(Normalize(dn));
		}

		private static string CheckNormalize(string dn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4;
			while ((num4 = dn.IndexOf("+", num3)) >= 0)
			{
				num2 += DnIndex(dn, num3, num4);
				if (num4 > 0 && dn[num4 - 1] != '\\' && num2 % 2 != 1)
				{
					stringBuilder.Append(ToDnChar(ReplaceDnChar(dn.Substring(num, num4 - num))) + "+");
					num = num4 + 1;
					num2 = 0;
				}
				num3 = num4 + 1;
			}
			stringBuilder.Append(ToDnChar(ReplaceDnChar(dn.Substring(num))));
			return stringBuilder.ToString();
		}

		private static string ToDnChar(string dn)
		{
			int num = dn.IndexOf("=");
			if (num == -1 || (num > 0 && dn[num - 1] == '\\'))
			{
				return dn;
			}
			string text = CheckOID(dn.Substring(0, num));
			string text2 = null;
			text2 = ((text[0] < '0' || text[0] > '9') ? CharNormalize(dn.Substring(num + 1)) : dn.Substring(num + 1));
			return text + "=" + text2;
		}

		private static string DnCheck(string dn, string s1 = ";", string s2 = ",")
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4;
			while ((num4 = dn.IndexOf(s1, num3)) >= 0)
			{
				num2 += DnIndex(dn, num3, num4);
				if (num4 > 0 && dn[num4 - 1] != '\\' && num2 % 2 != 1)
				{
					stringBuilder.Append(ReplaceDnChar(dn.Substring(num, num4 - num)) + s2);
					num = num4 + 1;
					num2 = 0;
				}
				num3 = num4 + 1;
			}
			stringBuilder.Append(ReplaceDnChar(dn.Substring(num)));
			return stringBuilder.ToString();
		}

		private static int DnIndex(string dn, int firstIndex, int lastIndex)
		{
			int num = 0;
			for (int i = firstIndex; i < lastIndex; i++)
			{
				if (dn[i] == '"')
				{
					num++;
				}
			}
			return num;
		}

		private static string ReplaceDnChar(string dn)
		{
			string text = dn.Trim();
			int num = dn.IndexOf(text) + text.Length;
			if (dn.Length > num && text.EndsWith("\\") && !text.EndsWith("\\\\") && dn[num] == ' ')
			{
				text += " ";
			}
			return text;
		}

		private static string CheckOID(string dn)
		{
			string text = dn.ToUpper().Trim();
			if (text.StartsWith("OID"))
			{
				text = text.Substring(3);
			}
			return text;
		}

		private static string CharNormalize(string dn)
		{
			string text = ReplaceDnChar(dn);
			if (text.StartsWith("\""))
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringReader stringReader = new StringReader(text.Substring(1, text.Length - 1 - 1));
				int num = 0;
				while ((num = stringReader.Read()) > -1)
				{
					char c = (char)num;
					switch (c)
					{
					case '#':
					case '+':
					case ',':
					case ';':
					case '<':
					case '=':
					case '>':
						stringBuilder.Append('\\');
						break;
					}
					stringBuilder.Append(c);
				}
				text = ReplaceDnChar(stringBuilder.ToString());
			}
			if (!IsRfc)
			{
				if (text.StartsWith("\\#"))
				{
					text = text.Substring(1);
				}
				return text;
			}
			if (text.StartsWith("#"))
			{
				text = "\\" + text;
			}
			return text;
		}

		private static string CheckRfcChar(string dn)
		{
			try
			{
				return ToRfc(ToHex(dn));
			}
			catch (Exception)
			{
				return dn;
			}
		}

		private static string ToXml(string dn)
		{
			try
			{
				return RfcChar(CheckHexChar(dn));
			}
			catch (Exception)
			{
				return dn;
			}
		}

		private static string CheckHexChar(string dn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringReader stringReader = new StringReader(dn);
			int num = 0;
			while ((num = stringReader.Read()) > -1)
			{
				char c = (char)num;
				if (c != '\\')
				{
					stringBuilder.Append(c);
					continue;
				}
				stringBuilder.Append(c);
				char c2 = (char)stringReader.Read();
				char c3 = (char)stringReader.Read();
				if (((c2 >= '0' && c2 <= '9') || (c2 >= 'A' && c2 <= 'F') || (c2 >= 'a' && c2 <= 'f')) && ((c3 >= '0' && c3 <= '9') || (c3 >= 'A' && c3 <= 'F') || (c3 >= 'a' && c3 <= 'f')))
				{
					char value = (char)byte.Parse(char.ToString(c2) + char.ToString(c3), NumberStyles.HexNumber);
					stringBuilder.Append(value);
				}
				else
				{
					stringBuilder.Append(c2);
					stringBuilder.Append(c3);
				}
			}
			return stringBuilder.ToString();
		}

		private static string ToHex(string dn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringReader stringReader = new StringReader(dn);
			int num = 0;
			while ((num = stringReader.Read()) > -1)
			{
				if (num >= 32)
				{
					stringBuilder.Append((char)num);
					continue;
				}
				stringBuilder.Append('\\');
				stringBuilder.Append(num.ToString("X"));
			}
			return stringBuilder.ToString();
		}

		private static string ToRfc(string dn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringReader stringReader = new StringReader(dn);
			int num = 0;
			while ((num = stringReader.Read()) > -1)
			{
				char c = (char)num;
				if (c != '\\')
				{
					stringBuilder.Append(c);
					continue;
				}
				char c2 = (char)stringReader.Read();
				if (c2 != ' ')
				{
					stringBuilder.Append('\\');
					stringBuilder.Append(c2);
				}
				else
				{
					stringBuilder.Append('\\');
					string value = "20";
					stringBuilder.Append(value);
				}
			}
			return stringBuilder.ToString();
		}

		private static string RfcChar(string dn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			int startIndex = 0;
			int num2;
			while ((num2 = dn.IndexOf("\\20", startIndex)) >= 0)
			{
				stringBuilder.Append(ReplaceDnChar(dn.Substring(num, num2 - num)) + "\\ ");
				num = num2 + 3;
				startIndex = num2 + 3;
			}
			stringBuilder.Append(dn.Substring(num));
			return stringBuilder.ToString();
		}

		public static string XmlDsigtoRFC2253(string dn)
		{
			IsRfc = false;
			return ToXml(Normalize(dn));
		}
	}
}
