using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OpenPop.Mime.Decode
{
	public static class EncodingFinder
	{
		public delegate Encoding FallbackDecoderDelegate(string characterSet);

		public static FallbackDecoderDelegate FallbackDecoder { private get; set; }

		private static Dictionary<string, Encoding> EncodingMap { get; set; }

		static EncodingFinder()
		{
			Reset();
		}

		internal static void Reset()
		{
			EncodingMap = new Dictionary<string, Encoding>();
			FallbackDecoder = null;
			AddMapping("utf8", Encoding.UTF8);
		}

		internal static Encoding FindEncoding(string characterSet)
		{
			if (characterSet == null)
			{
				throw new ArgumentNullException("characterSet");
			}
			string text = characterSet.ToUpperInvariant();
			if (EncodingMap.ContainsKey(text))
			{
				return EncodingMap[text];
			}
			try
			{
				if (text.Contains("WINDOWS") || text.Contains("CP"))
				{
					text = text.Replace("CP", "");
					text = text.Replace("WINDOWS", "");
					text = text.Replace("-", "");
					return Encoding.GetEncoding(int.Parse(text, CultureInfo.InvariantCulture));
				}
				return Encoding.GetEncoding(characterSet);
			}
			catch (ArgumentException)
			{
				if (FallbackDecoder == null)
				{
					throw;
				}
				Encoding encoding = FallbackDecoder(characterSet);
				if (encoding != null)
				{
					return encoding;
				}
				throw;
			}
		}

		public static void AddMapping(string characterSet, Encoding encoding)
		{
			if (characterSet == null)
			{
				throw new ArgumentNullException("characterSet");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			EncodingMap.Add(characterSet.ToUpperInvariant(), encoding);
		}
	}
}
