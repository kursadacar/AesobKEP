using System;
using System.Collections.Generic;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;
using Tr.Com.Eimza.SecureMail;
using Tr.Com.Eimza.SmartCard;

namespace Tr.Com.Eimza.EYazisma.Smime
{
	internal class SmimeCreater
	{
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public byte[] CreateWithSmartCard(string from, List<string> to, List<string> cc, List<string> bcc, string subject, string content, List<string> attachments, string mailType, string mailTypeId, SmartCardReader reader)
		{
			SecureMailMessage secureMailMessage = new SecureMailMessage();
			secureMailMessage.Headers.Add("Message-ID", MimeUtilities.GetUniqueMessageIDValue());
			secureMailMessage.From = new SecureMailAddress(from, reader);
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
					secureMailMessage.To.Add(new SecureMailAddress(item));
				}
				if (cc != null)
				{
					foreach (string item2 in cc)
					{
						if (item2 != null)
						{
							secureMailMessage.CC.Add(new SecureMailAddress(item2));
						}
					}
				}
				if (bcc != null)
				{
					foreach (string item3 in bcc)
					{
						if (item3 != null)
						{
							secureMailMessage.Bcc.Add(new SecureMailAddress(item3));
						}
					}
				}
				secureMailMessage.Subject = subject;
				secureMailMessage.Body = content;
				secureMailMessage.IsBodyHtml = false;
				if (attachments != null)
				{
					foreach (string attachment in attachments)
					{
						if (attachment != null)
						{
							try
							{
								secureMailMessage.Attachments.Add(new SecureAttachment(attachment));
							}
							catch (Exception)
							{
								throw new Exception("Mail'e Eklenecek Olan Ekler Okunamadı. Eklerin Adresini Doğru Verdiğinizden Emin Olunuz.");
							}
						}
					}
				}
				if (mailType.Equals("eYazisma") || mailType.Equals("eTebligat"))
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiTip", mailType);
				}
				else if (string.IsNullOrEmpty(mailType) || mailType == "standart")
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiTip", "standart");
				}
				if (!string.IsNullOrEmpty(mailTypeId))
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiID", mailTypeId);
				}
				secureMailMessage.IsSigned = true;
				try
				{
					byte[] bytesOfMessage = secureMailMessage.ToMailMessage().GetBytesOfMessage();
					secureMailMessage.Dispose();
					return bytesOfMessage;
				}
				catch (Exception exception)
				{
					logger.Error("S/Mime Mesaj MailMessage 'a Çevirilirken Hata Meydana Geldi.", exception);
					return null;
				}
			}
			throw new Exception("Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
		}

		public byte[] CreateWithSmartCard(string from, List<string> to, List<string> cc, List<string> bcc, string subject, string content, List<string> attachments, string mailType, string mailTypeId, SmartCardReader reader, OzetAlg ozetAlg)
		{
			SecureMailMessage secureMailMessage = new SecureMailMessage();
			secureMailMessage.Headers.Add("Message-ID", MimeUtilities.GetUniqueMessageIDValue());
			secureMailMessage.From = new SecureMailAddress(from, reader, ozetAlg);
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
					secureMailMessage.To.Add(new SecureMailAddress(item));
				}
				if (cc != null)
				{
					foreach (string item2 in cc)
					{
						if (item2 != null)
						{
							secureMailMessage.CC.Add(new SecureMailAddress(item2));
						}
					}
				}
				if (bcc != null)
				{
					foreach (string item3 in bcc)
					{
						if (item3 != null)
						{
							secureMailMessage.Bcc.Add(new SecureMailAddress(item3));
						}
					}
				}
				secureMailMessage.Subject = subject;
				secureMailMessage.Body = content;
				secureMailMessage.IsBodyHtml = false;
				if (attachments != null)
				{
					foreach (string attachment in attachments)
					{
						if (attachment != null)
						{
							try
							{
								secureMailMessage.Attachments.Add(new SecureAttachment(attachment));
							}
							catch (Exception)
							{
								throw new Exception("Mail'e Eklenecek Olan Ekler Okunamadı. Eklerin Adresini Doğru Verdiğinizden Emin Olunuz.");
							}
						}
					}
				}
				if (mailType.Equals("eYazisma") || mailType.Equals("eTebligat"))
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiTip", mailType);
				}
				else if (string.IsNullOrEmpty(mailType) || mailType == "standart")
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiTip", "standart");
				}
				if (!string.IsNullOrEmpty(mailTypeId))
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiID", mailTypeId);
				}
				secureMailMessage.IsSigned = true;
				try
				{
					byte[] bytesOfMessage = secureMailMessage.ToMailMessage().GetBytesOfMessage();
					secureMailMessage.Dispose();
					return bytesOfMessage;
				}
				catch (Exception exception)
				{
					logger.Error("S/Mime Mesaj MailMessage 'a Çevirilirken Hata Meydana Geldi.", exception);
					return null;
				}
			}
			throw new Exception("Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
		}

		public byte[] CreateWithSmartCard(string from, List<string> to, List<string> cc, List<string> bcc, string subject, string content, List<Ek> attachments, string mailType, string mailTypeId, SmartCardReader reader)
		{
			SecureMailMessage secureMailMessage = new SecureMailMessage();
			secureMailMessage.Headers.Add("Message-ID", MimeUtilities.GetUniqueMessageIDValue());
			secureMailMessage.From = new SecureMailAddress(from, reader);
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
					secureMailMessage.To.Add(new SecureMailAddress(item));
				}
				if (cc != null)
				{
					foreach (string item2 in cc)
					{
						if (item2 != null)
						{
							secureMailMessage.CC.Add(new SecureMailAddress(item2));
						}
					}
				}
				if (bcc != null)
				{
					foreach (string item3 in bcc)
					{
						if (item3 != null)
						{
							secureMailMessage.Bcc.Add(new SecureMailAddress(item3));
						}
					}
				}
				secureMailMessage.Subject = subject;
				secureMailMessage.Body = content;
				secureMailMessage.IsBodyHtml = false;
				if (attachments != null)
				{
					foreach (Ek attachment in attachments)
					{
						if (attachment != null && attachment.Degeri != null)
						{
							secureMailMessage.Attachments.Add(new SecureAttachment(attachment.Degeri, attachment.Adi));
						}
					}
				}
				if (mailType.Equals("eYazisma") || mailType.Equals("eTebligat"))
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiTip", mailType);
				}
				else if (string.IsNullOrEmpty(mailType) || mailType == "standart")
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiTip", "standart");
				}
				if (!string.IsNullOrEmpty(mailTypeId))
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiID", mailTypeId);
				}
				secureMailMessage.IsSigned = true;
				try
				{
					byte[] bytesOfMessage = secureMailMessage.ToMailMessage().GetBytesOfMessage();
					secureMailMessage.Dispose();
					return bytesOfMessage;
				}
				catch (Exception exception)
				{
					logger.Error("S/Mime Mesaj MailMessage 'a Çevirilirken Hata Meydana Geldi.", exception);
					return null;
				}
			}
			throw new Exception("Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
		}

		public byte[] CreateWithSmartCard(string from, List<string> to, List<string> cc, List<string> bcc, string subject, string content, List<Ek> attachments, string mailType, string mailTypeId, SmartCardReader reader, OzetAlg ozetAlg)
		{
			SecureMailMessage secureMailMessage = new SecureMailMessage();
			secureMailMessage.Headers.Add("Message-ID", MimeUtilities.GetUniqueMessageIDValue());
			secureMailMessage.From = new SecureMailAddress(from, reader, ozetAlg);
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
					secureMailMessage.To.Add(new SecureMailAddress(item));
				}
				if (cc != null)
				{
					foreach (string item2 in cc)
					{
						if (item2 != null)
						{
							secureMailMessage.CC.Add(new SecureMailAddress(item2));
						}
					}
				}
				if (bcc != null)
				{
					foreach (string item3 in bcc)
					{
						if (item3 != null)
						{
							secureMailMessage.Bcc.Add(new SecureMailAddress(item3));
						}
					}
				}
				secureMailMessage.Subject = subject;
				secureMailMessage.Body = content;
				secureMailMessage.IsBodyHtml = false;
				if (attachments != null)
				{
					foreach (Ek attachment in attachments)
					{
						if (attachment != null && attachment.Degeri != null)
						{
							secureMailMessage.Attachments.Add(new SecureAttachment(attachment.Degeri, attachment.Adi));
						}
					}
				}
				if (mailType.Equals("eYazisma") || mailType.Equals("eTebligat"))
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiTip", mailType);
				}
				else if (string.IsNullOrEmpty(mailType) || mailType == "standart")
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiTip", "standart");
				}
				if (!string.IsNullOrEmpty(mailTypeId))
				{
					secureMailMessage.Headers.Add("X-TR-REM-iletiID", mailTypeId);
				}
				secureMailMessage.IsSigned = true;
				try
				{
					byte[] bytesOfMessage = secureMailMessage.ToMailMessage().GetBytesOfMessage();
					secureMailMessage.Dispose();
					return bytesOfMessage;
				}
				catch (Exception exception)
				{
					logger.Error("S/Mime Mesaj MailMessage 'a Çevirilirken Hata Meydana Geldi.", exception);
					return null;
				}
			}
			throw new Exception("Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz.");
		}
	}
}
