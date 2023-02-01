using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class Header
	{
		private int _indexOnServer;

		private int _id = -1;

		private AddressCollection _to = new AddressCollection();

		private AddressCollection _cc = new AddressCollection();

		private AddressCollection _bcc = new AddressCollection();

		private AddressCollection _recipients = new AddressCollection();

		private Address _from = new Address();

		private List<TraceInfo> _trace = new List<TraceInfo>();

		private Address _sender = new Address();

		private Address _replyto = new Address();

		private ContentType _contentType = new ContentType();

		private ContentDisposition _contentDisposition = new ContentDisposition();

		private NameValueCollection _fieldNames = new NameValueCollection();

		private NameValueCollection _fields = new NameValueCollection();

		private byte[] _data;

		public byte[] OriginalData
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}

		public List<TraceInfo> Trace
		{
			get
			{
				return _trace;
			}
			set
			{
				_trace = value;
			}
		}

		public IPAddress SenderIP
		{
			get
			{
				if (Trace.Count > 0 && Regex.IsMatch(Trace[0].From, "\\b(?:[0-9]{1,3}\\.){3}[0-9]{1,3}\\b"))
				{
					return IPAddress.Parse(Regex.Match(Trace[0].From, "\\b(?:[0-9]{1,3}\\.){3}[0-9]{1,3}\\b").Value);
				}
				return null;
			}
		}

		public AddressCollection To
		{
			get
			{
				return _to;
			}
			set
			{
				_to = value;
			}
		}

		public NameValueCollection HeaderFieldNames
		{
			get
			{
				return _fieldNames;
			}
			set
			{
				_fieldNames = value;
			}
		}

		public NameValueCollection HeaderFields
		{
			get
			{
				return _fields;
			}
			set
			{
				_fields = value;
			}
		}

		public AddressCollection Cc
		{
			get
			{
				return _cc;
			}
			set
			{
				_cc = value;
			}
		}

		public AddressCollection Bcc
		{
			get
			{
				return _bcc;
			}
			set
			{
				_bcc = value;
			}
		}

		public AddressCollection Recipients
		{
			get
			{
				return _recipients;
			}
			set
			{
				_recipients = value;
			}
		}

		public Address Sender
		{
			get
			{
				return _sender;
			}
			set
			{
				_sender = value;
			}
		}

		public Address From
		{
			get
			{
				return _from;
			}
			set
			{
				_from = value;
			}
		}

		public Address ReplyTo
		{
			get
			{
				return _replyto;
			}
			set
			{
				_replyto = value;
			}
		}

		public string Subject
		{
			get
			{
				string empty = string.Empty;
				if (HeaderFields["subject"] == null)
				{
					return null;
				}
				return Codec.RFC2047Decode(HeaderFields.GetValues("subject")[0]);
			}
			set
			{
				AddHeaderField("Subject", value);
			}
		}

		public string InReplyTo
		{
			get
			{
				if (HeaderFields["in-reply-to"] != null)
				{
					return Parser.Clean(Parser.RemoveWhiteSpaces(HeaderFields.GetValues("in-reply-to")[0]));
				}
				return null;
			}
			set
			{
				AddHeaderField("In-Reply-To", value);
			}
		}

		public string References
		{
			get
			{
				if (HeaderFields["references"] != null)
				{
					return HeaderFields.GetValues("references")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("References", value);
			}
		}

		public string Comments
		{
			get
			{
				if (HeaderFields["comments"] != null)
				{
					return HeaderFields.GetValues("comments")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Comments", value);
			}
		}

		public string Keywords
		{
			get
			{
				if (HeaderFields["keywords"] != null)
				{
					return HeaderFields.GetValues("keywords")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Keywords", value);
			}
		}

		public string MessageId
		{
			get
			{
				if (HeaderFields["message-id"] != null)
				{
					return Parser.Clean(Parser.RemoveWhiteSpaces(HeaderFields.GetValues("message-id")[0])).Trim('<', '>', ' ');
				}
				return null;
			}
			set
			{
				AddHeaderField("Message-ID", value);
			}
		}

		public string Charset
		{
			get
			{
				if (ContentType.Parameters["charset"] != null)
				{
					return ContentType.Parameters["charset"];
				}
				return null;
			}
			set
			{
				if (ContentType.Parameters["charset"] != null)
				{
					ContentType.Parameters["charset"] = value;
				}
				else
				{
					ContentType.Parameters.Add("charset", value);
				}
			}
		}

		public DateTime ReceivedDate
		{
			get
			{
				if (Trace.Count > 0)
				{
					DateTime date = Trace[0].Date;
					return Trace[0].Date;
				}
				return Date;
			}
		}

		public DateTime Date
		{
			get
			{
				try
				{
					return Parser.ParseAsUniversalDateTime(HeaderFields["date"]);
				}
				catch
				{
					return DateTime.MinValue;
				}
			}
			set
			{
				AddHeaderField("Date", value.ToString("r"));
			}
		}

		public Address ReturnReceipt
		{
			get
			{
				if (HeaderFields["return-receipt-to"] != null)
				{
					return Parser.ParseAddresses(HeaderFields.GetValues("return-receipt-to")[0])[0];
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					AddHeaderField("return-receipt-to", value.Merged);
				}
			}
		}

		public Address ConfirmRead
		{
			get
			{
				if (HeaderFields["disposition-notification-to"] != null)
				{
					return Parser.ParseAddresses(HeaderFields.GetValues("disposition-notification-to")[0])[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Disposition-Notification-To", value.Merged);
			}
		}

		public string Flag
		{
			get
			{
				if (HeaderFields["x-message-flag"] != null)
				{
					return HeaderFields.GetValues("x-message-flag")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("X-Message-Flag", value);
			}
		}

		public string DateString
		{
			get
			{
				if (HeaderFields["date"] != null)
				{
					return HeaderFields.GetValues("date")[0];
				}
				return null;
			}
		}

		public MessagePriority Priority
		{
			get
			{
				if (HeaderFields["x-priority"] != null)
				{
					if (HeaderFields.GetValues("x-priority")[0].IndexOf("1") != -1 || HeaderFields.GetValues("x-priority")[0].IndexOf("High") != -1)
					{
						return MessagePriority.High;
					}
					if (HeaderFields.GetValues("x-priority")[0].IndexOf("3") != -1 || HeaderFields.GetValues("x-priority")[0].IndexOf("Normal") != -1)
					{
						return MessagePriority.Normal;
					}
					if (HeaderFields.GetValues("x-priority")[0].IndexOf("4") != -1 || HeaderFields.GetValues("x-priority")[0].IndexOf("Low") != -1)
					{
						return MessagePriority.Low;
					}
					if (HeaderFields.GetValues("x-msmail-priority")[0].IndexOf("1") != -1 || HeaderFields.GetValues("x-msmail-priority")[0].IndexOf("High") != -1)
					{
						return MessagePriority.High;
					}
					if (HeaderFields.GetValues("x-msmail-priority")[0].IndexOf("3") != -1 || HeaderFields.GetValues("x-msmail-priority")[0].IndexOf("Normal") != -1)
					{
						return MessagePriority.Normal;
					}
					if (HeaderFields.GetValues("x-msmail-priority")[0].IndexOf("4") != -1 || HeaderFields.GetValues("x-msmail-priority")[0].IndexOf("Low") != -1)
					{
						return MessagePriority.Low;
					}
					if (HeaderFields.GetValues("importance")[0].IndexOf("1") != -1 || HeaderFields.GetValues("importance")[0].IndexOf("High") != -1)
					{
						return MessagePriority.High;
					}
					if (HeaderFields.GetValues("importance")[0].IndexOf("3") != -1 || HeaderFields.GetValues("importance")[0].IndexOf("Normal") != -1)
					{
						return MessagePriority.Normal;
					}
					if (HeaderFields.GetValues("importance")[0].IndexOf("4") != -1 || HeaderFields.GetValues("importance")[0].IndexOf("Low") != -1)
					{
						return MessagePriority.Low;
					}
					return MessagePriority.Unknown;
				}
				return MessagePriority.Unknown;
			}
			set
			{
				if (HeaderFields["x-priority"] != null)
				{
					HeaderFields["x-priority"] = value.ToString();
				}
				else
				{
					AddHeaderField("x-priority", value.ToString());
				}
				if (HeaderFields["x-priority"] != null)
				{
					HeaderFields["x-priority"] = value.ToString();
				}
				else
				{
					AddHeaderField("x-priority", value.ToString());
				}
				if (HeaderFields["x-priority"] != null)
				{
					HeaderFields["x-priority"] = value.ToString();
				}
				else
				{
					AddHeaderField("x-priority", value.ToString());
				}
				if (HeaderFields["x-msmail-priority"] != null)
				{
					HeaderFields["x-msmail-priority"] = value.ToString();
				}
				else
				{
					AddHeaderField("x-msmail-priority", value.ToString());
				}
				if (HeaderFields["x-msmail-priority"] != null)
				{
					HeaderFields["x-msmail-priority"] = value.ToString();
				}
				else
				{
					AddHeaderField("x-msmail-priority", value.ToString());
				}
				if (HeaderFields["x-msmail-priority"] != null)
				{
					HeaderFields["x-msmail-priority"] = value.ToString();
				}
				else
				{
					AddHeaderField("x-msmail-priority", value.ToString());
				}
				if (HeaderFields["importance"] != null)
				{
					HeaderFields["importance"] = value.ToString();
				}
				else
				{
					AddHeaderField("importance", value.ToString());
				}
				if (HeaderFields["importance"] != null)
				{
					HeaderFields["importance"] = value.ToString();
				}
				else
				{
					AddHeaderField("importance", value.ToString());
				}
				if (HeaderFields["importance"] != null)
				{
					HeaderFields["importance"] = value.ToString();
				}
				else
				{
					AddHeaderField("importance", value.ToString());
				}
			}
		}

		public ContentTransferEncoding ContentTransferEncoding
		{
			get
			{
				if (HeaderFields["content-transfer-encoding"] != null)
				{
					if (HeaderFields.GetValues("content-transfer-encoding")[0].ToLower().IndexOf("quoted-printable") != -1)
					{
						return ContentTransferEncoding.QuotedPrintable;
					}
					if (HeaderFields.GetValues("content-transfer-encoding")[0].ToLower().IndexOf("base64") != -1)
					{
						return ContentTransferEncoding.Base64;
					}
					if (HeaderFields.GetValues("content-transfer-encoding")[0].ToLower().IndexOf("8bit") != -1)
					{
						return ContentTransferEncoding.EightBits;
					}
					if (HeaderFields.GetValues("content-transfer-encoding")[0].ToLower().IndexOf("7bit") != -1)
					{
						return ContentTransferEncoding.SevenBits;
					}
					if (HeaderFields.GetValues("content-transfer-encoding")[0].ToLower().IndexOf("binary") != -1)
					{
						return ContentTransferEncoding.Binary;
					}
					return ContentTransferEncoding.Unknown;
				}
				return ContentTransferEncoding.None;
			}
			set
			{
				switch (value)
				{
				case ContentTransferEncoding.EightBits:
					AddHeaderField("Content-Transfer-Encoding", "8bit");
					break;
				case ContentTransferEncoding.SevenBits:
					AddHeaderField("Content-Transfer-Encoding", "7bit");
					break;
				case ContentTransferEncoding.QuotedPrintable:
					AddHeaderField("Content-Transfer-Encoding", "quoted-printable");
					break;
				case ContentTransferEncoding.Binary:
					AddHeaderField("Content-Transfer-Encoding", "binary");
					break;
				case ContentTransferEncoding.Base64:
					AddHeaderField("Content-Transfer-Encoding", "base64");
					break;
				}
			}
		}

		public ContentType ContentType
		{
			get
			{
				return _contentType;
			}
			set
			{
				_contentType = value;
			}
		}

		public ContentDisposition ContentDisposition
		{
			get
			{
				return _contentDisposition;
			}
			set
			{
				_contentDisposition = value;
			}
		}

		public int IndexOnServer
		{
			get
			{
				return _indexOnServer;
			}
			set
			{
				_indexOnServer = value;
			}
		}

		public int Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public string NewsGroups
		{
			get
			{
				if (HeaderFields["newsgroups"] != null)
				{
					return HeaderFields.GetValues("newsgroups")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Newsgroups", value);
			}
		}

		public string Path
		{
			get
			{
				if (HeaderFields["path"] != null)
				{
					return HeaderFields.GetValues("path")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Path", value);
			}
		}

		public string FollowUpTo
		{
			get
			{
				if (HeaderFields["followup-to"] != null)
				{
					return HeaderFields.GetValues("followup-to")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Followup-To", value);
			}
		}

		public DateTime Expires
		{
			get
			{
				try
				{
					return Parser.ParseAsUniversalDateTime(HeaderFields["expires"]);
				}
				catch
				{
					return DateTime.MinValue;
				}
			}
			set
			{
				if (HeaderFields["expires"] != null)
				{
					HeaderFields["expires"] = value.ToString("r");
				}
				else
				{
					AddHeaderField("expires", value.ToString("r"));
				}
			}
		}

		public string ExpiresString
		{
			get
			{
				if (HeaderFields["expires"] != null)
				{
					return HeaderFields.GetValues("expires")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Expires", value);
			}
		}

		public string Control
		{
			get
			{
				if (HeaderFields["control"] != null)
				{
					return HeaderFields.GetValues("control")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Control", value);
			}
		}

		public string Distribution
		{
			get
			{
				if (HeaderFields["distribution"] != null)
				{
					return HeaderFields.GetValues("distribution")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Distribution", value);
			}
		}

		public string Organization
		{
			get
			{
				if (HeaderFields["organization"] != null)
				{
					return HeaderFields.GetValues("organization")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Organization", value);
			}
		}

		public string Summary
		{
			get
			{
				if (HeaderFields["summary"] != null)
				{
					return HeaderFields.GetValues("summary")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Summary", value);
			}
		}

		public string Approved
		{
			get
			{
				if (HeaderFields["approved"] != null)
				{
					return HeaderFields.GetValues("approved")[0];
				}
				return null;
			}
			set
			{
				AddHeaderField("Approved", value);
			}
		}

		public int BodyLineCount
		{
			get
			{
				if (HeaderFields["lines"] != null)
				{
					return Convert.ToInt32(HeaderFields.GetValues("lines")[0]);
				}
				return -1;
			}
			set
			{
				AddHeaderField("Lines", value.ToString());
			}
		}

		public UsenetXrefList Xref
		{
			get
			{
				UsenetXrefList usenetXrefList = new UsenetXrefList();
				usenetXrefList.Host = HeaderFields["xref"].Split(' ')[0];
				UsenetXrefList usenetXrefList2 = usenetXrefList;
				string[] array = HeaderFields["xref"].Split(' ');
				for (int i = 1; i < array.Length; i++)
				{
					if (array[i].IndexOf(":") != -1)
					{
						usenetXrefList2.Groups.Add(array[i].Split(':')[0], array[i].Split(':')[1]);
					}
				}
				return usenetXrefList2;
			}
			set
			{
				string text = value.Groups.AllKeys.Aggregate("", (string current, string str) => current + " " + str + ":" + value.Groups[str]);
				HeaderFields["xref"] = value.Host + text;
			}
		}

		public void AddHeaderField(string name, string value)
		{
			string text = name.ToLower();
			if (HeaderFields[text] == null || text.Equals("received"))
			{
				HeaderFields.Add(text, value);
			}
			else
			{
				HeaderFields[text] = value;
			}
			if (HeaderFieldNames[text] == null || text.Equals("received"))
			{
				HeaderFieldNames.Add(text, name);
			}
			else
			{
				HeaderFieldNames[text] = name;
			}
		}

		public BounceResult GetBounceStatus()
		{
			return GetBounceStatus(null);
		}

		public BounceResult GetBounceStatus(string signaturesFilePath)
		{
			string xml = (string.IsNullOrEmpty(signaturesFilePath) ? GetResource("ActiveUp.Net.Common.BouncedSignatures.xml") : File.OpenText(signaturesFilePath).ReadToEnd());
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			BounceResult bounceResult = new BounceResult();
			foreach (XmlElement item in xmlDocument.GetElementsByTagName("signature"))
			{
				if (From.Merged.IndexOf(item.GetElementsByTagName("from")[0].InnerText) != -1)
				{
					bounceResult.Level++;
				}
				if (Subject != null && Subject.IndexOf(item.GetElementsByTagName("subject")[0].InnerText) != -1)
				{
					bounceResult.Level++;
				}
			}
			return bounceResult;
		}

		public static string GetResource(string resource)
		{
			Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
			StreamReader streamReader = new StreamReader(manifestResourceStream);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			manifestResourceStream.Close();
			return result;
		}

		public string ToHeaderString()
		{
			return ToHeaderString(false);
		}

		public string ToHeaderString(bool removeBlindCopies, bool forceBase64Encoding = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (TraceInfo item in Trace)
			{
				AddHeaderField("Received", item.ToString());
			}
			if (!From.Email.Equals(string.Empty))
			{
				AddHeaderField("From", From.Merged);
			}
			if (!Sender.Email.Equals(string.Empty))
			{
				AddHeaderField("Sender", Sender.Merged);
			}
			if (To.Count > 0)
			{
				AddHeaderField("To", To.Merged);
			}
			if (Cc.Count > 0)
			{
				AddHeaderField("Cc", Cc.Merged);
			}
			if (Bcc.Count > 0 && !removeBlindCopies)
			{
				AddHeaderField("Bcc", Bcc.Merged);
			}
			if (!ReplyTo.Email.Equals(string.Empty))
			{
				AddHeaderField("Reply-To", ReplyTo.Merged);
			}
			if (Date.Equals(DateTime.MinValue))
			{
				Date = DateTime.Now;
			}
			if (string.IsNullOrEmpty(MessageId))
			{
				MessageId = "<AU" + Codec.GetUniqueString() + "@" + System.Net.Dns.GetHostName() + ">";
			}
			AddHeaderField("MIME-Version", "1.0");
			if (ContentType.MimeType.Length > 0)
			{
				string text = ContentType.ToString();
				text = text.Substring(text.IndexOf(":") + 1).TrimStart(' ');
				AddHeaderField("Content-Type", text);
			}
			if (ContentDisposition.Disposition.Length > 0)
			{
				string text2 = ContentDisposition.ToString();
				text2 = text2.Substring(text2.IndexOf(":") + 1).TrimStart(' ');
				AddHeaderField("Content-Disposition", text2);
			}
			if (forceBase64Encoding)
			{
				AddHeaderField("Content-Transfer-Encoding", "base64");
			}
			else if (ContentType.Type.Equals("text"))
			{
				AddHeaderField("Content-Transfer-Encoding", "quoted-printable");
			}
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			string[] allKeys = HeaderFields.AllKeys;
			foreach (string name in allKeys)
			{
				for (int j = 0; j < HeaderFields.GetValues(name).Length; j++)
				{
					stringBuilder.Append(HeaderFieldNames.GetValues(name)[j] + ": " + HeaderFields.GetValues(name)[j] + "\r\n");
				}
			}
			return stringBuilder.ToString().TrimEnd('\r', '\n');
		}

		public virtual string StoreToFile(string path)
		{
			FileStream fileStream = File.Create(path);
			fileStream.Write(_data, 0, _data.Length);
			fileStream.Close();
			return path;
		}
	}
}
