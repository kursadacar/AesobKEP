using System;
using System.IO;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaYukle
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyYukleSonuc Yukle(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string yuklenecekDosyaAdresi, EYazismaPaketTur? paketTuru)
		{
			if (string.IsNullOrEmpty(yuklenecekDosyaAdresi))
			{
				string text = "Yüklenecek Dosya Adresi Boş Bırakılamaz.";
				EyLog.Log("Yükle", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			if (!FileUtils.IsFilePathValid(yuklenecekDosyaAdresi))
			{
				string text2 = "Yüklenecek Dosya Adresini Kontrol Ediniz. Adres : " + yuklenecekDosyaAdresi;
				EyLog.Log("Yükle", EyLogTuru.HATA, text2);
				throw new EYazismaException(text2);
			}
			byte[] array = null;
			try
			{
				FileStream fileStream = new FileStream(yuklenecekDosyaAdresi, FileMode.Open, FileAccess.Read);
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
				fileStream.Close();
			}
			catch (Exception exception)
			{
				LOG.Error("Gönderilecek S/MIME Paketi Okunamadı. Adresin Doğru Olduğundan Emin Olunuz.", exception);
				throw new EYazismaException("Gönderilecek S/MIME Paketi Okunamadı. Adresin Doğru Olduğundan Emin Olunuz.");
			}
			return Yukle(client, kepHesap, array, paketTuru);
		}

		internal static EyYukleSonuc Yukle(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, byte[] yuklenecekDosya, EYazismaPaketTur? paketTuru)
		{
			if (yuklenecekDosya == null || yuklenecekDosya.Length == 0)
			{
				string text = "Yüklenecek Dosya Değeri Boş Bırakılamaz.";
				EyLog.Log("Yükle", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eyYukle eyYukle = new eyYukle();
				eyYukle.kepHesap = kepHesap;
				base64Binary base64Binary = new base64Binary();
				base64Binary.Value = yuklenecekDosya;
				base64Binary.contentType = "application/octet-stream";
				eyYukle.ePaket = base64Binary;
				if (paketTuru.HasValue)
				{
					eyYukle.ePaketTur = (eyPaketTur)paketTuru.Value;
					eyYukle.ePaketTurSpecified = true;
				}
				else
				{
					eyYukle.ePaketTurSpecified = false;
				}
				eyYukleSonuc eyYukleSonuc;
				try
				{
					eyYukleSonuc = client.Yukle(eyYukle);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyYukleSonuc.durum == 0)
				{
					EyLog.Log("Yükle", EyLogTuru.BILGI, "Durum: " + eyYukleSonuc.durum, "Sonuç: Başarılı.");
				}
				else
				{
					EyLog.Error("Yükle", eyYukleSonuc.durum.ToString(), eyYukleSonuc.hataaciklama);
				}
				return EyYukleSonuc.GetEyYukleSonuc(eyYukleSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
