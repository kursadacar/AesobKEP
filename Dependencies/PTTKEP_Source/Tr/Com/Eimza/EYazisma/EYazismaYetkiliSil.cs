using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaYetkiliSil
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyDurumSonuc YetkiliSil(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string tcno, string Adi, string Soyadi)
		{
			if (tcno == null || Adi == null || Soyadi == null)
			{
				string text = "Yetkili Silmek için Eksik Parametre. Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.";
				EyLog.Log("Yetkili Sil", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eyYetkiliSil eyYetkiliSil = new eyYetkiliSil();
				eyYetkiliSil.kepHesap = kepHesap;
				eyYetkiliSil.kepYetkiliAdi = Adi;
				eyYetkiliSil.kepYetkiliSoyadi = Soyadi;
				eyYetkiliSil.kepYetkiliTCNO = tcno;
				eyDurumSonuc eyDurumSonuc;
				try
				{
					eyDurumSonuc = client.YetkiliSil(eyYetkiliSil);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (!eyDurumSonuc.durum.HasValue)
				{
					EyLog.Log("Yetkili Sil", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
				}
				if (eyDurumSonuc.durum.Value == 0)
				{
					EyLog.Log("Yetkili Sil", EyLogTuru.BILGI, "Durum: " + eyDurumSonuc.durum.Value, "Sonuc: Başarılı.");
				}
				else
				{
					EyLog.Error("Yetkili Sil", eyDurumSonuc.durum.Value.ToString(), eyDurumSonuc.hataaciklama);
				}
				return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
