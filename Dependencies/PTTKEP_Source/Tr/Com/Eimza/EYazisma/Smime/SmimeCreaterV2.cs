using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ActiveUp.Net.Mail;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;
using Tr.Com.Eimza.SmartCard;

namespace Tr.Com.Eimza.EYazisma.Smime
{
	internal class SmimeCreaterV2
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public byte[] CreateWithSmartCard(string from, List<string> to, List<string> cc, List<string> bcc, string subject, string content, List<string> attachments, string mailType, string mailTypeId, EYazismaIcerikTur icerikTur, SmartCardReader reader)
		{
			Message message = new Message();
			message.MessageId = MimeUtilities.GetUniqueMessageIDValue();
			message.From = new Address(from);
			AddressCollection addressCollection = new AddressCollection();
			if (to != null && to.Count != 0)
			{
				foreach (string item in to)
				{
					if (item == null)
					{
						throw new Exception("Mesajın Kime Alanı Null Değere Sahip Liste Elemanı Olamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
					}
					if (!MimeUtilities.EmailKontrol(item))
					{
						throw new Exception("Mesajın Kime Alanı KEP Mail Formatına Uygun Değil. Verdiğiniz Parametreleri Kontrol Ediniz.");
					}
					addressCollection.Add(new Address(item));
				}
				message.To = addressCollection;
				AddressCollection addressCollection2 = new AddressCollection();
				if (cc != null)
				{
					foreach (string item2 in cc)
					{
						if (item2 != null)
						{
							addressCollection2.Add(new Address(item2));
						}
					}
				}
				message.Cc = addressCollection2;
				AddressCollection addressCollection3 = new AddressCollection();
				if (bcc != null)
				{
					foreach (string item3 in bcc)
					{
						if (item3 != null)
						{
							addressCollection3.Add(new Address(item3));
						}
					}
				}
				message.Bcc = addressCollection3;
				message.Subject = Codec.RFC2047Encode(subject, "UTF-8");
				if (icerikTur == EYazismaIcerikTur.TEXT)
				{
					message.BodyText = new MimeBody(BodyFormat.Text)
					{
						Charset = "UTF-8",
						ContentTransferEncoding = ContentTransferEncoding.QuotedPrintable,
						Text = content
					};
				}
				else
				{
					message.BodyText = new MimeBody(BodyFormat.Html)
					{
						Charset = "UTF-8",
						ContentTransferEncoding = ContentTransferEncoding.QuotedPrintable,
						Text = content
					};
				}
				if (attachments != null)
				{
					foreach (string attachment in attachments)
					{
						if (attachment != null)
						{
							try
							{
								MimePart part = new MimePart(File.ReadAllBytes(attachment), MimeTypesHelper.GetMimeqType(attachment), Codec.RFC2047Encode(attachment, "UTF-8"), "UTF-8");
								message.Attachments.Add(part);
							}
							catch (Exception)
							{
								throw new Exception("Mail'e Eklenecek Olan Ekler Okunamadı. Eklerin Adresini Doğru Verdiğinizden Emin Olunuz.");
							}
						}
					}
				}
				if (mailType != null && (mailType.Equals("eYazisma") || mailType.Equals("eTebligat")))
				{
					message.AddHeaderField("X-TR-REM-iletiTip", mailType);
				}
				else if (string.IsNullOrEmpty(mailType) || mailType == "standart")
				{
					message.AddHeaderField("X-TR-REM-iletiTip", "standart");
				}
				if (!string.IsNullOrEmpty(mailTypeId))
				{
					message.AddHeaderField("X-TR-REM-iletiID", mailTypeId);
				}
				message.CheckBuiltMimePartTree();
				message.SmimeAttachSignatureBy(reader);
				try
				{
					byte[] bytes = Encoding.UTF8.GetBytes(message.ToMimeString());
					message = null;
					return bytes;
				}
				catch (Exception exception)
				{
					LOG.Error("S/Mime Mesaj MailMessage 'a Çevirilirken Hata Meydana Geldi.", exception);
					return null;
				}
			}
			throw new Exception("Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
		}

		public byte[] CreateWithSmartCard(string from, List<string> to, List<string> cc, List<string> bcc, string subject, string content, List<string> attachments, string mailType, string mailTypeId, EYazismaIcerikTur icerikTur, SmartCardReader reader, OzetAlg alg)
		{
			Message message = new Message();
			message.MessageId = MimeUtilities.GetUniqueMessageIDValue();
			message.From = new Address(from);
			AddressCollection addressCollection = new AddressCollection();
			if (to != null && to.Count != 0)
			{
				foreach (string item in to)
				{
					if (item == null)
					{
						throw new Exception("Mesajın Kime Alanı Null Değere Sahip Liste Elemanı Olamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
					}
					if (!MimeUtilities.EmailKontrol(item))
					{
						throw new Exception("Mesajın Kime Alanı KEP Mail Formatına Uygun Değil. Verdiğiniz Parametreleri Kontrol Ediniz.");
					}
					addressCollection.Add(new Address(item));
				}
				message.To = addressCollection;
				AddressCollection addressCollection2 = new AddressCollection();
				if (cc != null)
				{
					foreach (string item2 in cc)
					{
						if (item2 != null)
						{
							addressCollection2.Add(new Address(item2));
						}
					}
				}
				message.Cc = addressCollection2;
				AddressCollection addressCollection3 = new AddressCollection();
				if (bcc != null)
				{
					foreach (string item3 in bcc)
					{
						if (item3 != null)
						{
							addressCollection3.Add(new Address(item3));
						}
					}
				}
				message.Bcc = addressCollection3;
				message.Subject = Codec.RFC2047Encode(subject, "UTF-8");
				if (icerikTur == EYazismaIcerikTur.TEXT)
				{
					message.BodyText = new MimeBody(BodyFormat.Text)
					{
						Charset = "UTF-8",
						ContentTransferEncoding = ContentTransferEncoding.QuotedPrintable,
						Text = content
					};
				}
				else
				{
					message.BodyText = new MimeBody(BodyFormat.Html)
					{
						Charset = "UTF-8",
						ContentTransferEncoding = ContentTransferEncoding.QuotedPrintable,
						Text = content
					};
				}
				if (attachments != null)
				{
					foreach (string attachment in attachments)
					{
						if (attachment != null)
						{
							try
							{
								MimePart part = new MimePart(File.ReadAllBytes(attachment), MimeTypesHelper.GetMimeqType(attachment), Codec.RFC2047Encode(attachment, "UTF-8"), "UTF-8");
								message.Attachments.Add(part);
							}
							catch (Exception)
							{
								throw new Exception("Mail'e Eklenecek Olan Ekler Okunamadı. Eklerin Adresini Doğru Verdiğinizden Emin Olunuz.");
							}
						}
					}
				}
				if (mailType != null && (mailType.Equals("eYazisma") || mailType.Equals("eTebligat")))
				{
					message.AddHeaderField("X-TR-REM-iletiTip", mailType);
				}
				else if (string.IsNullOrEmpty(mailType) || mailType == "standart")
				{
					message.AddHeaderField("X-TR-REM-iletiTip", "standart");
				}
				if (!string.IsNullOrEmpty(mailTypeId))
				{
					message.AddHeaderField("X-TR-REM-iletiID", mailTypeId);
				}
				message.CheckBuiltMimePartTree();
				message.SmimeAttachSignatureBy(reader, alg);
				try
				{
					byte[] bytes = Encoding.UTF8.GetBytes(message.ToMimeString());
					message = null;
					return bytes;
				}
				catch (Exception exception)
				{
					LOG.Error("S/Mime Mesaj MailMessage 'a Çevirilirken Hata Meydana Geldi.", exception);
					return null;
				}
			}
			throw new Exception("Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
		}

		public byte[] CreateWithSmartCard(string from, List<string> to, List<string> cc, List<string> bcc, string subject, string content, List<Ek> attachments, string mailType, string mailTypeId, EYazismaIcerikTur icerikTur, SmartCardReader reader)
		{
			Message message = new Message();
			message.MessageId = MimeUtilities.GetUniqueMessageIDValue();
			message.From = new Address(from);
			AddressCollection addressCollection = new AddressCollection();
			if (to != null && to.Count != 0)
			{
				foreach (string item in to)
				{
					if (item == null)
					{
						throw new Exception("Mesajın Kime Alanı Null Değere Sahip Liste Elemanı Olamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
					}
					if (!MimeUtilities.EmailKontrol(item))
					{
						throw new Exception("Mesajın Kime Alanı KEP Mail Formatına Uygun Değil. Verdiğiniz Parametreleri Kontrol Ediniz.");
					}
					addressCollection.Add(new Address(item));
				}
				message.To = addressCollection;
				AddressCollection addressCollection2 = new AddressCollection();
				if (cc != null)
				{
					foreach (string item2 in cc)
					{
						if (item2 != null)
						{
							addressCollection2.Add(new Address(item2));
						}
					}
				}
				message.Cc = addressCollection2;
				AddressCollection addressCollection3 = new AddressCollection();
				if (bcc != null)
				{
					foreach (string item3 in bcc)
					{
						if (item3 != null)
						{
							addressCollection3.Add(new Address(item3));
						}
					}
				}
				message.Bcc = addressCollection3;
				message.Subject = Codec.RFC2047Encode(subject, "UTF-8");
				if (icerikTur == EYazismaIcerikTur.TEXT)
				{
					message.BodyText = new MimeBody(BodyFormat.Text)
					{
						Charset = "UTF-8",
						ContentTransferEncoding = ContentTransferEncoding.QuotedPrintable,
						Text = content
					};
				}
				else
				{
					message.BodyText = new MimeBody(BodyFormat.Html)
					{
						Charset = "UTF-8",
						ContentTransferEncoding = ContentTransferEncoding.QuotedPrintable,
						Text = content
					};
				}
				if (attachments != null)
				{
					foreach (Ek attachment in attachments)
					{
						if (attachment != null)
						{
							try
							{
								MimePart part = new MimePart(attachment.Degeri, MimeTypesHelper.GetMimeqType(attachment.Adi), Codec.RFC2047Encode(attachment.Adi, "UTF-8"), "UTF-8");
								message.Attachments.Add(part);
							}
							catch (Exception)
							{
								throw new Exception("Mail'e Eklenecek Olan Ekler Okunamadı. Eklerin Adresini Doğru Verdiğinizden Emin Olunuz.");
							}
						}
					}
				}
				if (mailType != null && (mailType.Equals("eYazisma") || mailType.Equals("eTebligat")))
				{
					message.AddHeaderField("X-TR-REM-iletiTip", mailType);
				}
				else if (string.IsNullOrEmpty(mailType) || mailType == "standart")
				{
					message.AddHeaderField("X-TR-REM-iletiTip", "standart");
				}
				if (!string.IsNullOrEmpty(mailTypeId))
				{
					message.AddHeaderField("X-TR-REM-iletiID", mailTypeId);
				}
				message.CheckBuiltMimePartTree();
				message.SmimeAttachSignatureBy(reader);
				try
				{
					byte[] bytes = Encoding.UTF8.GetBytes(message.ToMimeString());
					message = null;
					return bytes;
				}
				catch (Exception exception)
				{
					LOG.Error("S/Mime Mesaj MailMessage 'a Çevirilirken Hata Meydana Geldi.", exception);
					return null;
				}
			}
			throw new Exception("Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
		}

		public byte[] CreateWithSmartCard(string from, List<string> to, List<string> cc, List<string> bcc, string subject, string content, List<Ek> attachments, string mailType, string mailTypeId, EYazismaIcerikTur icerikTur, SmartCardReader reader, OzetAlg alg)
		{
			Message message = new Message();
			message.MessageId = MimeUtilities.GetUniqueMessageIDValue();
			message.From = new Address(from);
			AddressCollection addressCollection = new AddressCollection();
			if (to != null && to.Count != 0)
			{
				foreach (string item in to)
				{
					if (item == null)
					{
						throw new Exception("Mesajın Kime Alanı Null Değere Sahip Liste Elemanı Olamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
					}
					if (!MimeUtilities.EmailKontrol(item))
					{
						throw new Exception("Mesajın Kime Alanı KEP Mail Formatına Uygun Değil. Verdiğiniz Parametreleri Kontrol Ediniz.");
					}
					addressCollection.Add(new Address(item));
				}
				message.To = addressCollection;
				AddressCollection addressCollection2 = new AddressCollection();
				if (cc != null)
				{
					foreach (string item2 in cc)
					{
						if (item2 != null)
						{
							addressCollection2.Add(new Address(item2));
						}
					}
				}
				message.Cc = addressCollection2;
				AddressCollection addressCollection3 = new AddressCollection();
				if (bcc != null)
				{
					foreach (string item3 in bcc)
					{
						if (item3 != null)
						{
							addressCollection3.Add(new Address(item3));
						}
					}
				}
				message.Bcc = addressCollection3;
				message.Subject = Codec.RFC2047Encode(subject, "UTF-8");
				if (icerikTur == EYazismaIcerikTur.TEXT)
				{
					message.BodyText = new MimeBody(BodyFormat.Text)
					{
						Charset = "UTF-8",
						ContentTransferEncoding = ContentTransferEncoding.QuotedPrintable,
						Text = content
					};
				}
				else
				{
					message.BodyText = new MimeBody(BodyFormat.Html)
					{
						Charset = "UTF-8",
						ContentTransferEncoding = ContentTransferEncoding.QuotedPrintable,
						Text = content
					};
				}
				if (attachments != null)
				{
					foreach (Ek attachment in attachments)
					{
						if (attachment != null)
						{
							try
							{
								MimePart part = new MimePart(attachment.Degeri, MimeTypesHelper.GetMimeqType(attachment.Adi), Codec.RFC2047Encode(attachment.Adi, "UTF-8"), "UTF-8");
								message.Attachments.Add(part);
							}
							catch (Exception)
							{
								throw new Exception("Mail'e Eklenecek Olan Ekler Okunamadı. Eklerin Adresini Doğru Verdiğinizden Emin Olunuz.");
							}
						}
					}
				}
				if (mailType != null && (mailType.Equals("eYazisma") || mailType.Equals("eTebligat")))
				{
					message.AddHeaderField("X-TR-REM-iletiTip", mailType);
				}
				else if (string.IsNullOrEmpty(mailType) || mailType == "standart")
				{
					message.AddHeaderField("X-TR-REM-iletiTip", "standart");
				}
				if (!string.IsNullOrEmpty(mailTypeId))
				{
					message.AddHeaderField("X-TR-REM-iletiID", mailTypeId);
				}
				message.CheckBuiltMimePartTree();
				message.SmimeAttachSignatureBy(reader, alg);
				try
				{
					byte[] bytes = Encoding.UTF8.GetBytes(message.ToMimeString());
					message = null;
					return bytes;
				}
				catch (Exception exception)
				{
					LOG.Error("S/Mime Mesaj MailMessage 'a Çevirilirken Hata Meydana Geldi.", exception);
					return null;
				}
			}
			throw new Exception("Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
		}
	}
}
