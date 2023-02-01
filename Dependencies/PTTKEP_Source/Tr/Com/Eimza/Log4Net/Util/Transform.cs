using System.Text.RegularExpressions;
using System.Xml;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public sealed class Transform
	{
		private const string CDATA_END = "]]>";

		private const string CDATA_UNESCAPABLE_TOKEN = "]]";

		private static Regex INVALIDCHARS = new Regex("[^\\x09\\x0A\\x0D\\x20-\\xFF\\u00FF-\\u07FF\\uE000-\\uFFFD]", RegexOptions.Compiled);

		private Transform()
		{
		}

		public static void WriteEscapedXmlString(XmlWriter writer, string textData, string invalidCharReplacement)
		{
			string text = MaskXmlInvalidCharacters(textData, invalidCharReplacement);
			int num = 12 * (1 + CountSubstrings(text, "]]>"));
			if (3 * (CountSubstrings(text, "<") + CountSubstrings(text, ">")) + 4 * CountSubstrings(text, "&") <= num)
			{
				writer.WriteString(text);
				return;
			}
			int num2 = text.IndexOf("]]>");
			if (num2 < 0)
			{
				writer.WriteCData(text);
				return;
			}
			int num3 = 0;
			while (num2 > -1)
			{
				writer.WriteCData(text.Substring(num3, num2 - num3));
				if (num2 == text.Length - 3)
				{
					num3 = text.Length;
					writer.WriteString("]]>");
					break;
				}
				writer.WriteString("]]");
				num3 = num2 + 2;
				num2 = text.IndexOf("]]>", num3);
			}
			if (num3 < text.Length)
			{
				writer.WriteCData(text.Substring(num3));
			}
		}

		public static string MaskXmlInvalidCharacters(string textData, string mask)
		{
			return INVALIDCHARS.Replace(textData, mask);
		}

		private static int CountSubstrings(string text, string substring)
		{
			int num = 0;
			int num2 = 0;
			int length = text.Length;
			int length2 = substring.Length;
			if (length == 0)
			{
				return 0;
			}
			if (length2 == 0)
			{
				return 0;
			}
			while (num2 < length)
			{
				int num3 = text.IndexOf(substring, num2);
				if (num3 == -1)
				{
					break;
				}
				num++;
				num2 = num3 + length2;
			}
			return num;
		}
	}
}
