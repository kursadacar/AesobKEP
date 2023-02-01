using System;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.RegularExpressions;
using ActiveUp.Net.Security;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class Parser
	{
		public delegate void OnBodyParsedEvent(object sender, Message message);

		public delegate void OnHeaderFieldParsingEvent(object sender, Header header);

		public delegate void OnErrorParsingEvent(object sender, Exception ex);

		private static Encoding defaultEncoding = Encoding.GetEncoding("iso-8859-1");

		public static event OnBodyParsedEvent BodyParsed;

		public static event OnHeaderFieldParsingEvent HeaderFieldParsing;

		public static event OnErrorParsingEvent ErrorParsing;

		internal static int GetMonth(string month)
		{
			switch (month)
			{
			case "Jan":
				return 1;
			case "Feb":
				return 2;
			case "Mar":
				return 3;
			case "Apr":
				return 4;
			case "May":
				return 5;
			case "Jun":
				return 6;
			case "Jul":
				return 7;
			case "Aug":
				return 8;
			case "Sep":
				return 9;
			case "Oct":
				return 10;
			case "Nov":
				return 11;
			case "Dec":
				return 12;
			default:
				return -1;
			}
		}

		internal static string InvGetMonth(int month)
		{
			switch (month)
			{
			case 1:
				return "Jan";
			case 2:
				return "Feb";
			case 3:
				return "Mar";
			case 4:
				return "Apr";
			case 5:
				return "May";
			case 6:
				return "Jun";
			case 7:
				return "Jul";
			case 8:
				return "Aug";
			case 9:
				return "Sep";
			case 10:
				return "Oct";
			case 11:
				return "Nov";
			case 12:
				return "Dec";
			default:
				return "???";
			}
		}

		private static ContentType GetContentType(string input)
		{
			ContentType contentType = new ContentType();
			contentType.MimeType = Regex.Match(input, "(?<=: ?)\\S+?(?=([;\\s]|\\Z))").Value;
			Match match = Regex.Match(input, "(?<=;\\s*)[^;\\s?]*=[^;]*(?=(;|\\Z))");
			while (match.Success)
			{
				contentType.Parameters.Add(FormatFieldName(match.Value.Substring(0, match.Value.IndexOf('='))).ToLower(), match.Value.Substring(match.Value.IndexOf('=') + 1).Replace("\"", "").Trim('\r', '\n'));
				match = match.NextMatch();
			}
			return contentType;
		}

		private static ContentDisposition GetContentDisposition(string input)
		{
			ContentDisposition contentDisposition = new ContentDisposition();
			contentDisposition.Disposition = Regex.Match(input.Replace("\t", ""), "(?<=: ?)\\S+?(?=([;\\s]|\\Z))").Value;
			Match match = Regex.Match(input.Replace("\t", ""), "(?<=;[ \\t]?)[^;]*=[^;]*(?=(;|\\Z))");
			while (match.Success)
			{
				contentDisposition.Parameters.Add(FormatFieldName(match.Value.Substring(0, match.Value.IndexOf('='))), match.Value.Substring(match.Value.IndexOf('=') + 1).Replace("\"", "").Trim('\r', '\n'));
				match = match.NextMatch();
			}
			return contentDisposition;
		}

		private static void ParseSubParts(ref MimePart part, Message message)
		{
			string text = part.ContentType.Parameters["boundary"];
			string @string = Encoding.ASCII.GetString(part.BinaryContent);
			byte[] array = part.BinaryContent;
			Logger.AddEntry("boundary : " + text);
			string[] array2 = Regex.Split(@string, "\\r?\\n?" + Regex.Escape("--" + text));
			for (int i = 1; i < array2.Length - 1; i++)
			{
				string text2 = array2[i];
				int byteCount = Encoding.ASCII.GetByteCount(@string.Substring(0, @string.IndexOf(text2)));
				byte[] array3 = new byte[byteCount + Encoding.ASCII.GetByteCount(text2.ToCharArray())];
				Array.Copy(array, array3, array3.Length);
				byte[] array4 = new byte[Encoding.ASCII.GetByteCount(text2)];
				Array.Copy(array3, byteCount, array4, 0, array4.Length);
				byte[] array5 = new byte[array.Length - array3.Length];
				Array.Copy(array, array3.Length, array5, 0, array.Length - array3.Length);
				array = array5;
				@string = Encoding.ASCII.GetString(array);
				if (!text2.StartsWith("--") && !string.IsNullOrEmpty(text2))
				{
					MimePart mimePart = ParseMimePart(array4, message);
					mimePart.ParentMessage = message;
					mimePart.Container = part;
					part.SubParts.Add(mimePart);
				}
			}
		}

		private static void DispatchParts(MimePart root, ref Message message)
		{
			foreach (MimePart subPart in root.SubParts)
			{
				DispatchPart(subPart, ref message);
			}
		}

		internal static void DispatchParts(ref Message message)
		{
			DispatchPart(message.PartTreeRoot, ref message);
		}

		private static void DispatchPart(MimePart part, ref Message message)
		{
			if (part.SubParts.Count > 0)
			{
				DispatchParts(part, ref message);
				return;
			}
			if (part.ContentDisposition.Disposition.Equals("attachment"))
			{
				message.Attachments.Add(part);
			}
			else if (part.ContentDisposition.Disposition.Equals("inline"))
			{
				message.EmbeddedObjects.Add(part);
			}
			else if (!message.BodyText.ToMimePart().ContentTransferEncoding.Equals(part.ContentTransferEncoding))
			{
				message.UnknownDispositionMimeParts.Add(part);
			}
			if (part.ContentType.Type.Equals("text") && !part.ContentDisposition.Disposition.Equals("attachment"))
			{
				if (part.ContentType.SubType.Equals("plain"))
				{
					message.BodyText.Charset = part.Charset;
					message.BodyText.Text = part.TextContent;
				}
				else if (part.ContentType.SubType.Equals("html"))
				{
					message.IsHtml = true;
					message.BodyHtml.Charset = part.Charset;
					message.BodyHtml.Text = part.TextContent;
				}
				else if (part.ContentType.SubType.Equals("xml"))
				{
					message.Attachments.Add(part);
				}
			}
			if (part.ContentType.MimeType.Equals("message/rfc822"))
			{
				message.SubMessages.Add(ParseMessage(part.BinaryContent));
			}
			if (part.ContentType.MimeType.Equals("application/pkcs7-signature") || part.ContentType.MimeType.Equals("application/x-pkcs7-signature"))
			{
				string textContent = part.Container.TextContent;
				textContent = Regex.Split(textContent, "\r\n--" + part.Container.ContentType.Parameters["boundary"])[1];
				textContent = textContent.TrimStart('\r', '\n');
				message.Signatures.Smime = new SignedCms(new ContentInfo(Encoding.ASCII.GetBytes(textContent)), true);
				message.Signatures.Smime.Decode(part.BinaryContent);
			}
			message.LeafMimeParts.Add(part);
		}

		private static void DecodePartBody(ref MimePart part)
		{
			string text = ((!string.IsNullOrEmpty(part.Charset)) ? part.Charset : "iso-8859-1");
			if (part.ContentTransferEncoding.Equals(ContentTransferEncoding.Base64))
			{
				part.TextContent = Encoding.ASCII.GetString(part.BinaryContent);
				try
				{
					part.BinaryContent = Convert.FromBase64String(part.TextContent);
				}
				catch (FormatException)
				{
					part.TextContent = part.TextContent.Remove(part.TextContent.LastIndexOf("=") + 1);
					part.BinaryContent = Convert.FromBase64String(part.TextContent);
				}
				part.TextContent = Encoding.ASCII.GetString(part.BinaryContent);
				if (part.ContentDisposition != 2)
				{
					part.TextContent = Codec.GetEncoding(text).GetString(part.BinaryContent, 0, part.BinaryContent.Length);
				}
			}
			else if (part.ContentTransferEncoding.Equals(ContentTransferEncoding.QuotedPrintable))
			{
				part.TextContent = Encoding.ASCII.GetString(part.BinaryContent);
				part.TextContent = Codec.FromQuotedPrintable(part.TextContent, text);
				part.BinaryContent = Codec.GetEncoding(text).GetBytes(part.TextContent);
			}
			else
			{
				part.TextContent = Codec.GetEncoding(text).GetString(part.BinaryContent);
			}
		}

		private static string ReplaceTimeZone(string input)
		{
			input = input.Replace("EDT", "-0400");
			input = input.Replace("EST", "-0500");
			input = input.Replace("CDT", "-0500");
			input = input.Replace("CST", "-0600");
			input = input.Replace("MDT", "-0600");
			input = input.Replace("MST", "-0700");
			input = input.Replace("PDT", "-0700");
			input = input.Replace("PST", "-0800");
			input = input.Replace("UT", "+0000");
			input = input.Replace("GMT", "+0000");
			return input;
		}

		internal static string RemoveWhiteSpaces(string input)
		{
			return Regex.Replace(input, "\\s+", "");
		}

		internal static string FormatFieldName(string fieldName)
		{
			return fieldName.ToLower().Trim();
		}

		internal static string Clean(string input)
		{
			return Regex.Replace(input, "(\\(((\\\\\\))|[^)])*\\))", "").Trim(' ');
		}

		public static string Fold(string input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = input.Split(' ');
			string text = string.Empty;
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				if (text.Length + text2.Length < 77)
				{
					text = text + text2 + " ";
					continue;
				}
				stringBuilder.Append(text + "\r\n ");
				text = string.Empty;
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		public static string Unfold(string input)
		{
			return Regex.Replace(input, "\\r?\\n(?=[ \\t])", "");
		}

		public static MimePart ParseMimePart(byte[] binaryData, Message message)
		{
			MimePart part = new MimePart();
			string @string = Encoding.ASCII.GetString(binaryData);
			part.ParentMessage = message;
			part.OriginalContent = @string;
			try
			{
				int num = Regex.Match(@string, ".(?=\\r?\\n\\r?\\n)").Index + 1;
				int num2 = Regex.Match(@string, "(?<=\\r?\\n\\r?\\n).").Index;
				if (num2 == 0)
				{
					int num3 = @string.IndexOf("\r\n\r\n");
					if (num3 > 0)
					{
						num2 = num3;
					}
				}
				if (@string.Length >= num)
				{
					string input = Unfold(@string.Substring(0, num));
					string empty = string.Empty;
					if (num2 < @string.Length)
					{
						empty = @string.Substring(num2);
						part.BinaryContent = GetBinaryPart(binaryData, empty);
					}
					Match match = Regex.Match(input, "(?<=((\\r?\\n)|\\n)|\\A)\\S+:(.|(\\r?\\n[\\t ]))+(?=((\\r?\\n)\\S)|\\Z)");
					while (match.Success)
					{
						if (match.Value.ToLower().StartsWith("content-type:"))
						{
							part.ContentType = GetContentType(match.Value);
						}
						else if (match.Value.ToLower().StartsWith("content-disposition:"))
						{
							part.ContentDisposition = GetContentDisposition(match.Value);
						}
						part.HeaderFields.Add(FormatFieldName(match.Value.Substring(0, match.Value.IndexOf(':'))), Codec.RFC2047Decode(match.Value.Substring(match.Value.IndexOf(':') + 1).Trim(' ', '\r', '\n')));
						part.HeaderFieldNames.Add(FormatFieldName(match.Value.Substring(0, match.Value.IndexOf(':'))), Codec.RFC2047Decode(match.Value.Substring(0, match.Value.IndexOf(':')).Trim(' ', '\r', '\n')));
						match = match.NextMatch();
					}
					if (part.ContentType.Type.ToLower().Equals("multipart"))
					{
						ParseSubParts(ref part, message);
					}
					else
					{
						part.ContentType.Type.ToLower().Equals("message");
					}
					DecodePartBody(ref part);
					try
					{
						Parser.BodyParsed(null, message);
						return part;
					}
					catch (Exception)
					{
						return part;
					}
				}
				return part;
			}
			catch (Exception ex2)
			{
				throw new ParsingException(ex2.Message);
			}
		}

		private static byte[] GetBinaryPart(byte[] srcData, string asciiPart)
		{
			byte[] array = new byte[Encoding.ASCII.GetByteCount(asciiPart.ToCharArray())];
			Array.Copy(srcData, srcData.Length - array.Length, array, 0, array.Length);
			return array;
		}

		public static Header ParseHeader(string filePath)
		{
			FileStream fileStream = File.OpenRead(filePath);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
			fileStream.Close();
			Header header = new Header();
			header.OriginalData = array;
			ParseHeader(ref header);
			return header;
		}

		public static Header ParseHeader(MemoryStream inputStream)
		{
			byte[] array = new byte[inputStream.Length];
			inputStream.Read(array, 0, array.Length);
			Header header = new Header();
			header.OriginalData = array;
			ParseHeader(ref header);
			return header;
		}

		public static Header ParseHeader(byte[] data)
		{
			Header header = new Header();
			header.OriginalData = data;
			ParseHeader(ref header);
			return header;
		}

		public static void ParseHeader(ref Header header)
		{
			Match match = Regex.Match(Unfold(Regex.Match(Encoding.GetEncoding("iso-8859-1").GetString(header.OriginalData, 0, header.OriginalData.Length), "[\\s\\S]+?((?=\\r?\\n\\r?\\n)|\\Z)").Value), "(?<=((\\r?\\n)|\\n)|\\A)\\S+:(.|(\\r?\\n[\\t ]))+(?=((\\r?\\n)\\S)|\\Z)");
			while (match.Success)
			{
				string text = FormatFieldName(match.Value.Substring(0, match.Value.IndexOf(':')));
				string text2 = Codec.RFC2047Decode(match.Value.Substring(match.Value.IndexOf(":") + 1)).Trim('\r', '\n').TrimStart(' ');
				if (text.Equals("received"))
				{
					header.Trace.Add(ParseTrace(match.Value.Trim(' ')));
				}
				else if (text.Equals("to"))
				{
					header.To = ParseAddresses(text2);
				}
				else if (text.Equals("cc"))
				{
					header.Cc = ParseAddresses(text2);
				}
				else if (text.Equals("bcc"))
				{
					header.Bcc = ParseAddresses(text2);
				}
				else if (text.Equals("reply-to"))
				{
					header.ReplyTo = ParseAddress(text2);
				}
				else if (text.Equals("from"))
				{
					header.From = ParseAddress(text2);
				}
				else if (text.Equals("sender"))
				{
					header.Sender = ParseAddress(text2);
				}
				else if (text.Equals("content-type"))
				{
					header.ContentType = GetContentType(match.Value);
				}
				else if (text.Equals("content-disposition"))
				{
					header.ContentDisposition = GetContentDisposition(match.Value);
				}
				header.HeaderFields.Add(text, text2);
				header.HeaderFieldNames.Add(text, match.Value.Substring(0, match.Value.IndexOf(':')));
				match = match.NextMatch();
				if (Parser.HeaderFieldParsing != null)
				{
					Parser.HeaderFieldParsing(null, header);
				}
			}
		}

		public static Header ParseHeaderString(string data)
		{
			return ParseHeader(Encoding.GetEncoding("iso-8859-1").GetBytes(data));
		}

		public static Message ParseMessage(byte[] data)
		{
			Message message = new Message();
			try
			{
				MimePart mimePart = ParseMimePart(data, message);
				message.OriginalData = data;
				message.HeaderFields = mimePart.HeaderFields;
				message.HeaderFieldNames = mimePart.HeaderFieldNames;
				string[] allKeys = message.HeaderFields.AllKeys;
				foreach (string text in allKeys)
				{
					string text2 = text;
					string text3 = message.HeaderFields[text];
					if (text2.Equals("received"))
					{
						Match match = Regex.Match(text3, "from.+?(?=(from|$))");
						while (match.Success)
						{
							message.Trace.Add(ParseTrace(text + ": " + match.Value));
							match = match.NextMatch();
						}
					}
					else if (text2.Equals("to"))
					{
						message.To = ParseAddresses(text3);
					}
					else if (text2.Equals("cc"))
					{
						message.Cc = ParseAddresses(text3);
					}
					else if (text2.Equals("bcc"))
					{
						message.Bcc = ParseAddresses(text3);
					}
					else if (text2.Equals("reply-to"))
					{
						message.ReplyTo = ParseAddress(text3);
					}
					else if (text2.Equals("from"))
					{
						message.From = ParseAddress(text3);
					}
					else if (text2.Equals("sender"))
					{
						message.Sender = ParseAddress(text3);
					}
					else if (text2.Equals("content-type"))
					{
						message.ContentType = GetContentType(text + ": " + text3);
					}
					else if (text2.Equals("content-disposition"))
					{
						message.ContentDisposition = GetContentDisposition(text + ": " + text3);
					}
					else if (text2.Equals("domainkey-signature"))
					{
						message.Signatures.DomainKeys = Signature.Parse(text + ": " + text3, message);
					}
				}
				if (message.ContentType.MimeType.Equals("application/pkcs7-mime") || message.ContentType.MimeType.Equals("application/x-pkcs7-mime"))
				{
					if (message.ContentType.Parameters["smime-type"] != null && message.ContentType.Parameters["smime-type"].Equals("enveloped-data"))
					{
						message.IsSmimeEncrypted = true;
					}
					if (message.ContentType.Parameters["smime-type"] != null && message.ContentType.Parameters["smime-type"].Equals("signed-data"))
					{
						message.HasSmimeSignature = true;
					}
				}
				if (message.ContentType.MimeType.Equals("multipart/signed"))
				{
					message.HasSmimeDetachedSignature = true;
				}
				message.PartTreeRoot = mimePart;
				DispatchParts(ref message);
				return message;
			}
			catch (Exception ex)
			{
				if (Parser.ErrorParsing != null)
				{
					Parser.ErrorParsing(null, ex);
					return message;
				}
				return message;
			}
		}

		public static Message ParseMessage(MemoryStream inputStream)
		{
			byte[] array = new byte[inputStream.Length];
			inputStream.Read(array, 0, array.Length);
			Message result = new Message
			{
				OriginalData = array
			};
			ParseMessage(array);
			return result;
		}

		public static Message ParseMessage(string data)
		{
			return ParseMessage(Encoding.GetEncoding("iso-8859-1").GetBytes(data));
		}

		public static Message ParseMessageFromFile(string filePath)
		{
			FileStream fileStream = File.OpenRead(filePath);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
			fileStream.Close();
			return ParseMessage(array);
		}

		public static AddressCollection ParseAddresses(string input)
		{
			AddressCollection addressCollection = new AddressCollection();
			string[] array = input.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IndexOf("@") == -1 && array.Length > i + 1)
				{
					array[i + 1] = array[i] + array[i + 1];
				}
			}
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text.IndexOf("@") != -1)
				{
					addressCollection.Add(ParseAddress((text.IndexOf("<") == -1 || text.IndexOf(":") == -1 || text.IndexOf(":") >= text.IndexOf("<")) ? text : ((text.Split(':')[0].IndexOf("\"") == -1) ? text.Split(':')[1] : text)));
				}
			}
			return addressCollection;
		}

		public static Address ParseAddress(string input)
		{
			input = input.TrimEnd(';');
			try
			{
				if (!input.Contains("<"))
				{
					return new Address
					{
						Email = RemoveWhiteSpaces(input)
					};
				}
				Address address = null;
				Match match = Regex.Match(input, "(\"?(.+)(\"?(?=\\s?<)|(?=<)))");
				address = ((!match.Success) ? new Address(input.Trim().Trim('<', '>'), string.Empty) : new Address(input.Replace(match.Value, string.Empty).Trim().Trim('<', '>'), match.Groups[1].Value));
				CleanupAddress(address);
				return address;
			}
			catch
			{
				return new Address
				{
					Email = input
				};
			}
		}

		private static void CleanupAddress(Address address)
		{
			address.Email = Clean(RemoveWhiteSpaces(address.Email)).Replace("\\\"", string.Empty).Replace("\"", string.Empty);
			if (!address.Name.Contains("\""))
			{
				address.Name = Clean(address.Name);
			}
			address.Name = address.Name.Trim(' ', '"');
		}

		public static DateTime ParseAsUniversalDateTime(string input)
		{
			try
			{
				input = ReplaceTimeZone(input);
				input = Clean(input);
				input = Regex.Replace(input, " +", " ");
				input = Regex.Replace(input, "( +: +)|(: +)|( +:)", ":");
				if (input.Contains(","))
				{
					input = input.Replace(input.Split(',')[0] + ", ", "");
				}
				string[] array = input.Replace("\t", string.Empty).Split(' ');
				int num = Convert.ToInt32(array[2]);
				if (num < 100)
				{
					num = ((num <= 49) ? (num + 2000) : (num + 1900));
				}
				int month = GetMonth(array[1]);
				int day = Convert.ToInt32(array[0]);
				string[] array2 = array[3].Split(':');
				int hour = Convert.ToInt32(array2[0]);
				int minute = Convert.ToInt32(array2[1]);
				int second = 0;
				if (array2.Length > 2)
				{
					second = Convert.ToInt32(array2[2]);
				}
				int num2 = Convert.ToInt32(array[4].Substring(0, 3));
				int num3 = Convert.ToInt32(array[4].Substring(3, 2));
				return new DateTime(num, month, day, hour, minute, second).AddHours(-num2).AddMinutes(-num3);
			}
			catch (Exception)
			{
				return DateTime.MinValue;
			}
		}

		public static TraceInfo ParseTrace(string input)
		{
			TraceInfo traceInfo = new TraceInfo();
			Match match = Regex.Match(input, "from.+?(?=(from|by|via|with|for|id|;|\\r?\\n))");
			if (match.Success)
			{
				traceInfo.From = match.Value.Trim(' ', '\t');
			}
			match = Regex.Match(input, "(?<=by ).+?(?= ?(from|by|via|with|for|id|;|\\r?\\n))");
			if (match.Success)
			{
				traceInfo.By = match.Value.Trim(' ', '\t');
			}
			match = Regex.Match(input, "(?<=via ).+?(?= ?(from|by|via|with|for|id|;|\\r?\\n))");
			if (match.Success)
			{
				traceInfo.Via = match.Value.Trim(' ', '\t');
			}
			match = Regex.Match(input, "(?<=with ).+?(?= ?(from|by|via|with|for|id|;|\\r?\\n))");
			if (match.Success)
			{
				traceInfo.With = match.Value.Trim(' ', '\t');
			}
			match = Regex.Match(input, "(?<=for ).+?(?= ?(from|by|via|with|for|id|;|\\r?\\n))");
			if (match.Success)
			{
				traceInfo.For = match.Value.Trim(' ', '\t');
			}
			match = Regex.Match(input, "(?<=id ).+?(?= ?(from|by|via|with|for|id|;|\\r?\\n))");
			if (match.Success)
			{
				traceInfo.Id = match.Value.Trim(' ', '\t');
			}
			traceInfo.Date = ParseAsUniversalDateTime(input.Substring(input.LastIndexOf(';') + 1));
			return traceInfo;
		}

		public static TraceInfoCollection ParseTraces(string[] input)
		{
			TraceInfoCollection traceInfoCollection = new TraceInfoCollection();
			TraceInfo traceInfo = new TraceInfo();
			try
			{
				foreach (string text in input)
				{
					traceInfo = new TraceInfo();
					string text2 = text.ToLower();
					if (text2.IndexOf(" from ") != -1)
					{
						traceInfo.From = text.Substring(text2.IndexOf(" from ") + 6, text.IndexOf(" ", text2.IndexOf(" from ") + 6) - (text2.IndexOf(" from ") + 6)).TrimEnd(';');
					}
					if (text2.IndexOf(" by ") != -1)
					{
						traceInfo.By = text.Substring(text2.IndexOf(" by ") + 4, text.IndexOf(" ", text2.IndexOf(" by ") + 4) - (text2.IndexOf(" by ") + 4)).TrimEnd(';');
					}
					if (text2.IndexOf(" for ") != -1)
					{
						traceInfo.For = text.Substring(text2.IndexOf(" for ") + 5, text.IndexOf(" ", text2.IndexOf(" for ") + 5) - (text2.IndexOf(" for ") + 5)).TrimEnd(';');
					}
					if (text2.IndexOf(" id ") != -1)
					{
						traceInfo.Id = text.Substring(text2.IndexOf(" id ") + 4, text.IndexOf(" ", text2.IndexOf(" id ") + 4) - (text2.IndexOf(" id ") + 4)).TrimEnd(';');
					}
					if (text2.IndexOf(" via ") != -1)
					{
						traceInfo.Via = text.Substring(text2.IndexOf(" via ") + 5, text.IndexOf(" ", text2.IndexOf(" via ") + 5) - (text2.IndexOf(" via ") + 5)).TrimEnd(';');
					}
					if (text2.IndexOf(" with ") != -1)
					{
						traceInfo.With = text.Substring(text2.IndexOf(" with ") + 6, text.IndexOf(" ", text2.IndexOf(" with ") + 6) - (text2.IndexOf(" with ") + 6)).TrimEnd(';');
					}
					traceInfo.Date = ParseAsUniversalDateTime(text.Split(';')[text.Split(';').Length - 1].Trim(' '));
					traceInfoCollection.Add(traceInfo);
				}
				return traceInfoCollection;
			}
			catch
			{
				return traceInfoCollection;
			}
		}
	}
}
