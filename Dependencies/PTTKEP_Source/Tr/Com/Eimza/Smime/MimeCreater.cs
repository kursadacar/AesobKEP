using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Tr.Com.Eimza.EYazisma;
using Tr.Com.Eimza.EYazisma.Config;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.Smime
{
	internal class MimeCreater
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal EyMimeOlusturSonuc Create(EyMimeMessage mimeMesaj, List<Ek> ekler, OzetAlg hashAlg, EYazismaApiConfig config)
		{
			EyMimeOlusturSonuc eyMimeOlusturSonuc = new EyMimeOlusturSonuc();
			if (ekler != null && ekler.Count > 0)
			{
				long num = 0L;
				foreach (Ek item in ekler)
				{
					num += Encoding.ASCII.GetBytes(Convert.ToBase64String(item.Degeri)).LongLength;
				}
				float num2 = num;
				float num3 = 0f;
				switch (config.MaxMailDosyaBoyutu.Cins)
				{
				case Boyut.Suffix.KB:
					num3 = config.MaxMailDosyaBoyutu.Size * 1024f;
					break;
				case Boyut.Suffix.MB:
					num3 = config.MaxMailDosyaBoyutu.Size * 1024f * 1024f;
					break;
				}
				num3 = num3 * 4f / 3f;
				num3 += 285004f / (float)Math.E;
				if (num2 > num3)
				{
					EyLog.Log("Mime Oluştur", EyLogTuru.HATA, "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.");
					eyMimeOlusturSonuc.HataAciklama = "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.";
					eyMimeOlusturSonuc.Durum = 161;
					return eyMimeOlusturSonuc;
				}
			}
			try
			{
				SMailMessage sMailMessage = new SMailMessage();
				sMailMessage.Headers.Add("Message-ID", MimeUtilities.GetUniqueMessageIDValue());
				sMailMessage.From = new SMailAddress(mimeMesaj.Kimden);
				if (mimeMesaj.Kime != null && mimeMesaj.Kime.Count != 0)
				{
					foreach (string item2 in mimeMesaj.Kime)
					{
						if (item2 == null)
						{
							return new EyMimeOlusturSonuc
							{
								Durum = 161,
								HataAciklama = "Mesajın Kime Alanı Null Değere Sahip Liste Elemanı Olamaz. Verdiğiniz Parametreleri Kontrol Ediniz."
							};
						}
						if (!MimeUtilities.EmailKontrol(item2))
						{
							return new EyMimeOlusturSonuc
							{
								Durum = 161,
								HataAciklama = "Mesajın Kime Alanı KEP Mail Formatına Uygun Değil. Verdiğiniz Parametreleri Kontrol Ediniz."
							};
						}
						sMailMessage.To.Add(new SMailAddress(item2));
					}
					if (mimeMesaj.Cc != null)
					{
						foreach (string item3 in mimeMesaj.Cc)
						{
							if (item3 != null)
							{
								sMailMessage.CC.Add(new SMailAddress(item3));
							}
						}
					}
					if (mimeMesaj.Bcc != null)
					{
						foreach (string item4 in mimeMesaj.Bcc)
						{
							if (item4 != null)
							{
								sMailMessage.Bcc.Add(new SMailAddress(item4));
							}
						}
					}
					sMailMessage.Subject = mimeMesaj.Konu;
					sMailMessage.Body = mimeMesaj.Icerik;
					sMailMessage.BodyEncoding = Encoding.UTF8;
					sMailMessage.IsBodyHtml = false;
					if (ekler != null)
					{
						foreach (Ek item5 in ekler)
						{
							if (item5 != null && item5.Degeri != null)
							{
								sMailMessage.Attachments.Add(new SMailAttachment(item5.Degeri, item5.Adi));
							}
						}
					}
					eyMimeOlusturSonuc.MailBilgileri = mimeMesaj;
					string value = "";
					switch (mimeMesaj.MailTipi)
					{
					case EYazismaPaketTur.ETEbligat:
						value = "eTebligat";
						break;
					case EYazismaPaketTur.EYazisma:
						value = "eYazisma";
						break;
					case EYazismaPaketTur.Standart:
						value = "standart";
						break;
					}
					if (!string.IsNullOrEmpty(value))
					{
						sMailMessage.Headers.Add("X-TR-REM-iletiTip", value);
					}
					else
					{
						sMailMessage.Headers.Add("X-TR-REM-iletiTip", "standart");
					}
					if (!string.IsNullOrEmpty(mimeMesaj.MailTipId))
					{
						sMailMessage.Headers.Add("X-TR-REM-iletiID", mimeMesaj.MailTipId);
					}
					sMailMessage.IsSigned = false;
					eyMimeOlusturSonuc.Icerik = sMailMessage.GetContent();
					eyMimeOlusturSonuc.KepIletisi = sMailMessage.ToMailMessage().GetBytesOfMessage();
					sMailMessage = null;
					switch (hashAlg)
					{
					case OzetAlg.SHA256:
						eyMimeOlusturSonuc.Hash = new SHA256Managed().ComputeHash(eyMimeOlusturSonuc.Icerik);
						eyMimeOlusturSonuc.HashBase64 = Convert.ToBase64String(eyMimeOlusturSonuc.Hash);
						break;
					case OzetAlg.SHA512:
						eyMimeOlusturSonuc.Hash = new SHA512Managed().ComputeHash(eyMimeOlusturSonuc.Icerik);
						eyMimeOlusturSonuc.HashBase64 = Convert.ToBase64String(eyMimeOlusturSonuc.Hash);
						break;
					}
					eyMimeOlusturSonuc.OzetAlgoritmasi = hashAlg;
					return eyMimeOlusturSonuc;
				}
				return new EyMimeOlusturSonuc
				{
					Durum = 161,
					HataAciklama = "Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz."
				};
			}
			catch (Exception exception)
			{
				LOG.Error("Mime Mesaj Oluşturulurken Hata Meydana Geldi.", exception);
				return null;
			}
		}

		internal EyMimeOlusturSonuc Create(EyMimeMessage mimeMesaj, List<string> eklerAdresi, OzetAlg hashAlg, EYazismaApiConfig config)
		{
			EyMimeOlusturSonuc eyMimeOlusturSonuc = new EyMimeOlusturSonuc();
			List<base64Binary> list = new List<base64Binary>();
			if (eklerAdresi != null)
			{
				foreach (string item in eklerAdresi)
				{
					base64Binary base64Binary = new base64Binary();
					FileStream fileStream = null;
					byte[] array;
					try
					{
						fileStream = new FileStream(item, FileMode.Open, FileAccess.Read);
						array = new byte[fileStream.Length];
						fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
					}
					catch (Exception exception)
					{
						string text = "Verdiğiniz Eklerin Adreslerini Kontrol Ediniz.";
						EyLog.Log("Mime Oluştur", EyLogTuru.HATA, text);
						LOG.Error(text, exception);
						throw new EYazismaException(text);
					}
					finally
					{
						if (fileStream != null)
						{
							fileStream.Close();
						}
					}
					base64Binary.Value = array;
					base64Binary.fileName = Path.GetFileName(item);
					base64Binary.contentType = "application/octet-stream";
					list.Add(base64Binary);
				}
			}
			long num = 0L;
			if (list.Count > 0)
			{
				foreach (base64Binary item2 in list)
				{
					num += item2.Value.LongLength;
				}
				float num2 = num;
				float num3 = 0f;
				switch (config.MaxMailDosyaBoyutu.Cins)
				{
				case Boyut.Suffix.KB:
					num3 = config.MaxMailDosyaBoyutu.Size * 1024f;
					break;
				case Boyut.Suffix.MB:
					num3 = config.MaxMailDosyaBoyutu.Size * 1024f * 1024f;
					break;
				}
				num3 = num3 * 4f / 3f;
				num3 += 285004f / (float)Math.E;
				if (num2 > num3)
				{
					string text2 = "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.";
					EyLog.Log("Mime Oluştur", EyLogTuru.HATA, text2);
					throw new EYazismaException(text2);
				}
			}
			try
			{
				SMailMessage sMailMessage = new SMailMessage();
				sMailMessage.Headers.Add("Message-ID", MimeUtilities.GetUniqueMessageIDValue());
				sMailMessage.From = new SMailAddress(mimeMesaj.Kimden);
				if (mimeMesaj.Kime != null && mimeMesaj.Kime.Count != 0)
				{
					foreach (string item3 in mimeMesaj.Kime)
					{
						if (item3 == null)
						{
							return new EyMimeOlusturSonuc
							{
								Durum = 161,
								HataAciklama = "Mesajın Kime Alanı Null Değere Sahip Liste Elemanı Olamaz. Verdiğiniz Parametreleri Kontrol Ediniz."
							};
						}
						if (!MimeUtilities.EmailKontrol(item3))
						{
							return new EyMimeOlusturSonuc
							{
								Durum = 161,
								HataAciklama = "Mesajın Kime Alanı KEP Mail Formatına Uygun Değil. Verdiğiniz Parametreleri Kontrol Ediniz."
							};
						}
						sMailMessage.To.Add(new SMailAddress(item3));
					}
					if (mimeMesaj.Cc != null)
					{
						foreach (string item4 in mimeMesaj.Cc)
						{
							if (item4 != null)
							{
								sMailMessage.CC.Add(new SMailAddress(item4));
							}
						}
					}
					if (mimeMesaj.Bcc != null)
					{
						foreach (string item5 in mimeMesaj.Bcc)
						{
							if (item5 != null)
							{
								sMailMessage.Bcc.Add(new SMailAddress(item5));
							}
						}
					}
					sMailMessage.Subject = mimeMesaj.Konu;
					sMailMessage.Body = mimeMesaj.Icerik;
					sMailMessage.BodyEncoding = Encoding.UTF8;
					sMailMessage.IsBodyHtml = false;
					foreach (base64Binary item6 in list)
					{
						if (item6 != null && item6.Value != null)
						{
							sMailMessage.Attachments.Add(new SMailAttachment(item6.Value, item6.fileName));
						}
					}
					eyMimeOlusturSonuc.MailBilgileri = mimeMesaj;
					string value = "";
					switch (mimeMesaj.MailTipi)
					{
					case EYazismaPaketTur.ETEbligat:
						value = "eTebligat";
						break;
					case EYazismaPaketTur.EYazisma:
						value = "eYazisma";
						break;
					case EYazismaPaketTur.Standart:
						value = "standart";
						break;
					}
					if (!string.IsNullOrEmpty(value))
					{
						sMailMessage.Headers.Add("X-TR-REM-iletiTip", value);
					}
					else
					{
						sMailMessage.Headers.Add("X-TR-REM-iletiTip", "standart");
					}
					if (!string.IsNullOrEmpty(mimeMesaj.MailTipId))
					{
						sMailMessage.Headers.Add("X-TR-REM-iletiID", mimeMesaj.MailTipId);
					}
					sMailMessage.IsSigned = false;
					eyMimeOlusturSonuc.Icerik = sMailMessage.GetContent();
					eyMimeOlusturSonuc.KepIletisi = sMailMessage.ToMailMessage().GetBytesOfMessage();
					sMailMessage = null;
					switch (hashAlg)
					{
					case OzetAlg.SHA256:
						eyMimeOlusturSonuc.Hash = new SHA256Managed().ComputeHash(eyMimeOlusturSonuc.Icerik);
						eyMimeOlusturSonuc.HashBase64 = Convert.ToBase64String(eyMimeOlusturSonuc.Hash);
						break;
					case OzetAlg.SHA512:
						eyMimeOlusturSonuc.Hash = new SHA512Managed().ComputeHash(eyMimeOlusturSonuc.Icerik);
						eyMimeOlusturSonuc.HashBase64 = Convert.ToBase64String(eyMimeOlusturSonuc.Hash);
						break;
					}
					eyMimeOlusturSonuc.OzetAlgoritmasi = hashAlg;
					return eyMimeOlusturSonuc;
				}
				return new EyMimeOlusturSonuc
				{
					Durum = 161,
					HataAciklama = "Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz."
				};
			}
			catch (Exception exception2)
			{
				LOG.Error("Mime Mesaj Oluşturulurken Hata Meydana Geldi.", exception2);
				return null;
			}
		}
	}
}
