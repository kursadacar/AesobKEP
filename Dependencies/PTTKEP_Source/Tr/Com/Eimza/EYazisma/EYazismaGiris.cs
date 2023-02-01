using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaGiris
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyGirisSonuc Giris(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, EYazismaGirisTur tur)
		{
			try
			{
				eyGiris eyGiris = new eyGiris();
				eyGiris.kepHesap = kepHesap;
				eyGiris.girisTur = (eyGirisTur)tur;
				eyGiris.girisTurSpecified = true;
				eyGirisSonuc eyGirisSonuc;
				try
				{
					eyGirisSonuc = client.Giris(eyGiris);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (string.IsNullOrEmpty(eyGirisSonuc.durum))
				{
					EyLog.Log("Giriş", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyGirisSonuc.GetEyGirisSonuc(eyGirisSonuc);
				}
				if (eyGirisSonuc.durum.Equals("1"))
				{
					EyLog.Error("Giriş", eyGirisSonuc.durum, eyGirisSonuc.hataaciklama);
					return EyGirisSonuc.GetEyGirisSonuc(eyGirisSonuc);
				}
				EyLog.Log("Giriş", EyLogTuru.BILGI, "Durum: " + eyGirisSonuc.durum, "Guvenlik ID: " + eyGirisSonuc.eGuvenlikId, "Giriş Hash: " + eyGirisSonuc.eHash, "Giriş Metin: " + eyGirisSonuc.eMetin);
				return EyGirisSonuc.GetEyGirisSonuc(eyGirisSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
