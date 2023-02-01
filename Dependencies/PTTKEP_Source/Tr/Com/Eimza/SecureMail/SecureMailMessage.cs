using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Tr.Com.Eimza.EYazisma;

namespace Tr.Com.Eimza.SecureMail
{
	internal class SecureMailMessage : IDisposable
	{
		public SecureAttachmentCollection Attachments { get; private set; }

		public SecureMailAddressCollection Bcc { get; private set; }

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

		public SecureMailAddressCollection CC { get; private set; }

		public SecureDeliveryNotificationOptions DeliveryNotificationOptions
		{
			get
			{
				return (SecureDeliveryNotificationOptions)InternalMailMessage.DeliveryNotificationOptions;
			}
			set
			{
				InternalMailMessage.DeliveryNotificationOptions = (DeliveryNotificationOptions)value;
			}
		}

		public SecureMailAddress From { get; set; }

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

		public SecureMailPriority Priority
		{
			get
			{
				switch (InternalMailMessage.Priority)
				{
				case MailPriority.Normal:
					return SecureMailPriority.Normal;
				case MailPriority.Low:
					return SecureMailPriority.Low;
				case MailPriority.High:
					return SecureMailPriority.High;
				default:
					return (SecureMailPriority)InternalMailMessage.Priority;
				}
			}
			set
			{
				switch (value)
				{
				case SecureMailPriority.Normal:
					InternalMailMessage.Priority = MailPriority.Normal;
					break;
				case SecureMailPriority.Low:
					InternalMailMessage.Priority = MailPriority.Low;
					break;
				case SecureMailPriority.High:
					InternalMailMessage.Priority = MailPriority.High;
					break;
				default:
					InternalMailMessage.Priority = (MailPriority)value;
					break;
				}
			}
		}

		public SecureMailAddress ReplyTo { get; set; }

		public SecureMailAddress Sender { get; set; }

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

		public SecureMailAddressCollection To { get; private set; }

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

		public SecureMailMessage()
		{
			InternalMailMessage = new MailMessage();
			Attachments = new SecureAttachmentCollection();
			To = new SecureMailAddressCollection();
			CC = new SecureMailAddressCollection();
			Bcc = new SecureMailAddressCollection();
		}

		public SecureMailMessage(string from, string to)
			: this()
		{
			From = new SecureMailAddress(from);
			To.Add(to);
		}

		public SecureMailMessage(SecureMailAddress from, SecureMailAddress to)
			: this()
		{
			From = from;
			To.Add(to);
		}

		public SecureMailMessage(string from, string to, string subject, string body)
			: this()
		{
			From = new SecureMailAddress(from);
			To.Add(to);
			InternalMailMessage.Subject = subject;
			InternalMailMessage.Body = body;
		}

		private SecureMessageContent GetUnsignedContent()
		{
			SecureContentType secureContentType = new SecureContentType();
			secureContentType.MediaType = (IsBodyHtml ? "text/html" : "text/plain");
			secureContentType.CharSet = BodyEncoding.BodyName;
			Encoding encoding = BodyEncoding ?? Encoding.ASCII;
			SecureMessageContent secureMessageContent = new SecureMessageContent(transferEncoding: (encoding != Encoding.ASCII && encoding != Encoding.UTF8) ? SecureTransferEncoding.Base64 : SecureTransferEncoding.QuotedPrintable, body: encoding.GetBytes(Body), contentType: secureContentType, encodeBody: IsMultipart);
			if (Attachments.Count == 0)
			{
				return secureMessageContent;
			}
			SecureContentType secureContentType2 = new SecureContentType("multipart/mixed");
			secureContentType2.GenerateBoundary();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\r\n");
			stringBuilder.Append("--");
			stringBuilder.Append(secureContentType2.Boundary);
			stringBuilder.Append("\r\n");
			stringBuilder.Append("Content-Type: ");
			stringBuilder.Append(secureMessageContent.ContentType.ToString());
			stringBuilder.Append("\r\n");
			stringBuilder.Append("Content-Transfer-Encoding: ");
			stringBuilder.Append(TransferEncoder.GetTransferEncodingName(secureMessageContent.TransferEncoding));
			stringBuilder.Append("\r\n\r\n");
			stringBuilder.Append(Encoding.ASCII.GetString(secureMessageContent.Body));
			stringBuilder.Append("\r\n");
			foreach (SecureAttachment attachment in Attachments)
			{
				stringBuilder.Append("--");
				stringBuilder.Append(secureContentType2.Boundary);
				stringBuilder.Append("\r\n");
				stringBuilder.Append("Content-Type: ");
				stringBuilder.Append(attachment.ContentType.ToString());
				stringBuilder.Append("\r\n");
				stringBuilder.Append("Content-Transfer-Encoding: base64\r\n");
				stringBuilder.Append("Content-Disposition: attachment; filename=" + attachment.ContentType.Name);
				stringBuilder.Append("\r\n\r\n");
				stringBuilder.Append(TransferEncoder.ToBase64(attachment.RawBytes));
				stringBuilder.Append("\r\n");
			}
			stringBuilder.Append("--");
			stringBuilder.Append(secureContentType2.Boundary);
			stringBuilder.Append("--\r\n");
			return new SecureMessageContent(Encoding.ASCII.GetBytes(stringBuilder.ToString()), secureContentType2, SecureTransferEncoding.SevenBit, false);
		}

