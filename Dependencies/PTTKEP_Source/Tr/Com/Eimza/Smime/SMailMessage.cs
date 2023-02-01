using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.RegularExpressions;
using Tr.Com.Eimza.EYazisma;
using Tr.Com.Eimza.EYazisma.Utilities;

namespace Tr.Com.Eimza.Smime
{
	internal class SMailMessage : IDisposable
	{
		public byte[] UnSignedContent { get; set; }

		public SMailMessageContent SignedMessageContent { get; set; }

		public SMailMessageContent CompleteContent { get; set; }

		public byte[] SignedContent { get; set; }

		public string BoundaryUnSignedContent { get; set; }

		public string BoundarySignedContent { get; set; }

		public SMailAttachmentCollection Attachments { get; private set; }

		public SMailAddressCollection Bcc { get; private set; }

		public string Body
		{
			get
			{
				return InternalMailMessage.Body;
			}
			set
			{
				InternalMailMessage.Body = value;
			}
		}

		public Encoding BodyEncoding
		{
			get
			{
				return InternalMailMessage.BodyEncoding;
			}
			set
			{
				InternalMailMessage.BodyEncoding = value;
			}
		}

		public SMailAddressCollection CC { get; private set; }

		public SMailDeliveryNotificationOptions DeliveryNotificationOptions
		{
			get
			{
				return (SMailDeliveryNotificationOptions)InternalMailMessage.DeliveryNotificationOptions;
			}
			set
			{
				InternalMailMessage.DeliveryNotificationOptions = (DeliveryNotificationOptions)value;
			}
		}

		public SMailAddress From { get; set; }

		public NameValueCollection Headers
		{
			get
			{
				return InternalMailMessage.Headers;
			}
		}

		public bool IsBodyHtml
		{
			get
			{
				return InternalMailMessage.IsBodyHtml;
			}
			set
			{
				InternalMailMessage.IsBodyHtml = value;
			}
		}

		public SMailPriority Priority
		{
			get
			{
				switch (InternalMailMessage.Priority)
				{
				case MailPriority.Normal:
					return SMailPriority.Normal;
				case MailPriority.Low:
					return SMailPriority.Low;
				case MailPriority.High:
					return SMailPriority.High;
				default:
					return (SMailPriority)InternalMailMessage.Priority;
				}
			}
			set
			{
				switch (value)
				{
				case SMailPriority.Normal:
					InternalMailMessage.Priority = MailPriority.Normal;
					break;
				case SMailPriority.Low:
					InternalMailMessage.Priority = MailPriority.Low;
					break;
				case SMailPriority.High:
					InternalMailMessage.Priority = MailPriority.High;
					break;
				default:
					InternalMailMessage.Priority = (MailPriority)value;
					break;
				}
			}
		}

		public SMailAddress ReplyTo { get; set; }

		public SMailAddress Sender { get; set; }

		public string Subject
		{
			get
			{
				return InternalMailMessage.Subject;
			}
			set
			{
				InternalMailMessage.Subject = value;
			}
		}

		public Encoding SubjectEncoding
		{
			get
			{
				return InternalMailMessage.SubjectEncoding;
			}
			set
			{
				InternalMailMessage.SubjectEncoding = value;
			}
		}

		public SMailAddressCollection To { get; private set; }

		internal bool IsMultipart
		{
			get
			{
				if (Attachments.Count <= 0)
				{
					return IsSigned;
				}
				return true;
			}
		}

		public bool IsSigned { get; set; }

		internal MailMessage InternalMailMessage { get; private set; }

		public SMailMessage()
		{
			InternalMailMessage = new MailMessage();
			Attachments = new SMailAttachmentCollection();
			To = new SMailAddressCollection();
			CC = new SMailAddressCollection();
			Bcc = new SMailAddressCollection();
		}

		public SMailMessage(MailMessage message)
		{
			InternalMailMessage = message;
			Attachments = new SMailAttachmentCollection();
			To = new SMailAddressCollection();
			CC = new SMailAddressCollection();
			Bcc = new SMailAddressCollection();
		}

		public SMailMessage(string from, string to)
			: this()
		{
			From = new SMailAddress(from);
			To.Add(to);
		}

		public SMailMessage(SMailAddress from, SMailAddress to)
			: this()
		{
			From = from;
			To.Add(to);
		}

		public SMailMessage(string from, string to, string subject, string body)
			: this()
		{
			From = new SMailAddress(from);
			To.Add(to);
			InternalMailMessage.Subject = subject;
			InternalMailMessage.Body = body;
		}

