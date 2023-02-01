using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaAlindiOnay
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyDurumSonuc AlindiOnay(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string kepId, string dizin)
		{
			if (string.IsNullOrEmpty(kepId))
			{
				string text = "KepID Parametresini Boş Bırakamazsınız.";
				EyLog.Log("Alındı Onay", EyLogTuru.HATA, text);
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
					eyDurumSonuc = client.AlindiOnay(eyPaketKepId);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (!eyDurumSonuc.durum.HasValue)
				{
					LOG.Info("Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
				}
				if (eyDurumSonuc.durum.Value == 0)
				{
					EyLog.Log("Alındı Onay", EyLogTuru.BILGI, "Durum: " + eyDurumSonuc.durum.Value, "Sonuç: Başarılı.");
				}
				else
				{
					EyLog.Error("Alındı Onay", eyDurumSonuc.durum.Value, eyDurumSonuc.hataaciklama);
				}
				return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}

		internal static EyDurumSonuc AlindiOnay(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, int msgSiraNo, string dizin)
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
					eyDurumSonuc = client.AlindiOnay(eyPaketKepId);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (!eyDurumSonuc.durum.HasValue)
				{
					LOG.Info("Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
				}
				if (eyDurumSonuc.durum.Value == 0)
				{
					EyLog.Log("Alındı Onay", EyLogTuru.BILGI, "Durum: " + eyDurumSonuc.durum.Value, "Sonuç: Başarılı.");
				}
				else
				{
					EyLog.Error("Alındı Onay", eyDurumSonuc.durum.Value, eyDurumSonuc.hataaciklama);
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