		public string GetUnSignedString(SecureMessageContent unsignedContent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Content-Type: ");
			stringBuilder.Append(unsignedContent.ContentType.ToString());
			stringBuilder.Append("\r\n");
			stringBuilder.Append("Content-Transfer-Encoding: ");
			stringBuilder.Append(TransferEncoder.GetTransferEncodingName(unsignedContent.TransferEncoding));
			stringBuilder.Append("\r\n\r\n");
			stringBuilder.Append(Encoding.ASCII.GetString(unsignedContent.Body));
			return stringBuilder.ToString();
		}

		private SecureMessageContent SignContent(SecureMessageContent unsignedContent)
		{
			string text = "sha256";
			if (From.OzetAlg.HasValue)
			{
				switch (From.OzetAlg.Value)
				{
				case OzetAlg.SHA256:
					text = "sha256";
					break;
				case OzetAlg.SHA512:
					text = "sha512";
					break;
				}
			}
			SecureContentType secureContentType = new SecureContentType("multipart/signed; protocol=\"application/x-pkcs7-signature\"; micalg=" + text + "; ");
			secureContentType.GenerateBoundary();
			string unSignedString = GetUnSignedString(unsignedContent);
			byte[] bytes = (From.OzetAlg.HasValue ? CryptoHelper.GetSignatureWithSmartCard(unSignedString, From.Reader, From.OzetAlg.Value) : CryptoHelper.GetSignatureWithSmartCard(unSignedString, From.Reader));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("--");
			stringBuilder.Append(secureContentType.Boundary);
			stringBuilder.Append("\r\n");
			stringBuilder.Append(unSignedString);
			stringBuilder.Append("\r\n");
			stringBuilder.Append("--");
			stringBuilder.Append(secureContentType.Boundary);
			stringBuilder.Append("\r\n");
			stringBuilder.Append("Content-Type: application/x-pkcs7-signature; name=\"smime.p7s\"\r\n");
			stringBuilder.Append("Content-Transfer-Encoding: base64\r\n");
			stringBuilder.Append("Content-Disposition: attachment; filename=\"smime.p7s\"\r\n");
			stringBuilder.Append("Content-Description: S/MIME Cryptographic Signature\r\n\r\n");
			stringBuilder.Append(TransferEncoder.ToBase64(bytes));
			stringBuilder.Append("\r\n\r\n");
			stringBuilder.Append("--");
			stringBuilder.Append(secureContentType.Boundary);
			stringBuilder.Append("--");
			return new SecureMessageContent(Encoding.ASCII.GetBytes(stringBuilder.ToString()), secureContentType, SecureTransferEncoding.SevenBit, false);
		}

		internal SecureMessageContent GetCompleteContent()
		{
			SecureMessageContent secureMessageContent = GetUnsignedContent();
			if (IsSigned)
			{
				secureMessageContent = SignContent(secureMessageContent);
			}
			return secureMessageContent;
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
			foreach (SecureMailAddress item in To)
			{
				mailMessage.To.Add(item.InternalMailAddress);
			}
			foreach (SecureMailAddress item2 in CC)
			{
				mailMessage.CC.Add(item2.InternalMailAddress);
			}
			foreach (SecureMailAddress item3 in Bcc)
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
			SecureMessageContent completeContent = GetCompleteContent();
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = ((completeContent.TransferEncoding != SecureTransferEncoding.SevenBit) ? completeContent.Body : Encoding.ASCII.GetBytes(Regex.Replace(Encoding.ASCII.GetString(completeContent.Body), "^\\.", "..", RegexOptions.Multiline)));
			memoryStream.Write(array, 0, array.Length);
			memoryStream.Position = 0L;
			AlternateView alternateView = new AlternateView(memoryStream, completeContent.ContentType.InternalContentType);
			alternateView.TransferEncoding = TransferEncoder.ConvertTransferEncoding(completeContent.TransferEncoding);
			mailMessage.AlternateViews.Add(alternateView);
			return mailMessage;
		}

		public static implicit operator MailMessage(SecureMailMessage message)
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
