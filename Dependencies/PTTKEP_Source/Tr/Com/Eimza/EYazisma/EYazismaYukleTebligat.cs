using System;
using System.IO;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaYukleTebligat
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyYukleTebligatSonuc YukleTebligat(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string yuklenecekDosyaAdresi, string barkod, string birimID, string birimAdi, bool donuslu)
		{
			if (string.IsNullOrEmpty(yuklenecekDosyaAdresi))
			{
				string text = "Yüklenecek Dosya Adresi Boş Bırakılamaz.";
				EyLog.Log("Yükle Tebligat", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			if (!FileUtils.IsFilePathValid(yuklenecekDosyaAdresi))
			{
				string text2 = "Yüklenecek Dosya Adresini Kontrol Ediniz. Adres : " + yuklenecekDosyaAdresi;
				EyLog.Log("Yükle Tebligat", EyLogTuru.HATA, text2);
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
			return YukleTebligat(client, kepHesap, array, barkod, birimID, birimAdi, donuslu);
		}

		internal static EyYukleTebligatSonuc YukleTebligat(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, byte[] yuklenecekDosya, string barkod, string birimID, string birimAdi, bool donuslu)
		{
			if (yuklenecekDosya == null || yuklenecekDosya.Length == 0)
			{
				string text = "Yüklenecek Dosya Değeri Boş Bırakılamaz.";
				EyLog.Log("Yükle Tebligat", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eyYukleTebligat eyYukleTebligat = new eyYukleTebligat();
				base64Binary base64Binary = new base64Binary();
				base64Binary.Value = yuklenecekDosya;
				base64Binary.contentType = "application/octet-stream";
				eyYukleTebligat.ePaket = base64Binary;
				eyYukleTebligat.Barkod = barkod;
				eyYukleTebligat.BirimAdi = birimAdi;
				eyYukleTebligat.BirimId = birimID;
				eyYukleTebligat.Donuslu = donuslu;
				eyYukleTebligat.DonusluSpecified = true;
				eyYukleTebligat.kepHesap = kepHesap;
				eyYukleTebligatSonuc eyYukleTebligatSonuc;
				try
				{
					eyYukleTebligatSonuc = client.YukleTebligat(eyYukleTebligat);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyYukleTebligatSonuc.durum == 0)
				{
					EyLog.Log("Yükle Tebligat", EyLogTuru.BILGI, "Durum: " + eyYukleTebligatSonuc.durum, "Sonuç: Başarılı.");
				}
				else
				{
					EyLog.Error("Yükle Tebligat", eyYukleTebligatSonuc.durum.ToString(), eyYukleTebligatSonuc.hataaciklama);
				}
				return EyYukleTebligatSonuc.GetEyYukleTebligatSonuc(eyYukleTebligatSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
