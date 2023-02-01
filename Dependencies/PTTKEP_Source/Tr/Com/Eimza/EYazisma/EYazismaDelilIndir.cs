using System;
using System.IO;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaDelilIndir
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyPaketDelilIndirSonuc DelilIndir(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string delilId, string delilKayitAdresi = null, bool Save = false)
		{
			if (string.IsNullOrEmpty(delilId))
			{
				string text = "DelilID Parametresi Boş Bırakılamaz.";
				EyLog.Log("Delil Indir", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			if (string.IsNullOrEmpty(delilKayitAdresi) && Save)
			{
				string text2 = "Delil Kayıt Adresi Boş Bırakılamaz.";
				EyLog.Log("Delil Indir", EyLogTuru.HATA, text2);
				throw new EYazismaException(text2);
			}
			try
			{
				eyPaketDelilId eyPaketDelilId = new eyPaketDelilId();
				eyPaketDelilId.kepHesap = kepHesap;
				eyPaketDelilId.delilId = delilId;
				eyPaketDelilIndirSonuc eyPaketDelilIndirSonuc;
				try
				{
					eyPaketDelilIndirSonuc = client.PaketDelilIndir(eyPaketDelilId);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyPaketDelilIndirSonuc.eDelil == null)
				{
					EyLog.Error("Delil Indir", eyPaketDelilIndirSonuc.durum, eyPaketDelilIndirSonuc.hataaciklama);
					return EyPaketDelilIndirSonuc.GetEyPaketDelilIndirSonuc(eyPaketDelilIndirSonuc);
				}
				if (!string.IsNullOrEmpty(eyPaketDelilIndirSonuc.hataaciklama))
				{
					EyLog.Error("Delil Indir", Convert.ToString(eyPaketDelilIndirSonuc.durum), eyPaketDelilIndirSonuc.hataaciklama);
				}
				if (Save)
				{
					base64Binary eDelil = eyPaketDelilIndirSonuc.eDelil;
					try
					{
						string path = "Delil-" + delilId + ".xml";
						string savePath = Path.Combine(delilKayitAdresi, path);
						FileUtils.SaveFile(eDelil.Value, savePath);
						EyLog.Log("Delil Indir", EyLogTuru.BILGI, "Durum: " + eyPaketDelilIndirSonuc.durum, "Delil, " + Path.Combine(delilKayitAdresi, path) + " kaydedildi.");
					}
					catch (Exception exception2)
					{
						LOG.Error("Delil Dosyası Diske Kaydedilemedi. Delil'e Delil Indir fonksiyonundan Geri Dönen EYazismaDelilIndirSonuc Altındaki EyazismaDelil Properties'inden Erişebilirsiniz.", exception2);
						EyLog.Log("Delil Indir", EyLogTuru.UYARI, "Delil dosyası diske yazılırken hata meydana geldi.");
						EyLog.Log("Delil Indir", EyLogTuru.BILGI, "Delil'e Delil Indir fonksiyonundan Geri Dönen EYazismaDelilIndirSonuc Altındaki EyazismaDelil Properties'inden Erişebilirsiniz.");
						return EyPaketDelilIndirSonuc.GetEyPaketDelilIndirSonuc(eyPaketDelilIndirSonuc);
					}
				}
				return EyPaketDelilIndirSonuc.GetEyPaketDelilIndirSonuc(eyPaketDelilIndirSonuc);
			}
			catch (Exception exception3)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception3);
				return null;
			}
		}
	}
}
