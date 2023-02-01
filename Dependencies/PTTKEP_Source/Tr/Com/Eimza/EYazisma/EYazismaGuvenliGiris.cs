using System;
using System.IO;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaGuvenliGiris
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyGuvenliGirisSonuc GuvenliGiris(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string smskey, string eGuvenlikId)
		{
			if (string.IsNullOrEmpty(smskey))
			{
				string text = "Sms Key Boş Bırakılamaz!";
				EyLog.Log("Güvenli Giriş", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eyGuvenliGiris eyGuvenliGiris = new eyGuvenliGiris();
				eyGuvenliGiris.kepHesap = kepHesap;
				eyGuvenliGiris.girisTur = eyGirisTur.OTP;
				eyGuvenliGiris.girisTurSpecified = true;
				eyGuvenliGiris.smsKey = smskey;
				eyGuvenliGiris.eGuvenlikId = eGuvenlikId;
				eyGuvenliGirisSonuc eyGuvenliGirisSonuc;
				try
				{
					eyGuvenliGirisSonuc = client.GuvenliGiris(eyGuvenliGiris);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (string.IsNullOrEmpty(eyGuvenliGirisSonuc.durum))
				{
					EyLog.Log("Güvenli Giriş", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyGuvenliGirisSonuc.GetEyGuvenliGirisSonuc(eyGuvenliGirisSonuc);
				}
				if (eyGuvenliGirisSonuc.durum.Equals("1"))
				{
					EyLog.Error("Güvenli Giriş", eyGuvenliGirisSonuc.durum, eyGuvenliGirisSonuc.hataaciklama);
					return EyGuvenliGirisSonuc.GetEyGuvenliGirisSonuc(eyGuvenliGirisSonuc);
				}
				EyLog.Log("Güvenli Giriş", EyLogTuru.BILGI, "Durum: " + eyGuvenliGirisSonuc.durum, " Güvenlik ID: " + eyGuvenliGirisSonuc.eGuvenlikId);
				return EyGuvenliGirisSonuc.GetEyGuvenliGirisSonuc(eyGuvenliGirisSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}

		internal static EyGuvenliGirisSonuc GuvenliEGiris(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string eGuvenlikId, string P7sPath)
		{
			if (string.IsNullOrEmpty(P7sPath))
			{
				string text = "Imza Adresi Boş Bırakılamaz!";
				EyLog.Log("Güvenli Giriş", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			if (!FileUtils.IsFilePathValid(P7sPath))
			{
				string text2 = "Yüklenecek Dosya Adresini Kontrol Ediniz. Adres : " + P7sPath;
				EyLog.Log("Güvenli Giriş", EyLogTuru.HATA, text2);
				throw new EYazismaException(text2);
			}
			base64Binary base64Binary = new base64Binary();
			FileStream fileStream = null;
			byte[] array;
			try
			{
				fileStream = new FileStream(P7sPath, FileMode.Open, FileAccess.Read);
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
			}
			catch (Exception exception)
			{
				string text3 = "Güvenli Girişte Kullanılan Imza Dosyası Okunamadı. Dosya Yolunu Kontrol Ediniz. Adres : " + P7sPath;
				LOG.Error(text3, exception);
				EyLog.Log("Güvenli Giriş", EyLogTuru.HATA, text3);
				throw new EYazismaException(text3);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
			base64Binary.Value = array;
			try
			{
				eyGuvenliGiris eyGuvenliGiris = new eyGuvenliGiris();
				eyGuvenliGiris.kepHesap = kepHesap;
				eyGuvenliGiris.eCadesBes = base64Binary;
				eyGuvenliGiris.girisTur = eyGirisTur.EIMZA;
				eyGuvenliGiris.girisTurSpecified = true;
				eyGuvenliGiris.eGuvenlikId = eGuvenlikId;
				eyGuvenliGirisSonuc eyGuvenliGirisSonuc;
				try
				{
					eyGuvenliGirisSonuc = client.GuvenliGiris(eyGuvenliGiris);
				}
				catch (Exception exception2)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception2);
					return null;
				}
				if (string.IsNullOrEmpty(eyGuvenliGirisSonuc.durum))
				{
					EyLog.Log("Güvenli Giriş", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyGuvenliGirisSonuc.GetEyGuvenliGirisSonuc(eyGuvenliGirisSonuc);
				}
				if (eyGuvenliGirisSonuc.durum.Equals("1"))
				{
					EyLog.Error("Güvenli Giriş", eyGuvenliGirisSonuc.durum, eyGuvenliGirisSonuc.hataaciklama);
					return EyGuvenliGirisSonuc.GetEyGuvenliGirisSonuc(eyGuvenliGirisSonuc);
				}
				EyLog.Log("Güvenli Giriş", EyLogTuru.BILGI, "Durum: " + eyGuvenliGirisSonuc.durum, " Güvenlik ID: " + eyGuvenliGirisSonuc.eGuvenlikId);
				return EyGuvenliGirisSonuc.GetEyGuvenliGirisSonuc(eyGuvenliGirisSonuc);
			}
			catch (Exception exception3)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception3);
				return null;
			}
		}

		internal static EyGuvenliGirisSonuc GuvenliEGiris(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string eGuvenlikId, byte[] P7sImza)
		{
			if (P7sImza == null || P7sImza.Length == 0)
			{
				string text = "Imza Değeri Boş Bırakılamaz!";
				EyLog.Log("Güvenli Giriş", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eyGuvenliGiris eyGuvenliGiris = new eyGuvenliGiris();
				eyGuvenliGiris.kepHesap = kepHesap;
				base64Binary base64Binary = new base64Binary();
				base64Binary.Value = P7sImza;
				eyGuvenliGiris.eCadesBes = base64Binary;
				eyGuvenliGiris.girisTur = eyGirisTur.EIMZA;
				eyGuvenliGiris.girisTurSpecified = true;
				eyGuvenliGiris.eGuvenlikId = eGuvenlikId;
				eyGuvenliGirisSonuc eyGuvenliGirisSonuc;
				try
				{
					eyGuvenliGirisSonuc = client.GuvenliGiris(eyGuvenliGiris);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (string.IsNullOrEmpty(eyGuvenliGirisSonuc.durum))
				{
					EyLog.Log("Güvenli Giriş", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyGuvenliGirisSonuc.GetEyGuvenliGirisSonuc(eyGuvenliGirisSonuc);
				}
				if (eyGuvenliGirisSonuc.durum.Equals("1"))
				{
					EyLog.Error("Güvenli Giriş", eyGuvenliGirisSonuc.durum, eyGuvenliGirisSonuc.hataaciklama);
					return EyGuvenliGirisSonuc.GetEyGuvenliGirisSonuc(eyGuvenliGirisSonuc);
				}
				EyLog.Log("Güvenli Giriş", EyLogTuru.BILGI, "Durum: " + eyGuvenliGirisSonuc.durum, " Güvenlik ID: " + eyGuvenliGirisSonuc.eGuvenlikId);
				return EyGuvenliGirisSonuc.GetEyGuvenliGirisSonuc(eyGuvenliGirisSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
