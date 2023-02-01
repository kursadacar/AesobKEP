using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using ActiveUp.Net.Common.Rfc2047;

namespace ActiveUp.Net.Mail
{
    [Serializable]
	public class MimePart
	{
		public MimePartCollection SubParts { get; set; }

		public Message ParentMessage { get; set; }

		public NameValueCollection HeaderFieldNames { get; set; }

		public NameValueCollection HeaderFields { get; set; }

		public string ContentName
		{
			get
			{
				if (HeaderFields["content-name"] != null)
				{
					return Rfc2047Codec.Decode(HeaderFields.GetValues("content-name")[0]);
				}
				if (ContentType != null && ContentType.Parameters["name"] != null)
				{
					return Rfc2047Codec.Decode(ContentType.Parameters["name"]);
				}
				return null;
			}
			set
			{
				HeaderFields["content-name"] = value;
			}
		}

		public string ContentId
		{
			get
			{
				if (HeaderFields["content-id"] != null)
				{
					return "<" + HeaderFields.GetValues("content-id")[0].Trim('<', '>') + ">";
				}
				return null;
			}
			set
			{
				HeaderFields["content-id"] = "<" + value.Trim('<', '>') + ">";
			}
		}

		public string EmbeddedObjectLink
		{
			get
			{
				if (EmbeddedObjectContentId != null)
				{
					return "cid:" + EmbeddedObjectContentId;
				}
				return null;
			}
		}

		public string EmbeddedObjectContentId
		{
			get
			{
				if (HeaderFields["content-id"] != null)
				{
					return HeaderFields.GetValues("content-id")[0].Trim('<', '>');
				}
				return null;
			}
		}

		public string ContentDescription
		{
			get
			{
				if (HeaderFields["content-description"] != null)
				{
					return HeaderFields.GetValues("content-description")[0];
				}
				return null;
			}
			set
			{
				HeaderFields["content-description"] = value;
			}
		}

		public string TextContent { get; set; }

		public string TextContentTransferEncoded
		{
			get
			{
				if (ContentTransferEncoding == ContentTransferEncoding.SevenBits)
				{
					return TextContent;
				}
				if (ContentTransferEncoding == ContentTransferEncoding.Base64)
				{
					return Base64EncodeAndWrap();
				}
				if (IsText)
				{
					return Codec.ToQuotedPrintable(TextContent, Charset ?? "us-ascii");
				}
				if (MimeType.Contains("message/") || MimeType.Contains("image/") || MimeType.Contains("application/"))
				{
					return TextContent;
				}
				return Codec.Wrap(Convert.ToBase64String(BinaryContent), 77);
			}
		}

		public MimePart Container { get; set; }

		public string OriginalContent { get; set; }

		public ContentType ContentType { get; set; }

		public string MimeType
		{
			get
			{
				return ContentType.MimeType.ToLower();
			}
		}

		public bool IsText
		{
			get
			{
				return MimeType.Contains("text/");
			}
		}

		public string Charset
		{
			get
			{
				return ContentType.Parameters["charset"];
			}
			set
			{
				ContentType.Parameters["charset"] = value;
			}
		}

		public ContentDisposition ContentDisposition { get; set; }

		public ContentTransferEncoding ContentTransferEncoding
		{
			get
			{
				switch (HeaderFields["content-transfer-encoding"])
				{
				case "quoted-printable":
					return ContentTransferEncoding.QuotedPrintable;
				case "base64":
					return ContentTransferEncoding.Base64;
				case "8bit":
					return ContentTransferEncoding.EightBits;
				case "7bit":
					return ContentTransferEncoding.SevenBits;
				case "binary":
					return ContentTransferEncoding.Binary;
				default:
					return ContentTransferEncoding.Unknown;
				}
			}
			set
			{
				switch (value)
				{
				case ContentTransferEncoding.Binary:
					HeaderFields["content-transfer-encoding"] = "binary";
					break;
				case ContentTransferEncoding.QuotedPrintable:
					HeaderFields["content-transfer-encoding"] = "quoted-printable";
					break;
				case ContentTransferEncoding.SevenBits:
					HeaderFields["content-transfer-encoding"] = "7bit";
					break;
				case ContentTransferEncoding.EightBits:
					HeaderFields["content-transfer-encoding"] = "8bit";
					break;
				default:
					HeaderFields["content-transfer-encoding"] = "base64";
					break;
				}
			}
		}

