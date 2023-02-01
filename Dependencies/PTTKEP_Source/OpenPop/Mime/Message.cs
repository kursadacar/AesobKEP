using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using OpenPop.Mime.Header;
using OpenPop.Mime.Traverse;

namespace OpenPop.Mime
{
	public class Message
	{
		public MessageHeader Headers { get; private set; }

		public MessagePart MessagePart { get; private set; }

		public byte[] RawMessage { get; private set; }

		public Message(byte[] rawMessageContent)
			: this(rawMessageContent, true)
		{
		}

		public Message(byte[] rawMessageContent, bool parseBody)
		{
			RawMessage = rawMessageContent;
			MessageHeader headers;
			byte[] body;
			HeaderExtractor.ExtractHeadersAndBody(rawMessageContent, out headers, out body);
			Headers = headers;
			if (parseBody)
			{
				MessagePart = new MessagePart(body, Headers);
			}
		}

		public MailMessage ToMailMessage()
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.Subject = Headers.Subject;
			mailMessage.SubjectEncoding = Encoding.UTF8;
			MessagePart messagePart = FindFirstHtmlVersion();
			if (messagePart != null)
			{
				mailMessage.IsBodyHtml = true;
			}
			else
			{
				messagePart = FindFirstPlainTextVersion();
			}
			if (messagePart != null)
			{
				mailMessage.Body = messagePart.GetBodyAsText();
				mailMessage.BodyEncoding = messagePart.BodyEncoding;
			}
			foreach (MessagePart item in (IEnumerable<MessagePart>)FindAllTextVersions())
			{
				if (item != messagePart)
				{
					AlternateView alternateView = new AlternateView(new MemoryStream(item.Body));
					alternateView.ContentId = item.ContentId;
					alternateView.ContentType = item.ContentType;
					mailMessage.AlternateViews.Add(alternateView);
				}
			}
			foreach (MessagePart item2 in (IEnumerable<MessagePart>)FindAllAttachments())
			{
				Attachment attachment = new Attachment(new MemoryStream(item2.Body), item2.ContentType);
				attachment.ContentId = item2.ContentId;
				mailMessage.Attachments.Add(attachment);
			}
			if (Headers.From != null && Headers.From.HasValidMailAddress)
			{
				mailMessage.From = Headers.From.MailAddress;
			}
			if (Headers.ReplyTo != null && Headers.ReplyTo.HasValidMailAddress)
			{
				mailMessage.ReplyTo = Headers.ReplyTo.MailAddress;
			}
			if (Headers.Sender != null && Headers.Sender.HasValidMailAddress)
			{
				mailMessage.Sender = Headers.Sender.MailAddress;
			}
			foreach (RfcMailAddress item3 in Headers.To)
			{
				if (item3.HasValidMailAddress)
				{
					mailMessage.To.Add(item3.MailAddress);
				}
			}
			foreach (RfcMailAddress item4 in Headers.Cc)
			{
				if (item4.HasValidMailAddress)
				{
					mailMessage.CC.Add(item4.MailAddress);
				}
			}
			foreach (RfcMailAddress item5 in Headers.Bcc)
			{
				if (item5.HasValidMailAddress)
				{
					mailMessage.Bcc.Add(item5.MailAddress);
				}
			}
			return mailMessage;
		}

		public MessagePart FindFirstPlainTextVersion()
		{
			return FindFirstMessagePartWithMediaType("text/plain");
		}

		public MessagePart FindFirstHtmlVersion()
		{
			return FindFirstMessagePartWithMediaType("text/html");
		}

		public List<MessagePart> FindAllTextVersions()
		{
			return new TextVersionFinder().VisitMessage(this);
		}

		public List<MessagePart> FindAllAttachments()
		{
			return new AttachmentFinder().VisitMessage(this);
		}

		public MessagePart FindFirstMessagePartWithMediaType(string mediaType)
		{
			return new FindFirstMessagePartWithMediaType().VisitMessage(this, mediaType);
		}

		public List<MessagePart> FindAllMessagePartsWithMediaType(string mediaType)
		{
			return new FindAllMessagePartsWithMediaType().VisitMessage(this, mediaType);
		}

		public void Save(FileInfo file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			using (FileStream messageStream = new FileStream(file.FullName, FileMode.Create))
			{
				Save(messageStream);
			}
		}

		public void Save(Stream messageStream)
		{
			if (messageStream == null)
			{
				throw new ArgumentNullException("messageStream");
			}
			messageStream.Write(RawMessage, 0, RawMessage.Length);
		}

		public static Message Load(FileInfo file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			if (!file.Exists)
			{
				throw new FileNotFoundException("Cannot load message from non-existent file", file.FullName);
			}
			using (FileStream messageStream = new FileStream(file.FullName, FileMode.Open))
			{
				return Load(messageStream);
			}
		}

		public static Message Load(Stream messageStream)
		{
			if (messageStream == null)
			{
				throw new ArgumentNullException("messageStream");
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte[] buffer = new byte[4096];
				int count;
				while ((count = messageStream.Read(buffer, 0, 4096)) > 0)
				{
					memoryStream.Write(buffer, 0, count);
				}
				return new Message(memoryStream.ToArray());
			}
		}
	}
}
