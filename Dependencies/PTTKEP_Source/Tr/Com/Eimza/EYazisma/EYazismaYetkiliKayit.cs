using System;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaYetkiliKayit
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EyYetkiliKayitSonuc YetkiliKayit(eyServisPortTypeClient client, eyKepHesapGirisP kepHesap, string tcno, string tel, string Adi, string Soyadi, string EPosta, string islemTip = "INSERT")
		{
			if (tcno == null || tel == null || Adi == null || Soyadi == null || EPosta == null)
			{
				string text = "Yetkili Bilgilerinde Eksiklik Var. Fonksiyona Verdiğiniz Parametreleri Kontrol Ediniz.";
				EyLog.Log("Yetkili Kayıt", EyLogTuru.HATA, text);
				throw new EYazismaException(text);
			}
			if (tcno.Length != 11)
			{
				string text2 = "Yetkili TC No Yanlış. TC Numarası 11 Karakter Olmalıdır.";
				EyLog.Log("Yetkili Kayıt", EyLogTuru.HATA, text2);
				throw new EYazismaException(text2);
			}
			try
			{
				eyYetkiliKayit eyYetkiliKayit = new eyYetkiliKayit();
				eyYetkiliKayit.kepHesap = kepHesap;
				eyYetkiliKayit.kepYetkiliTCNO = tcno;
				eyYetkiliKayit.kepYetkiliTel = tel;
				eyYetkiliKayit.kepYetkiliAdi = Adi.ToUpper(Util.TR);
				eyYetkiliKayit.kepYetkiliSoyadi = Soyadi.ToUpper(Util.TR);
				eyYetkiliKayit.kepYetkiliEPosta = EPosta;
				eyYetkiliKayit.islemTip = islemTip;
				eyYetkiliKayitSonuc eyYetkiliKayitSonuc;
				try
				{
					eyYetkiliKayitSonuc = client.YetkiliKayit(eyYetkiliKayit);
				}
				catch (Exception exception)
				{
					LOG.Error("Web Servisten Yanıt Alınamadı.", exception);
					return null;
				}
				if (!eyYetkiliKayitSonuc.durum.HasValue)
				{
					EyLog.Log("Yetkili Kayıt", EyLogTuru.HATA, "Web Servis Cevap Olarak Boş Değer Döndü.");
					return EyYetkiliKayitSonuc.GetEyYetkiliKayitSonuc(eyYetkiliKayitSonuc);
				}
				if (eyYetkiliKayitSonuc.durum.Value == 0)
				{
					EyLog.Log("Yetkili Kayıt", EyLogTuru.BILGI, "Durum: " + eyYetkiliKayitSonuc.durum.Value, "Sonuç: Başarılı.");
				}
				else
				{
					EyLog.Error("Yetkili Kayıt", eyYetkiliKayitSonuc.durum.Value.ToString(), eyYetkiliKayitSonuc.hataaciklama);
				}
				return EyYetkiliKayitSonuc.GetEyYetkiliKayitSonuc(eyYetkiliKayitSonuc);
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}
	}
}