		public byte[] BinaryContent { get; set; }

		public bool IsBinary
		{
			get
			{
				return BinaryContent.Length != 0;
			}
		}

		public string ContentLocation
		{
			get
			{
				return HeaderFields["content-location"];
			}
			set
			{
				HeaderFields["content-location"] = value;
			}
		}

		public int Size
		{
			get
			{
				if (!IsBinary)
				{
					return TextContent.Length;
				}
				return BinaryContent.Length;
			}
		}

		public string Filename
		{
			get
			{
				string text = string.Empty;
				if (HeaderFields["filename"] != null)
				{
					text = HeaderFields.GetValues("filename")[0];
				}
				else if (ContentDisposition != null && ContentDisposition.FileName != null)
				{
					text = ContentDisposition.FileName;
				}
				else if (ContentDisposition.Parameters["filename"] != null)
				{
					text = ContentDisposition.Parameters["filename"];
				}
				else if (!string.IsNullOrEmpty(ContentName))
				{
					text = ContentName;
				}
				text = text.Replace("\"", string.Empty);
				return Codec.RFC2047Decode(text);
			}
			set
			{
				if (HeaderFields["filename"] != null)
				{
					HeaderFields["filename"] = value;
				}
				else
				{
					AddHeaderField("filename", value);
				}
				ContentDisposition.FileName = value;
			}
		}

		public MimePart()
		{
			ContentDisposition = new ContentDisposition();
			ContentType = new ContentType();
			BinaryContent = new byte[0];
			HeaderFields = new NameValueCollection();
			HeaderFieldNames = new NameValueCollection();
			SubParts = new MimePartCollection();
		}

		public MimePart(byte[] attachment, string fileExtension)
			: this(attachment, MimeTypesHelper.GetMimeqType(fileExtension), fileExtension)
		{
		}

		public MimePart(string path, bool generateContentId, string charset = null)
			: this(File.ReadAllBytes(path), MimeTypesHelper.GetMimeqType(Path.GetExtension(path)), Path.GetFileName(path), charset)
		{
			if (generateContentId)
			{
				SetContentId();
			}
		}

		public MimePart(string path, string contentId, string charset = null)
			: this(File.ReadAllBytes(path), MimeTypesHelper.GetMimeqType(Path.GetExtension(path)), Path.GetFileName(path), charset)
		{
			ContentId = contentId;
		}

		public MimePart(byte[] content, string mimeType, string fileName, string charset = null)
			: this()
		{
			BinaryContent = content;
			ContentType.MimeType = mimeType;
			ContentDisposition.FileName = fileName;
			ContentName = fileName;
			BuildTextContent(charset);
		}

		private void BuildTextContent(string charset = null)
		{
			if (IsText)
			{
				Charset = charset;
				ContentTransferEncoding = ContentTransferEncoding.QuotedPrintable;
				TextContent = Encoding.GetEncoding(charset ?? Encoding.UTF8.BodyName).GetString(BinaryContent, 0, BinaryContent.Length);
			}
			else
			{
				ContentTransferEncoding = ContentTransferEncoding.Base64;
				TextContent = Convert.ToBase64String(BinaryContent);
			}
		}

		public void SetContentId()
		{
			ContentId = "AMLv2" + Codec.GetUniqueString() + "@" + System.Net.Dns.GetHostName();
		}

