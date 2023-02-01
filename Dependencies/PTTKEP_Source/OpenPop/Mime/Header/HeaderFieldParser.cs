using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using OpenPop.Common.Logging;
using OpenPop.Mime.Decode;

namespace OpenPop.Mime.Header
{
	internal static class HeaderFieldParser
	{
		public static ContentTransferEncoding ParseContentTransferEncoding(string headerValue)
		{
			if (headerValue == null)
			{
				throw new ArgumentNullException("headerValue");
			}
			switch (headerValue.Trim().ToUpperInvariant())
			{
			case "7BIT":
				return ContentTransferEncoding.SevenBit;
			case "8BIT":
				return ContentTransferEncoding.EightBit;
			case "QUOTED-PRINTABLE":
				return ContentTransferEncoding.QuotedPrintable;
			case "BASE64":
				return ContentTransferEncoding.Base64;
			case "BINARY":
				return ContentTransferEncoding.Binary;
			default:
				DefaultLogger.Log.LogDebug("Wrong ContentTransferEncoding was used. It was: " + headerValue);
				return ContentTransferEncoding.SevenBit;
			}
		}

		public static MailPriority ParseImportance(string headerValue)
		{
			if (headerValue == null)
			{
				throw new ArgumentNullException("headerValue");
			}
			switch (headerValue.ToUpperInvariant())
			{
			case "5":
			case "HIGH":
				return MailPriority.High;
			case "3":
			case "NORMAL":
				return MailPriority.Normal;
			case "1":
			case "LOW":
				return MailPriority.Low;
			default:
				DefaultLogger.Log.LogDebug("HeaderFieldParser: Unknown importance value: \"" + headerValue + "\". Using default of normal importance.");
				return MailPriority.Normal;
			}
		}

		public static ContentType ParseContentType(string headerValue)
		{
			if (headerValue == null)
			{
				throw new ArgumentNullException("headerValue");
			}
			ContentType contentType = new ContentType();
			foreach (KeyValuePair<string, string> item in Rfc2231Decoder.Decode(headerValue))
			{
				string text = item.Key.ToUpperInvariant().Trim();
				string text2 = Utility.RemoveQuotesIfAny(item.Value.Trim());
				if (text != null)
				{
					if (text != null && text.Length == 0)
					{
						if (text2.ToUpperInvariant().Equals("TEXT"))
						{
							text2 = "text/plain";
						}
						contentType.MediaType = text2;
						continue;
					}
					switch (text)
					{
					case "BOUNDARY":
						contentType.Boundary = text2;
						continue;
					case "CHARSET":
						contentType.CharSet = text2;
						continue;
					case "NAME":
						contentType.Name = EncodedWord.Decode(text2);
						continue;
					}
				}
				if (contentType.Parameters == null)
				{
					throw new Exception("The ContentType parameters property is null. This will never be thrown.");
				}
				contentType.Parameters.Add(text, text2);
			}
			return contentType;
		}

		public static ContentDisposition ParseContentDisposition(string headerValue)
		{
			if (headerValue == null)
			{
				throw new ArgumentNullException("headerValue");
			}
			ContentDisposition contentDisposition = new ContentDisposition();
			foreach (KeyValuePair<string, string> item in Rfc2231Decoder.Decode(headerValue))
			{
				string text = item.Key.ToUpperInvariant().Trim();
				string text2 = Utility.RemoveQuotesIfAny(item.Value.Trim());
				switch (text)
				{
				case "":
					contentDisposition.DispositionType = text2;
					continue;
				case "NAME":
				case "FILENAME":
					contentDisposition.FileName = EncodedWord.Decode(text2);
					continue;
				case "CREATION-DATE":
				{
					DateTime creationDate = new DateTime(Rfc2822DateTime.StringToDate(text2).Ticks);
					contentDisposition.CreationDate = creationDate;
					continue;
				}
				case "MODIFICATION-DATE":
				{
					DateTime modificationDate = new DateTime(Rfc2822DateTime.StringToDate(text2).Ticks);
					contentDisposition.ModificationDate = modificationDate;
					continue;
				}
				case "READ-DATE":
				{
					DateTime readDate = new DateTime(Rfc2822DateTime.StringToDate(text2).Ticks);
					contentDisposition.ReadDate = readDate;
					continue;
				}
				case "SIZE":
					contentDisposition.Size = SizeParser.Parse(text2);
					continue;
				}
				if (text.StartsWith("X-"))
				{
					contentDisposition.Parameters.Add(text, text2);
					continue;
				}
				throw new ArgumentException("Unknown parameter in Content-Disposition. Ask developer to fix! Parameter: " + text);
			}
			return contentDisposition;
		}

		public static string ParseId(string headerValue)
		{
			return headerValue.Trim().TrimEnd('>').TrimStart('<');
		}

		public static List<string> ParseMultipleIDs(string headerValue)
		{
			List<string> list = new List<string>();
			string[] array = headerValue.Trim().Split(new char[1] { '>' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string headerValue2 in array)
			{
				list.Add(ParseId(headerValue2));
			}
			return list;
		}
	}
}
