using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaPaketSorgula
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyPaketSonuc PaketSorgula(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, DateTime? ilkTarih, DateTime? sonTarih, string dizin)
		{
			try
			{
				eyPaketSorgula eyPaketSorgula = new eyPaketSorgula();
				eyPaketSorgula.kepHesap = kepHesap;
				eyPaketSorgula.dizin = dizin;
				if (ilkTarih.HasValue)
				{
					eyPaketSorgula.ilktarih = ilkTarih.Value;
					eyPaketSorgula.ilktarihSpecified = true;
				}
				else
				{
					eyPaketSorgula.ilktarihSpecified = false;
				}
				if (sonTarih.HasValue)
				{
					eyPaketSorgula.sontarih = sonTarih.Value;
					eyPaketSorgula.sontarihSpecified = true;
				}
				else
				{
					eyPaketSorgula.sontarihSpecified = false;
				}
				eyPaketSonuc eyPaketSonuc;
				try
				{
					eyPaketSonuc = client.PaketSorgula(eyPaketSorgula);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyPaketSonuc.kepId == null && eyPaketSonuc.durum == null)
				{
					EyLog.Log("Paket Sorgula", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü. Kep Hesabınızı ve Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.");
					return EyPaketSonuc.GetEyPaketSonuc(eyPaketSonuc);
				}
				if (eyPaketSonuc.kepId == null && eyPaketSonuc.durum[0].HasValue)
				{
					EyLog.Error("Paket Sorgula", Convert.ToString(eyPaketSonuc.durum[0]), eyPaketSonuc.hataaciklama[0]);
					return EyPaketSonuc.GetEyPaketSonuc(eyPaketSonuc);
				}
				if (eyPaketSonuc.kepId != null && eyPaketSonuc.kepId[0] == null)
				{
					if (eyPaketSonuc.durum[0].HasValue)
					{
						EyLog.Error("Paket Sorgula", eyPaketSonuc.durum[0].Value.ToString(), "Verilen Tarihler Arasında Paket Bulunamadı.");
					}
					return EyPaketSonuc.GetEyPaketSonuc(eyPaketSonuc);
				}
				return EyPaketSonuc.GetEyPaketSonuc(eyPaketSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
