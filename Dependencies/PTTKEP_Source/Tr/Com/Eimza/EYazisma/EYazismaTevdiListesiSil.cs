using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaTevdiListesiSil
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyDurumSonuc TevdiListesiSil(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string tevdiListeNo)
		{
			if (string.IsNullOrEmpty(tevdiListeNo))
			{
				string text = "Tevdi Liste No Boş Bırakılamaz.";
				EyLog.Log("Tevdi Listesi Sil", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eyTevdiListesiSil eyTevdiListesiSil = new eyTevdiListesiSil();
				eyTevdiListesiSil.kepHesap = kepHesap;
				eyTevdiListesiSil.TevdiListeNo = tevdiListeNo;
				eyDurumSonuc eyDurumSonuc;
				try
				{
					eyDurumSonuc = client.TevdiListesiSil(eyTevdiListesiSil);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (!eyDurumSonuc.durum.HasValue)
				{
					EyLog.Log("Tevdi Listesi Sil", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
				}
				if (eyDurumSonuc.durum == 0)
				{
					EyLog.Log("Tevdi Listesi Sil", EyLogTuru.BILGI, "Durum: " + eyDurumSonuc.durum, "Sonuç: Başarılı.");
				}
				else
				{
					EyLog.Error("Tevdi Listesi Sil", eyDurumSonuc.durum.ToString(), eyDurumSonuc.hataaciklama);
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
