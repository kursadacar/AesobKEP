using System;
using System.IO;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaPaketIndir
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyPaketIndirSonuc PaketIndir(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string messageKepId, string eGuvenlikId, EYazismaPart part, string dizin, string mesajKayitAdresi = null, bool Save = false)
		{
			if (string.IsNullOrEmpty(messageKepId))
			{
				string text = "KepID Parametresini Boş Bırakamazsınız.";
				EyLog.Log("Paket Indir", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			if (string.IsNullOrEmpty(mesajKayitAdresi) && Save)
			{
				string text2 = "Kep İletisi Kayıt Adresi Boş Bırakılamaz.";
				EyLog.Log("Paket Indir", EyLogTuru.HATA, text2);
				throw new EYazismaException(text2);
			}
			try
			{
				eyIndir eyIndir = new eyIndir();
				eyIndir.kepHesap = kepHesap;
				eyIndir.kepId = messageKepId;
				eyIndir.eGuvenlikId = eGuvenlikId;
				eyIndir.kepSiraNoSpecified = false;
				eyIndir.ePart = (eyPart)part;
				eyIndir.dizin = dizin;
				eyIndirSonuc eyIndirSonuc;
				try
				{
					eyIndirSonuc = client.Indir(eyIndir);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyIndirSonuc.ePaket == null)
				{
					EyLog.Error("Paket Indir", Convert.ToString(eyIndirSonuc.durum), eyIndirSonuc.hataaciklama);
					return EyPaketIndirSonuc.GetEyIndirSonuc(eyIndirSonuc);
				}
				if (eyIndirSonuc.ePaket[0].Value == null)
				{
					EyLog.Error("Paket Indir", "-1", "Paket Bulunamadı.");
					return EyPaketIndirSonuc.GetEyIndirSonuc(eyIndirSonuc);
				}
				if (!string.IsNullOrEmpty(eyIndirSonuc.hataaciklama))
				{
					EyLog.Error("Paket Indir", Convert.ToString(eyIndirSonuc.durum), eyIndirSonuc.hataaciklama);
					return EyPaketIndirSonuc.GetEyIndirSonuc(eyIndirSonuc);
				}
				if (Save)
				{
					base64Binary[] ePaket = eyIndirSonuc.ePaket;
					if (ePaket != null)
					{
						base64Binary[] array = ePaket;
						foreach (base64Binary base64Binary in array)
						{
							string text3 = "";
							text3 = ((base64Binary.fileName == null) ? ("Mesaj-" + MimeUtilities.GetCurrentMiliSecond() + ".eml") : ((!(Path.GetExtension(base64Binary.fileName) == "")) ? ("Mesaj-" + MimeUtilities.GetCurrentMiliSecond() + Path.GetExtension(base64Binary.fileName)) : ("Mesaj-" + MimeUtilities.GetCurrentMiliSecond() + ".eml")));
							try
							{
								string savePath = Path.Combine(mesajKayitAdresi, text3);
								FileUtils.SaveFile(base64Binary.Value, savePath);
							}
							catch (Exception exception2)
							{
								LOG.Error("Paket Dosyası Diske Yazılırken Hata Meydana Geldi.", exception2);
								EyLog.Log("Paket Indir", EyLogTuru.UYARI, "Paket Dosyası Diske Yazılırken Hata Meydana Geldi.");
								EyLog.Log("Paket Indir", EyLogTuru.BILGI, "Paket'e PaketIndir Fonksiyonundan Geri Dönen EyPaketIndirSonuc Altındaki EYazismaPaketi Properties'inden Erişebilirsiniz.");
								return EyPaketIndirSonuc.GetEyIndirSonuc(eyIndirSonuc);
							}
							EyLog.Log("Paket Indir", EyLogTuru.BILGI, "Sonuç: Başarılı", "Paket, " + Path.Combine(mesajKayitAdresi, text3) + " kaydedildi.");
						}
					}
				}
				return EyPaketIndirSonuc.GetEyIndirSonuc(eyIndirSonuc);
			}
			catch (Exception exception3)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception3);
				return null;
			}
		}

		internal static EyPaketIndirSonuc PaketIndir(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, int kepSiraNo, string eGuvenlikId, EYazismaPart part, string dizin, string mesajKayitAdresi = null, bool Save = false)
		{
			if (string.IsNullOrEmpty(mesajKayitAdresi) && Save)
			{
				string text = "Kep İletisi Kayıt Adresi Boş Bırakılamaz.";
				EyLog.Log("Paket Indir", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eyIndir eyIndir = new eyIndir();
				eyIndir.kepHesap = kepHesap;
				eyIndir.eGuvenlikId = eGuvenlikId;
				eyIndir.kepSiraNo = kepSiraNo;
				eyIndir.kepSiraNoSpecified = true;
				eyIndir.ePart = (eyPart)part;
				eyIndir.dizin = dizin;
				eyIndirSonuc eyIndirSonuc;
				try
				{
					eyIndirSonuc = client.Indir(eyIndir);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyIndirSonuc.ePaket == null)
				{
					EyLog.Error("Paket Indir", Convert.ToString(eyIndirSonuc.durum), eyIndirSonuc.hataaciklama);
					return EyPaketIndirSonuc.GetEyIndirSonuc(eyIndirSonuc);
				}
				if (eyIndirSonuc.ePaket[0].Value == null)
				{
					EyLog.Error("Paket Indir", "-1", "Paket Bulunamadı.");
					return EyPaketIndirSonuc.GetEyIndirSonuc(eyIndirSonuc);
				}
				if (!string.IsNullOrEmpty(eyIndirSonuc.hataaciklama))
				{
					EyLog.Error("Paket Indir", Convert.ToString(eyIndirSonuc.durum), eyIndirSonuc.hataaciklama);
					return EyPaketIndirSonuc.GetEyIndirSonuc(eyIndirSonuc);
				}
				if (Save)
				{
					base64Binary[] ePaket = eyIndirSonuc.ePaket;
					if (ePaket != null)
					{
						base64Binary[] array = ePaket;
						foreach (base64Binary base64Binary in array)
						{
							string text2 = "";
							text2 = ((base64Binary.fileName == null) ? ("Mesaj-" + MimeUtilities.GetCurrentMiliSecond() + ".eml") : ((!(Path.GetExtension(base64Binary.fileName) == "")) ? ("Mesaj-" + MimeUtilities.GetCurrentMiliSecond() + Path.GetExtension(base64Binary.fileName)) : ("Mesaj-" + MimeUtilities.GetCurrentMiliSecond() + ".eml")));
							try
							{
								string savePath = Path.Combine(mesajKayitAdresi, text2);
								FileUtils.SaveFile(base64Binary.Value, savePath);
							}
							catch (Exception ex)
							{
								LOG.Error("Kep Iletisi Diske Kaydedilemedi. Hata : " + ex.Message, ex);
								EyLog.Log("Paket Indir", EyLogTuru.UYARI, "Paket Dosyası Diske Yazılırken Hata Meydana Geldi.");
								EyLog.Log("Paket Indir", EyLogTuru.BILGI, "Paket'e PaketIndir Fonksiyonundan Geri Dönen EyPaketIndirSonuc Altındaki EYazismaPaketi Properties'inden Erişebilirsiniz.");
								return EyPaketIndirSonuc.GetEyIndirSonuc(eyIndirSonuc);
							}
							EyLog.Log("Paket Indir", EyLogTuru.BILGI, "Sonuç: Başarılı", "Paket, " + Path.Combine(mesajKayitAdresi, text2) + " kaydedildi.");
						}
					}
				}
				return EyPaketIndirSonuc.GetEyIndirSonuc(eyIndirSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