		public byte[] GetContent()
		{
			CompleteContent = GetUnsignedContent();
			UnSignedContent = CompleteContent.Body;
			StringBuilder unSignedMessage = GetUnSignedMessage(CompleteContent);
			return Encoding.ASCII.GetBytes(unSignedMessage.ToString());
		}

		private SMailMessageContent GetUnsignedContent()
		{
			SMailContentType sMailContentType = new SMailContentType();
			sMailContentType.MediaType = (IsBodyHtml ? "text/html" : "text/plain");
			sMailContentType.CharSet = BodyEncoding.BodyName;
			Encoding encoding = BodyEncoding ?? Encoding.ASCII;
			SMailMessageContent sMailMessageContent = new SMailMessageContent(transferEncoding: (encoding != Encoding.ASCII && encoding != Encoding.UTF8) ? SMailTransferEncoding.Base64 : SMailTransferEncoding.QuotedPrintable, body: encoding.GetBytes(Body), contentType: sMailContentType, encodeBody: IsMultipart);
			if (Attachments.Count == 0)
			{
				return sMailMessageContent;
			}
			SMailContentType sMailContentType2 = new SMailContentType("multipart/mixed");
			if (BoundaryUnSignedContent == null)
			{
				sMailContentType2.GenerateBoundary();
				BoundaryUnSignedContent = sMailContentType2.Boundary;
			}
			else
			{
				sMailContentType2.Boundary = BoundaryUnSignedContent;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\r\n");
			stringBuilder.Append("--");
			stringBuilder.Append(sMailContentType2.Boundary);
			stringBuilder.Append("\r\n");
			stringBuilder.Append("Content-Type: ");
			stringBuilder.Append(sMailMessageContent.ContentType.ToString());
			stringBuilder.Append("\r\n");
			stringBuilder.Append("Content-Transfer-Encoding: ");
			stringBuilder.Append(SMailTransferEncoder.GetTransferEncodingName(sMailMessageContent.TransferEncoding));
			stringBuilder.Append("\r\n\r\n");
			stringBuilder.Append(Encoding.ASCII.GetString(sMailMessageContent.Body));
			stringBuilder.Append("\r\n");
			foreach (SMailAttachment attachment in Attachments)
			{
				stringBuilder.Append("--");
				stringBuilder.Append(sMailContentType2.Boundary);
				stringBuilder.Append("\r\n");
				stringBuilder.Append("Content-Type: ");
				stringBuilder.Append(attachment.ContentType.ToString());
				stringBuilder.Append("\r\n");
				stringBuilder.Append("Content-Transfer-Encoding: base64\r\n");
				stringBuilder.Append("Content-Disposition: attachment; filename=" + attachment.ContentType.Name);
				stringBuilder.Append("\r\n\r\n");
				stringBuilder.Append(SMailTransferEncoder.ToBase64(attachment.RawBytes));
				stringBuilder.Append("\r\n");
			}
			stringBuilder.Append("--");
			stringBuilder.Append(sMailContentType2.Boundary);
			stringBuilder.Append("--\r\n");
			string s = stringBuilder.ToString();
			return new SMailMessageContent(Encoding.ASCII.GetBytes(s), sMailContentType2, SMailTransferEncoding.SevenBit, false);
		}

		private StringBuilder GetUnSignedMessage(SMailMessageContent unsignedContent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Content-Type: ");
			stringBuilder.Append(unsignedContent.ContentType.ToString());
			stringBuilder.Append("\r\n");
			stringBuilder.Append("Content-Transfer-Encoding: ");
			stringBuilder.Append(SMailTransferEncoder.GetTransferEncodingName(unsignedContent.TransferEncoding));
			stringBuilder.Append("\r\n\r\n");
			stringBuilder.Append(Encoding.ASCII.GetString(unsignedContent.Body));
			return stringBuilder;
		}

		private SMailMessageContent SignContent(string unsignedContent)
		{
			string text = "sha256";
			try
			{
				SignedCms signedCms = new SignedCms();
				signedCms.Decode(SignedContent);
				switch (signedCms.Certificates[0].SignatureAlgorithm.Value)
				{
				case "1.2.840.113549.1.1.5":
					text = "sha1";
					break;
				case "1.2.840.113549.1.1.11":
					text = "sha256";
					break;
				case "1.2.840.113549.1.1.12":
					text = "sha384";
					break;
				case "1.2.840.113549.1.1.13":
					text = "sha512";
					break;
				}
			}
			catch (Exception)
			{
				text = "sha256";
				EyLog.Log("S/Mime Olustur", EyLogTuru.UYARI, "Imza Verisinden Sertifika Özet Algoritması Çekilemedi.");
			}
			SMailContentType sMailContentType = new SMailContentType("multipart/signed; protocol=\"application/x-pkcs7-signature\"; micalg=" + text + "; ");
			sMailContentType.GenerateBoundary();
			byte[] signedContent = SignedContent;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("--");
			stringBuilder.Append(sMailContentType.Boundary);
			stringBuilder.Append("\r\n");
			stringBuilder.Append(unsignedContent);
			stringBuilder.Append("\r\n");
			stringBuilder.Append("--");
			stringBuilder.Append(sMailContentType.Boundary);
			stringBuilder.Append("\r\n");
			stringBuilder.Append("Content-Type: application/pkcs7-signature; name=smime.p7s; smime-type=signed-data\r\n");
			stringBuilder.Append("Content-Transfer-Encoding: base64\r\n");
			stringBuilder.Append("Content-Disposition: attachment; filename=\"smime.p7s\"\r\n");
			stringBuilder.Append("Content-Description: S/MIME Cryptographic Signature\r\n\r\n");
			stringBuilder.Append(SMailTransferEncoder.ToBase64(signedContent));
			stringBuilder.Append("\r\n");
			stringBuilder.Append("--");
			stringBuilder.Append(sMailContentType.Boundary);
			stringBuilder.Append("--");
			return new SMailMessageContent(Encoding.ASCII.GetBytes(stringBuilder.ToString()), sMailContentType, SMailTransferEncoding.SevenBit, false);
		}

		private SMailMessageContent GetCompleteContent()
		{
			if (UnSignedContent == null)
			{
				SignedMessageContent = GetUnsignedContent();
			}
			if (IsSigned && UnSignedContent != null)
			{
				SignedMessageContent = SignContent(Encoding.ASCII.GetString(UnSignedContent));
			}
			return SignedMessageContent;
		}

		public MailMessage ToMailMessage()
		{
			MailMessage mailMessage = new MailMessage();
			if (From != null)
			{
				mailMessage.From = From.InternalMailAddress;
			}
			if (Sender != null)
			{
				mailMessage.Sender = Sender.InternalMailAddress;
			}
			if (ReplyTo != null)
			{
				mailMessage.ReplyTo = ReplyTo.InternalMailAddress;
			}
			foreach (SMailAddress item in To)
			{
				mailMessage.To.Add(item.InternalMailAddress);
			}
			foreach (SMailAddress item2 in CC)
			{
				mailMessage.CC.Add(item2.InternalMailAddress);
			}
			foreach (SMailAddress item3 in Bcc)
			{
				mailMessage.Bcc.Add(item3.InternalMailAddress);
			}
			mailMessage.DeliveryNotificationOptions = InternalMailMessage.DeliveryNotificationOptions;
			foreach (string header in InternalMailMessage.Headers)
			{
				mailMessage.Headers.Add(header, InternalMailMessage.Headers[header]);
			}
			mailMessage.Priority = InternalMailMessage.Priority;
			mailMessage.Subject = InternalMailMessage.Subject;
			mailMessage.SubjectEncoding = InternalMailMessage.SubjectEncoding;
			if (InternalMailMessage.BodyEncoding == null)
			{
				InternalMailMessage.BodyEncoding = Encoding.ASCII;
			}
			mailMessage.BodyEncoding = InternalMailMessage.BodyEncoding;
			SMailMessageContent sMailMessageContent = (IsSigned ? GetCompleteContent() : CompleteContent);
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = ((sMailMessageContent.TransferEncoding != SMailTransferEncoding.SevenBit) ? sMailMessageContent.Body : Encoding.ASCII.GetBytes(Regex.Replace(Encoding.ASCII.GetString(sMailMessageContent.Body), "^\\.", "..", RegexOptions.Multiline)));
			memoryStream.Write(array, 0, array.Length);
			memoryStream.Position = 0L;
			AlternateView alternateView = new AlternateView(memoryStream, sMailMessageContent.ContentType.InternalContentType);
			alternateView.TransferEncoding = SMailTransferEncoder.ConvertTransferEncoding(sMailMessageContent.TransferEncoding);
			mailMessage.AlternateViews.Add(alternateView);
			return mailMessage;
		}

		public static implicit operator MailMessage(SMailMessage message)
		{
			if (message == null)
			{
				return null;
			}
			return message.ToMailMessage();
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && InternalMailMessage != null)
			{
				InternalMailMessage.Dispose();
			}
			GC.Collect();
		}
	}
}
