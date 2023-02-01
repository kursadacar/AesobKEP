using System;
using System.Text.RegularExpressions;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class MimeBody
	{
		private string _charset = "iso-8859-1";

		private string _text = string.Empty;

		private ContentTransferEncoding _encoding = ContentTransferEncoding.QuotedPrintable;

		private BodyFormat _format;

		public BodyFormat Format
		{
			get
			{
				return _format;
			}
			set
			{
				_format = value;
			}
		}

		public string Charset
		{
			get
			{
				return _charset;
			}
			set
			{
				_charset = value;
			}
		}

		public string Text
		{
			get
			{
				return _text.Replace("FLAGS (Seen)", string.Empty);
			}
			set
			{
				_text = value;
			}
		}

		public string TextStripped
		{
			get
			{
				string input = _text.Replace("</p>", "\n\n").Replace("</P>", "\n\n").Replace("<br>", "\n")
					.Replace("<BR>", "\n")
					.Replace("&nbsp;", " ");
				return new Regex("<[^>]*>").Replace(input, string.Empty);
			}
		}

		public ContentTransferEncoding ContentTransferEncoding
		{
			get
			{
				return _encoding;
			}
			set
			{
				_encoding = value;
			}
		}

		public MimeBody(BodyFormat format)
		{
			_format = format;
		}

		public MimePart ToMimePart()
		{
			MimePart mimePart = new MimePart();
			if (Format.Equals(BodyFormat.Text))
			{
				mimePart.ContentType.MimeType = "text/plain";
			}
			else if (Format.Equals(BodyFormat.Html))
			{
				mimePart.ContentType.MimeType = "text/html";
			}
			mimePart.ContentType.Parameters.Add("charset", Charset);
			mimePart.ContentTransferEncoding = ContentTransferEncoding;
			mimePart.TextContent = Text;
			return mimePart;
		}
	}
}
