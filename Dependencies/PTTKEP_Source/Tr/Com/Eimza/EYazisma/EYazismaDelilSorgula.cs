using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaDelilSorgula
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyPaketDelilSonuc DelilSorgula(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string kepMesajId)
		{
			if (string.IsNullOrEmpty(kepMesajId))
			{
				string text = "KepID Parametresini Boş Bırakamazsınız.";
				EyLog.Log("Delil Sorgula", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eyPaketKepId eyPaketKepId = new eyPaketKepId();
				eyPaketKepId.kepHesap = kepHesap;
				eyPaketKepId.kepId = kepMesajId;
				eyPaketKepId.kepSiraNoSpecified = false;
				eyPaketDelilSonuc eyPaketDelilSonuc;
				try
				{
					eyPaketDelilSonuc = client.PaketDelilSorgula(eyPaketKepId);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyPaketDelilSonuc.delilId == null && eyPaketDelilSonuc.durum == null)
				{
					string text2 = "Web Servis Cevap Olarak Boş Değer Döndü. Kep Hesabınızı ve Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.";
					LOG.Info(text2);
					EyLog.Log("Delil Sorgula", EyLogTuru.HATA, text2);
					return EyPaketDelilSonuc.GetEyPaketDelilSonuc(eyPaketDelilSonuc);
				}
				if (eyPaketDelilSonuc.durum != null && eyPaketDelilSonuc.delilId == null)
				{
					EyLog.Error("Delil Sorgula", Convert.ToString(eyPaketDelilSonuc.durum[0]), eyPaketDelilSonuc.hataaciklama[0]);
					return EyPaketDelilSonuc.GetEyPaketDelilSonuc(eyPaketDelilSonuc);
				}
				return EyPaketDelilSonuc.GetEyPaketDelilSonuc(eyPaketDelilSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}

		internal static EyPaketDelilSonuc DelilSorgula(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, int kepSiraNo)
		{
			try
			{
				eyPaketKepId eyPaketKepId = new eyPaketKepId();
				eyPaketKepId.kepHesap = kepHesap;
				eyPaketKepId.kepSiraNo = kepSiraNo;
				eyPaketKepId.kepSiraNoSpecified = true;
				eyPaketDelilSonuc eyPaketDelilSonuc;
				try
				{
					eyPaketDelilSonuc = client.PaketDelilSorgula(eyPaketKepId);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyPaketDelilSonuc.delilId == null && eyPaketDelilSonuc.durum == null)
				{
					string text = "Web Servis Cevap Olarak Boş Değer Döndü. Kep Hesabınızı ve Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.";
					LOG.Info(text);
					EyLog.Log("Delil Sorgula", EyLogTuru.HATA, text);
					return EyPaketDelilSonuc.GetEyPaketDelilSonuc(eyPaketDelilSonuc);
				}
				if (eyPaketDelilSonuc.durum != null && eyPaketDelilSonuc.delilId == null)
				{
					EyLog.Error("Delil Sorgula", Convert.ToString(eyPaketDelilSonuc.durum[0]), eyPaketDelilSonuc.hataaciklama[0]);
					return EyPaketDelilSonuc.GetEyPaketDelilSonuc(eyPaketDelilSonuc);
				}
				return EyPaketDelilSonuc.GetEyPaketDelilSonuc(eyPaketDelilSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
