using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.Config;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaMimeYap
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyMimeYapSonuc MimeYap(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, List<string> eklerAdresi, List<string> kime, string icerik, string konu, string paketId, EYazismaOzetAlg ozetAlg, EYazismaIcerikTur icerikTur, EYazismaPaketTur paketTur, EYazismaApiConfig config)
		{
			if (kime == null || kime.Count == 0)
			{
				string text = "Kime Alanı Boş Bırakılamaz.";
				EyLog.Log("Mime Yap", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			List<base64Binary> list = new List<base64Binary>();
			if (eklerAdresi != null)
			{
				foreach (string item in eklerAdresi)
				{
					base64Binary base64Binary = new base64Binary();
					FileStream fileStream = null;
					byte[] array;
					try
					{
						fileStream = new FileStream(item, FileMode.Open, FileAccess.Read);
						array = new byte[fileStream.Length];
						fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
					}
					catch (Exception exception)
					{
						string text2 = "Verdiğiniz Eklerin Adreslerini Kontrol Ediniz.";
						EyLog.Log("Mime Yap", EyLogTuru.HATA, text2);
						LOG.Error(text2, exception);
						throw new EYazismaException(text2);
					}
					finally
					{
						if (fileStream != null)
						{
							fileStream.Close();
						}
					}
					base64Binary.Value = array;
					base64Binary.fileName = Path.GetFileName(item);
					base64Binary.contentType = "application/octet-stream";
					list.Add(base64Binary);
				}
			}
			long num = 0L;
			if (list.Count > 0)
			{
				foreach (base64Binary item2 in list)
				{
					num += item2.Value.LongLength;
				}
				float num2 = num;
				float num3 = 0f;
				switch (config.MaxMailDosyaBoyutu.Cins)
				{
				case Boyut.Suffix.KB:
					num3 = config.MaxMailDosyaBoyutu.Size * 1024f;
					break;
				case Boyut.Suffix.MB:
					num3 = config.MaxMailDosyaBoyutu.Size * 1024f * 1024f;
					break;
				}
				num3 = num3 * 4f / 3f;
				num3 += 285004f / (float)Math.E;
				if (num2 > num3)
				{
					string text3 = "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.";
					EyLog.Log("Mime Yap", EyLogTuru.HATA, text3);
					throw new EYazismaException(text3);
				}
			}
			try
			{
				eyMimeYap eyMimeYap = new eyMimeYap();
				eyMimeYap.eIcerikTur = (eyIcerikTur)icerikTur;
				eyMimeYap.icerik = icerik;
				eyMimeYap.konu = konu;
				string text4 = string.Empty;
				for (int i = 0; i <= kime.Count - 1; i++)
				{
					text4 += kime[i];
					if (i == kime.Count - 1)
					{
						break;
					}
					text4 += ",";
				}
				eyMimeYap.kime = text4;
				eyMimeYap.ePaketTur = (eyPaketTur)paketTur;
				eyMimeYap.ePaketId = paketId;
				eyMimeYap.eOzetAlg = (eyOzetAlg)ozetAlg;
				eyMimeYap.eIcerikTurSpecified = true;
				eyMimeYap.eOzetAlgSpecified = true;
				eyMimeYap.ePaketTurSpecified = true;
				eyMimeYap.kepHesap = kepHesap;
				eyMimeYap.ekler = list.ToArray();
				eyMimeYapSonuc eyMimeYapSonuc;
				try
				{
					eyMimeYapSonuc = client.MimeYap(eyMimeYap);
				}
				catch (Exception exception2)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception2);
					return null;
				}
				if (eyMimeYapSonuc.durum == null)
				{
					EyLog.Log("Mime Yap", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyMimeYapSonuc.GetEyMimeYapSonuc(eyMimeYapSonuc);
				}
				if (eyMimeYapSonuc.durum.Equals("0"))
				{
					EyLog.Log("Mime Yap", EyLogTuru.BILGI, "Durum: " + eyMimeYapSonuc.durum, "Sonuç: Başarılı.");
				}
				else
				{
					EyLog.Error("Mime Yap", eyMimeYapSonuc.durum.ToString(), eyMimeYapSonuc.hataaciklama);
				}
				return EyMimeYapSonuc.GetEyMimeYapSonuc(eyMimeYapSonuc);
			}
			catch (Exception exception3)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception3);
				return null;
			}
		}

		internal static EyMimeYapSonuc MimeYap(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, List<Ek> eklerAdresi, List<string> kime, string icerik, string konu, string paketId, EYazismaOzetAlg ozetAlg, EYazismaIcerikTur icerikTur, EYazismaPaketTur paketTur, EYazismaApiConfig config)
		{
			if (kime == null || kime.Count == 0)
			{
				string text = "Kime Alanı Boş Bırakılamaz.";
				EyLog.Log("Mime Yap", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			long num = 0L;
			if (eklerAdresi != null && eklerAdresi.Count > 0)
			{
				foreach (Ek item in eklerAdresi)
				{
					num += item.Degeri.LongLength;
				}
				float num2 = num;
				float num3 = 0f;
				switch (config.MaxMailDosyaBoyutu.Cins)
				{
				case Boyut.Suffix.KB:
					num3 = config.MaxMailDosyaBoyutu.Size * 1024f;
					break;
				case Boyut.Suffix.MB:
					num3 = config.MaxMailDosyaBoyutu.Size * 1024f * 1024f;
					break;
				}
				num3 = num3 * 4f / 3f;
				num3 += 285004f / (float)Math.E;
				if (num2 > num3)
				{
					string text2 = "Eklerin Toplam Boyutu " + config.MaxMailDosyaBoyutu.Size + " " + config.MaxMailDosyaBoyutu.Cins.ToString() + " Boyutunu Geçemez.";
					EyLog.Log("Mime Yap", EyLogTuru.HATA, text2);
					throw new EYazismaException(text2);
				}
			}
			try
			{
				eyMimeYap eyMimeYap = new eyMimeYap();
				List<base64Binary> list = new List<base64Binary>();
				if (eklerAdresi != null)
				{
					foreach (Ek item2 in eklerAdresi)
					{
						base64Binary base64Binary = new base64Binary();
						base64Binary.Value = item2.Degeri;
						base64Binary.fileName = item2.Adi;
						base64Binary.contentType = "application/octet-stream";
						list.Add(base64Binary);
					}
				}
				eyMimeYap.eIcerikTur = (eyIcerikTur)icerikTur;
				eyMimeYap.icerik = icerik;
				eyMimeYap.konu = konu;
				string text3 = string.Empty;
				for (int i = 0; i <= kime.Count - 1; i++)
				{
					text3 += kime[i];
					if (i == kime.Count - 1)
					{
						break;
					}
					text3 += ",";
				}
				eyMimeYap.kime = text3;
				eyMimeYap.ePaketTur = (eyPaketTur)paketTur;
				eyMimeYap.ePaketId = paketId;
				eyMimeYap.eOzetAlg = (eyOzetAlg)ozetAlg;
				eyMimeYap.eIcerikTurSpecified = true;
				eyMimeYap.eOzetAlgSpecified = true;
				eyMimeYap.ePaketTurSpecified = true;
				eyMimeYap.kepHesap = kepHesap;
				eyMimeYap.ekler = list.ToArray();
				eyMimeYapSonuc eyMimeYapSonuc;
				try
				{
					eyMimeYapSonuc = client.MimeYap(eyMimeYap);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (eyMimeYapSonuc.durum == null)
				{
					EyLog.Log("Mime Yap", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyMimeYapSonuc.GetEyMimeYapSonuc(eyMimeYapSonuc);
				}
				if (eyMimeYapSonuc.durum.Equals("0"))
				{
					EyLog.Log("Mime Yap", EyLogTuru.BILGI, "Durum: " + eyMimeYapSonuc.durum, "Sonuç: Başarılı.");
				}
				else
				{
					EyLog.Error("Mime Yap", eyMimeYapSonuc.durum.ToString(), eyMimeYapSonuc.hataaciklama);
				}
				return EyMimeYapSonuc.GetEyMimeYapSonuc(eyMimeYapSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
