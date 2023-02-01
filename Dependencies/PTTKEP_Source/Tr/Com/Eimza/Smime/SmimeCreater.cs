using System;
using System.Reflection;
using System.Text;
using Tr.Com.Eimza.EYazisma;
using Tr.Com.Eimza.EYazisma.Config;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.Smime
{
	internal class SmimeCreater
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal EySmimeOlusturSonuc Create(EyMimeMessage mimeMesaj, byte[] icerik, byte[] signOfMime, EYazismaApiConfig config)
		{
			EySmimeOlusturSonuc eySmimeOlusturSonuc = new EySmimeOlusturSonuc();
			if (signOfMime == null)
			{
				EyLog.Log("S/Mime Oluştur", EyLogTuru.HATA, "S/Mime Pakete Konulacak Olan Imza Verisi Boş Bırakılamaz.");
				eySmimeOlusturSonuc.HataAciklama = "S/Mime Pakete Konulacak Olan Imza Verisi Boş Bırakılamaz.";
				eySmimeOlusturSonuc.Durum = 161;
				return eySmimeOlusturSonuc;
			}
			if (icerik != null)
			{
				float num = icerik.LongLength;
				float num2 = 0f;
				switch (config.MaxMailDosyaBoyutu.Cins)
				{
				case Boyut.Suffix.KB:
					num2 = config.MaxMailDosyaBoyutu.Size * 1024f;
					break;
				case Boyut.Suffix.MB:
					num2 = config.MaxMailDosyaBoyutu.Size * 1024f * 1024f;
					break;
				}
				num2 = num2 * 4f / 3f;
				num2 += 285004f / (float)Math.E;
				if (num > num2)
				{
					EyLog.Log("S/Mime Oluştur", EyLogTuru.HATA, "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.");
					eySmimeOlusturSonuc.HataAciklama = "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.";
					eySmimeOlusturSonuc.Durum = 161;
					return eySmimeOlusturSonuc;
				}
			}
			if (icerik == null || icerik.Length == 0)
			{
				EyLog.Log("S/Mime Oluştur", EyLogTuru.HATA, "Içerik Değeri Boş Bırakılamaz.");
				eySmimeOlusturSonuc.HataAciklama = "Içerik Değeri Boş Bırakılamaz.";
				eySmimeOlusturSonuc.Durum = 161;
				return eySmimeOlusturSonuc;
			}
			SMailMessage sMailMessage = new SMailMessage();
			try
			{
				eySmimeOlusturSonuc.MesajId = MimeUtilities.GetUniqueMessageIDValue();
				sMailMessage.Headers.Add("Message-ID", eySmimeOlusturSonuc.MesajId);
				sMailMessage.From = new SMailAddress(mimeMesaj.Kimden);
				if (mimeMesaj.Kime == null)
				{
					return new EySmimeOlusturSonuc
					{
						Durum = 161,
						HataAciklama = "Mesajın Kime Alanı Boş Bırakılamaz. Verdiğiniz Parametreleri Kontrol Ediniz."
					};
				}
				foreach (string item in mimeMesaj.Kime)
				{
					if (item == null)
					{
						return new EySmimeOlusturSonuc
						{
							Durum = 161,
							HataAciklama = "Mesajın Kime Alanı Null Değere Sahip Liste Elemanı Olamaz. Verdiğiniz Parametreleri Kontrol Ediniz."
						};
					}
					if (!MimeUtilities.EmailKontrol(item))
					{
						return new EySmimeOlusturSonuc
						{
							Durum = 161,
							HataAciklama = "Mesajın Kime Alanı KEP Mail Formatına Uygun Değil. Verdiğiniz Parametreleri Kontrol Ediniz."
						};
					}
					sMailMessage.To.Add(new SMailAddress(item));
				}
				if (mimeMesaj.Cc != null)
				{
					foreach (string item2 in mimeMesaj.Cc)
					{
						if (item2 != null)
						{
							sMailMessage.CC.Add(new SMailAddress(item2));
						}
					}
				}
				if (mimeMesaj.Bcc != null)
				{
					foreach (string item3 in mimeMesaj.Bcc)
					{
						if (item3 != null)
						{
							sMailMessage.Bcc.Add(new SMailAddress(item3));
						}
					}
				}
				sMailMessage.Subject = mimeMesaj.Konu;
				sMailMessage.IsBodyHtml = false;
				sMailMessage.BodyEncoding = Encoding.UTF8;
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
				sMailMessage.UnSignedContent = icerik;
				sMailMessage.SignedContent = signOfMime;
				sMailMessage.IsSigned = true;
			}
			catch (Exception exception)
			{
				LOG.Error("S/Mime Mesaj Oluşturulurken Hata Meydana Geldi. Gönderdiğiniz Parametreleri Kontrol Ediniz...", exception);
				return new EySmimeOlusturSonuc
				{
					HataAciklama = "S/Mime Mesaj Oluşturulurken Hata Meydana Geldi. Gönderdiğiniz Parametreleri Kontrol Ediniz...",
					Durum = 161
				};
			}
			try
			{
				eySmimeOlusturSonuc.KepIletisi = sMailMessage.ToMailMessage().GetBytesOfMessage();
				eySmimeOlusturSonuc.Durum = 0;
				eySmimeOlusturSonuc.HataAciklama = "";
				sMailMessage = null;
				return eySmimeOlusturSonuc;
			}
			catch (Exception exception2)
			{
				LOG.Error("S/Mime ileti MailMessage'a Çevirilirken Hata Meydana Geldi.", exception2);
				return null;
			}
		}
	}
}