		public void SetContentId(string contentID)
		{
			ContentId = contentID;
		}

		public string GetCidReference()
		{
			return "cid:" + ContentId.Trim('<', '>');
		}

		public string StoreToFile(string destinationPath)
		{
			File.WriteAllBytes(destinationPath, BinaryContent);
			return destinationPath;
		}

		public string ToMimeString(bool forceBase64Encoding = false)
		{
			string empty = string.Empty;
			if (ContentType.Type != "multipart")
			{
				empty = (forceBase64Encoding ? Base64EncodeAndWrap() : TextContentTransferEncoded);
				return GetHeaderString(forceBase64Encoding) + "\r\n" + empty + "\r\n";
			}
			InitializeBoundaryIfNotProvided();
			string text = ContentType.Parameters["boundary"];
			empty += GetHeaderString(forceBase64Encoding);
			foreach (MimePart subPart in SubParts)
			{
				empty = empty + "\r\n--" + text + "\r\n";
				empty += subPart.ToMimeString(forceBase64Encoding);
			}
			return empty + "\r\n\r\n--" + text + "--\r\n";
		}

		private void InitializeBoundaryIfNotProvided()
		{
			if (string.IsNullOrEmpty(ContentType.Parameters["boundary"]))
			{
				string value = "---=_Part_" + Codec.GetUniqueString();
				ContentType.Parameters.Add("boundary", value);
			}
		}

		public static MimePart GetSignaturePart(SignedCms cms)
		{
			return new MimePart
			{
				ContentType = 
				{
					MimeType = "application/x-pkcs7-signature",
					Parameters = { { "name", "\"smime.p7s\"" } }
				},
				ContentTransferEncoding = ContentTransferEncoding.Base64,
				ContentDisposition = 
				{
					Disposition = "attachment",
					FileName = "smime.p7s"
				},
				BinaryContent = cms.Encode()
			};
		}

		public string GetHeaderString(bool forceBase64Encoding = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			ContentType contentType = ContentType;
			stringBuilder.Append(((contentType != null) ? contentType.ToString() : null) + "\r\n");
			if (ContentDisposition.Disposition.Length > 0)
			{
				ContentDisposition contentDisposition = ContentDisposition;
				stringBuilder.Append(((contentDisposition != null) ? contentDisposition.ToString() : null) + "\r\n");
			}
			AppendContentEncoding(stringBuilder, forceBase64Encoding);
			IEnumerable<string> headerNames = HeaderFields.AllKeys.Except(new string[3] { "content-type", "content-disposition", "content-transfer-encoding" });
			AppendGivenHeaderFields(stringBuilder, headerNames);
			return stringBuilder.ToString().Trim('\r', '\n') + "\r\n";
		}

		private void AppendContentEncoding(StringBuilder builder, bool forceBase64Encoding)
		{
			string arg = (forceBase64Encoding ? "base64" : HeaderFields["content-transfer-encoding"]);
			builder.AppendFormat("{0}: {1}\r\n", Codec.GetFieldName("content-transfer-encoding"), arg);
		}

		private void AppendGivenHeaderFields(StringBuilder builder, IEnumerable<string> headerNames)
		{
			foreach (string headerName in headerNames)
			{
				builder.AppendFormat("{0}: {1}\r\n", Codec.GetFieldName(headerName), HeaderFields[headerName]);
			}
		}

		private string Base64EncodeAndWrap()
		{
			if (IsBinary)
			{
				return Codec.Wrap(Convert.ToBase64String(BinaryContent), 78);
			}
			return Codec.Wrap(Convert.ToBase64String(Encoding.GetEncoding(Charset ?? Encoding.UTF8.BodyName).GetBytes(TextContent ?? string.Empty)), 78);
		}

		private void AddHeaderField(string name, string value)
		{
			string name2 = name.ToLower();
			HeaderFields[name2] = value;
			HeaderFieldNames[name2] = name;
		}
	}
}
