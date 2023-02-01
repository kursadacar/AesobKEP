using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaPaketSil
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyDurumSonuc PaketSil(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string kepId, string dizin)
		{
			if (string.IsNullOrEmpty(kepId))
			{
				string text = "KepID Parametresini Boş Bırakamazsınız.";
				EyLog.Log("Paket Sil", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eyPaketKepId eyPaketKepId = new eyPaketKepId();
				eyPaketKepId.kepHesap = kepHesap;
				eyPaketKepId.kepId = kepId;
				eyPaketKepId.kepSiraNoSpecified = false;
				eyPaketKepId.dizin = dizin;
				eyDurumSonuc eyDurumSonuc;
				try
				{
					eyDurumSonuc = client.PaketSil(eyPaketKepId);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (!eyDurumSonuc.durum.HasValue)
				{
					EyLog.Log("Paket Sil", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
				}
				if (eyDurumSonuc.durum.Value == 0)
				{
					EyLog.Log("Paket Sil", EyLogTuru.BILGI, "Durum: " + eyDurumSonuc.durum.Value, "Sonuç : Başarılı.");
				}
				else
				{
					EyLog.Error("Paket Sil", eyDurumSonuc.durum.Value.ToString(), eyDurumSonuc.hataaciklama);
				}
				return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}

		internal static EyDurumSonuc PaketSil(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, int msgSiraNo, string dizin)
		{
			try
			{
				eyPaketKepId eyPaketKepId = new eyPaketKepId();
				eyPaketKepId.kepHesap = kepHesap;
				eyPaketKepId.kepSiraNo = msgSiraNo;
				eyPaketKepId.kepSiraNoSpecified = true;
				eyPaketKepId.dizin = dizin;
				eyDurumSonuc eyDurumSonuc;
				try
				{
					eyDurumSonuc = client.PaketSil(eyPaketKepId);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (!eyDurumSonuc.durum.HasValue)
				{
					EyLog.Log("Paket Sil", EyLogTuru.HATA, " Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
				}
				if (eyDurumSonuc.durum.Value == 0)
				{
					EyLog.Log("Paket Sil", EyLogTuru.BILGI, "Durum: " + eyDurumSonuc.durum.Value, "Sonuç : Başarılı.");
				}
				else
				{
					EyLog.Error("Paket Sil", eyDurumSonuc.durum.Value.ToString(), eyDurumSonuc.hataaciklama);
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
