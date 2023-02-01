using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaKotaSorgula
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyKotaSonuc KotaSorgula(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap)
		{
			EyKotaSonuc eyKotaSonuc = new EyKotaSonuc();
			try
			{
				int? kota;
				try
				{
					kota = client.KotaSorgula(kepHesap);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (!kota.HasValue)
				{
					EyLog.Log("Kota Sorgula", EyLogTuru.HATA, "Web Servisten Kota Bilgisi Boş Döndü.");
					eyKotaSonuc.Durum = 1;
					eyKotaSonuc.HataAciklama = "Web Servisten Kota Bilgisi Boş Döndü.";
					eyKotaSonuc.Cins = "";
					return eyKotaSonuc;
				}
				EyLog.Log("Kota Sorgula", EyLogTuru.BILGI, "Kota : " + kota.Value + " KB");
				eyKotaSonuc.Durum = 0;
				eyKotaSonuc.HataAciklama = "Kota Sorgulama Başarılı";
				eyKotaSonuc.Kota = kota;
				eyKotaSonuc.Cins = "KB";
				return eyKotaSonuc;
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
