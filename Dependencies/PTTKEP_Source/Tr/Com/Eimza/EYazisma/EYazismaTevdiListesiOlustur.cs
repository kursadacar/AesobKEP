using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaTevdiListesiOlustur
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyTevdiListesiSonuc TevdiListesiOlustur(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string birimID)
		{
			if (string.IsNullOrEmpty(birimID))
			{
				string text = "BirimID değeri Boş Bırakılamaz.";
				EyLog.Log("Tevdi Listesi Olustur", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eyTevdiListesi eyTevdiListesi = new eyTevdiListesi();
				eyTevdiListesi.kepHesap = kepHesap;
				eyTevdiListesi.BirimId = birimID;
				eyTevdiListesiSonuc eyTevdiListesiSonuc;
				try
				{
					eyTevdiListesiSonuc = client.TevdiListesiOlustur(eyTevdiListesi);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyTevdiListesiSonuc.durum == 0)
				{
					EyLog.Log("Tevdi Listesi Olustur", EyLogTuru.BILGI, "Durum: " + eyTevdiListesiSonuc.durum, "Sonuç: Başarılı.");
				}
				else
				{
					EyLog.Error("Tevdi Listesi Olustur", eyTevdiListesiSonuc.durum.ToString(), eyTevdiListesiSonuc.hataaciklama);
				}
				return EyTevdiListesiSonuc.GetEyTevdiListesiSonuc(eyTevdiListesiSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
