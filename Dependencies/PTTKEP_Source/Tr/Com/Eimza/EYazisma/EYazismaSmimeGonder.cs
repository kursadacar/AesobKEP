using System;
using System.IO;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaSmimeGonder
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyDurumSonuc SmimeGonder(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string imzaDosyasininAdresi, string mesajId)
		{
			if (string.IsNullOrEmpty(imzaDosyasininAdresi))
			{
				string text = "Imza Dosyasının Adresi Boş Bırakılamaz.";
				EyLog.Log("Smime Gönder", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			if (!FileUtils.IsFilePathValid(imzaDosyasininAdresi))
			{
				string text2 = "Yüklenecek Dosya Adresini Kontrol Ediniz. Adres : " + imzaDosyasininAdresi;
				EyLog.Log("Smime Gönder", EyLogTuru.HATA, text2);
				throw new EYazismaException(text2);
			}
			base64Binary base64Binary = new base64Binary();
			FileStream fileStream = null;
			byte[] array;
			try
			{
				fileStream = new FileStream(imzaDosyasininAdresi, FileMode.Open, FileAccess.Read);
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
			}
			catch (Exception exception)
			{
				string message = "Imza Dosyası Diskten Okunamadı.";
				LOG.Error(message, exception);
				throw new EYazismaException(message);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Flush();
					fileStream.Close();
				}
			}
			try
			{
				eySmimeGonder eySmimeGonder = new eySmimeGonder();
				base64Binary.Value = array;
				base64Binary.fileName = Path.GetFileName(imzaDosyasininAdresi);
				base64Binary.contentType = "application/octet-stream";
				eySmimeGonder.kepHesap = kepHesap;
				eySmimeGonder.mesajid = mesajId;
				eySmimeGonder.imza = base64Binary;
				eyDurumSonuc eyDurumSonuc;
				try
				{
					eyDurumSonuc = client.SmimeGonder(eySmimeGonder);
				}
				catch (Exception exception2)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception2);
					return null;
				}
				if (!eyDurumSonuc.durum.HasValue)
				{
					EyLog.Log("Smime Gönder", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
				}
				if (eyDurumSonuc.durum.Value == 0)
				{
					EyLog.Log("Smime Gönder", EyLogTuru.BILGI, "Durum: " + eyDurumSonuc.durum.Value, "Sonuc: Başarılı.");
				}
				else
				{
					EyLog.Error("Smime Gönder", eyDurumSonuc.durum.Value.ToString(), eyDurumSonuc.hataaciklama);
				}
				return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
			}
			catch (Exception exception3)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception3);
				return null;
			}
		}

		internal static EyDurumSonuc SmimeGonder(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, byte[] bFile, string mesajId)
		{
			if (bFile == null || bFile.Length == 0)
			{
				string text = "Imza Dosyasının Değeri Boş Bırakılamaz.";
				EyLog.Log("Smime Gönder", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			try
			{
				eySmimeGonder eySmimeGonder = new eySmimeGonder();
				base64Binary base64Binary = new base64Binary();
				base64Binary.Value = bFile;
				base64Binary.fileName = "SmimePaketi-" + MimeUtilities.GetCurrentMiliSecond() + ".p7s";
				base64Binary.contentType = "application/octet-stream";
				eySmimeGonder.kepHesap = kepHesap;
				eySmimeGonder.mesajid = mesajId;
				eySmimeGonder.imza = base64Binary;
				eyDurumSonuc eyDurumSonuc;
				try
				{
					eyDurumSonuc = client.SmimeGonder(eySmimeGonder);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (!eyDurumSonuc.durum.HasValue)
				{
					EyLog.Log("Smime Gönder", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyDurumSonuc.GetEyDurumSonuc(eyDurumSonuc);
				}
				if (eyDurumSonuc.durum.Value == 0)
				{
					EyLog.Log("Smime Gönder", EyLogTuru.BILGI, "Durum: " + eyDurumSonuc.durum.Value, "Sonuc: Başarılı.");
				}
				else
				{
					EyLog.Error("Smime Gönder", eyDurumSonuc.durum.Value.ToString(), eyDurumSonuc.hataaciklama);
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
