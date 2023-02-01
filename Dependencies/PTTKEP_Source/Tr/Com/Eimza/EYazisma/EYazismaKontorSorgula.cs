using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaKontorSorgula
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyKontorSonuc KontorSorgula(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap)
		{
			try
			{
				kontorSonuc kontorSonuc;
				try
				{
					kontorSonuc = client.KontorSorgula(kepHesap);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (kontorSonuc.cins.Contains("Şifre veya Parola Yanlış"))
				{
					EyLog.Log("Kontor Sorgula", EyLogTuru.HATA, "Kep Hesabının Şifre veya Parola Yanlış.");
					return EyKontorSonuc.GetKontorSonuc(kontorSonuc);
				}
				if (kontorSonuc.cins == null)
				{
					EyLog.Log("Kontor Sorgula", EyLogTuru.HATA, "Web Servis Kontor Miktarının Cinsi Null Döndü..");
					return EyKontorSonuc.GetKontorSonuc(kontorSonuc);
				}
				EyLog.Log("Kontor Sorgula", EyLogTuru.BILGI, "Miktar : " + kontorSonuc.miktar + " " + kontorSonuc.cins);
				return EyKontorSonuc.GetKontorSonuc(kontorSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
