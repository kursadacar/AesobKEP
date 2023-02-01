using System;
using System.Collections.Generic;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.Config;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	public class SharedApi : IDisposable
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly EYazismaApiConfig Config;

		private readonly eyKepHesapGirisP KepHesabi;

		private readonly eyServisPortTypeClient WebServiceClient;

		public SharedApi(string KepHesapAdi, string TcNo, string Parola, string Sifre, EYazismaApiConfig Ayarlar)
		{
			KepHesabi = new eyKepHesapGirisP();
			KepHesabi.kepHesap = KepHesapAdi;
			KepHesabi.tcno = TcNo;
			KepHesabi.parola = Parola;
			KepHesabi.sifre = Sifre;
			Config = Ayarlar;
			WebServiceClient = Config.EyServisPortClient;
		}

		~SharedApi()
		{
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		public EyDurumSonuc AlindiOnay(string KepMesajId, string dizin)
		{
			EyLog.Log("Alındı Onay", "Başladı", KepHesabi.kepHesap);
			EyDurumSonuc result = EYazismaAlindiOnay.AlindiOnay(WebServiceClient, KepHesabi, KepMesajId, dizin);
			EyLog.Log("Alındı Onay", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyDurumSonuc AlindiOnay(int MesajSiraNo, string dizin)
		{
			EyLog.Log("Alındı Onay", "Başladı", KepHesabi.kepHesap);
			EyDurumSonuc result = EYazismaAlindiOnay.AlindiOnay(WebServiceClient, KepHesabi, MesajSiraNo, dizin);
			EyLog.Log("Alındı Onay", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyPaketDelilIndirSonuc DelilIndir(string DelilID, string DelilKayitAdresi)
		{
			EyLog.Log("Delil Indir", "Başladı", KepHesabi.kepHesap);
			EyPaketDelilIndirSonuc result = EYazismaDelilIndir.DelilIndir(WebServiceClient, KepHesabi, DelilID, DelilKayitAdresi, true);
			EyLog.Log("Delil Indir", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyPaketDelilIndirSonuc DelilIndir(string DelilID)
		{
			EyLog.Log("Delil Indir", "Başladı", KepHesabi.kepHesap);
			EyPaketDelilIndirSonuc result = EYazismaDelilIndir.DelilIndir(WebServiceClient, KepHesabi, DelilID);
			EyLog.Log("Delil Indir", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyPaketDelilSonuc DelilSorgula(string KepMesajId)
		{
			EyLog.Log("Delil Sorgula", "Başladı", KepHesabi.kepHesap);
			EyPaketDelilSonuc result = EYazismaDelilSorgula.DelilSorgula(WebServiceClient, KepHesabi, KepMesajId);
			EyLog.Log("Delil Sorgula", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyPaketDelilSonuc DelilSorgula(int KepSiraNo)
		{
			EyLog.Log("Delil Sorgula", "Başladı", KepHesabi.kepHesap);
			EyPaketDelilSonuc result = EYazismaDelilSorgula.DelilSorgula(WebServiceClient, KepHesabi, KepSiraNo);
			EyLog.Log("Delil Sorgula", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public void Dispose()
		{
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		public EyDizinSonuc DizinSorgula()
		{
			EyLog.Log("Dizin Sorgula", "Başladı", KepHesabi.kepHesap);
			EyDizinSonuc result = EYazismaDizinSorgula.DizinSorgula(WebServiceClient, KepHesabi);
			EyLog.Log("Dizin Sorgula", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyGirisSonuc Giris(EYazismaGirisTur GirisTuru)
		{
			EyLog.Log("Giriş", "Başladı", KepHesabi.kepHesap);
			EyGirisSonuc result = EYazismaGiris.Giris(WebServiceClient, KepHesabi, GirisTuru);
			EyLog.Log("Giriş", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		[Obsolete]
		public EyGuvenliGirisSonuc GuvenliGiris(EYazismaGirisTur GirisTuru, string SmsKey, byte[] P7sImza, string GuvenlikId)
		{
			EyLog.Log("Güvenli Giriş", "Başladı", KepHesabi.kepHesap);
			switch (GirisTuru)
			{
			case EYazismaGirisTur.OTP:
				return GuvenliGiris(GuvenlikId, SmsKey);
			case EYazismaGirisTur.EIMZA:
				return GuvenliEGiris(GuvenlikId, P7sImza);
			default:
				throw new EYazismaException("EYazismaGirisTur Boş Bırakılamaz.");
			}
		}

		[Obsolete]
		public EyGuvenliGirisSonuc GuvenliGiris(EYazismaGirisTur GirisTuru, string GuvenlikId, string SmsKey, string P7sAdresi)
		{
			EyLog.Log("Güvenli Giriş", "Başladı", KepHesabi.kepHesap);
			switch (GirisTuru)
			{
			case EYazismaGirisTur.OTP:
				return GuvenliGiris(GuvenlikId, SmsKey);
			case EYazismaGirisTur.EIMZA:
				return GuvenliEGiris(GuvenlikId, P7sAdresi);
			default:
				throw new EYazismaException("EYazismaGirisTur Boş Bırakılamaz.");
			}
		}

		public EyGuvenliGirisSonuc GuvenliEGiris(string GuvenlikId, byte[] P7sImza)
		{
			EyLog.Log("Güvenli Giriş", "Başladı", KepHesabi.kepHesap);
			EyGuvenliGirisSonuc result = EYazismaGuvenliGiris.GuvenliEGiris(WebServiceClient, KepHesabi, GuvenlikId, P7sImza);
			EyLog.Log("Güvenli Giriş", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyGuvenliGirisSonuc GuvenliEGiris(string GuvenlikId, string P7sAdresi)
		{
			EyLog.Log("Güvenli Giriş", "Başladı", KepHesabi.kepHesap);
			EyGuvenliGirisSonuc result = EYazismaGuvenliGiris.GuvenliEGiris(WebServiceClient, KepHesabi, GuvenlikId, P7sAdresi);
			EyLog.Log("Güvenli Giriş", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyGuvenliGirisSonuc GuvenliGiris(string GuvenlikId, string SmsKey)
		{
			EyLog.Log("Güvenli Giriş", "Başladı", KepHesabi.kepHesap);
			EyGuvenliGirisSonuc result = EYazismaGuvenliGiris.GuvenliGiris(WebServiceClient, KepHesabi, SmsKey, GuvenlikId);
			EyLog.Log("Güvenli Giriş", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		private EyYukleSonuc KepIletiGonder(string YuklenecekDosyaAdresi, EYazismaPaketTur? PaketTuru)
		{
			EyLog.Log("Yükle", "Başladı", KepHesabi.kepHesap);
			EyYukleSonuc result = EYazismaYukle.Yukle(WebServiceClient, KepHesabi, YuklenecekDosyaAdresi, PaketTuru);
			EyLog.Log("Yükle", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		private EyYukleSonuc KepIletiGonder(byte[] YuklenecekDosya, EYazismaPaketTur? PaketTuru)
		{
			EyLog.Log("Yükle", "Başladı", KepHesabi.kepHesap);
			EyYukleSonuc result = EYazismaYukle.Yukle(WebServiceClient, KepHesabi, YuklenecekDosya, PaketTuru);
			EyLog.Log("Yükle", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyYukleSonuc Yukle(string YuklenecekDosyaAdresi, EYazismaPaketTur? PaketTuru)
		{
			return KepIletiGonder(YuklenecekDosyaAdresi, PaketTuru);
		}

		public EyYukleSonuc Yukle(byte[] YuklenecekDosya, EYazismaPaketTur? PaketTuru)
		{
			return KepIletiGonder(YuklenecekDosya, PaketTuru);
		}

		public EyKontorSonuc KontorSorgula()
		{
			EyLog.Log("Kontor Sorgula", "Başladı", KepHesabi.kepHesap);
			EyKontorSonuc result = EYazismaKontorSorgula.KontorSorgula(WebServiceClient, KepHesabi);
			EyLog.Log("Kontor Sorgula", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyKotaSonuc KotaSorgula()
		{
			EyLog.Log("Kota Sorgula", "Başladı", KepHesabi.kepHesap);
			EyKotaSonuc result = EYazismaKotaSorgula.KotaSorgula(WebServiceClient, KepHesabi);
			EyLog.Log("Kota Sorgula", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyMimeYapSonuc MimeYap(List<string> Kime, string Konu, string Icerik, List<string> EklerinAdresi, EYazismaPaketTur PaketTur, string PaketTurId, EYazismaOzetAlg OzetAlg, EYazismaIcerikTur IcerikTur)
		{
			EyLog.Log("Mime Yap", "Başladı", KepHesabi.kepHesap);
			EyMimeYapSonuc result = EYazismaMimeYap.MimeYap(WebServiceClient, KepHesabi, EklerinAdresi, Kime, Icerik, Konu, PaketTurId, OzetAlg, IcerikTur, PaketTur, Config);
			EyLog.Log("Mime Yap", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyMimeYapSonuc MimeYap(List<string> Kime, string Konu, string Icerik, EYazismaPaketTur PaketTur, string PaketTurId, List<Ek> Ekler, EYazismaOzetAlg OzetAlg, EYazismaIcerikTur IcerikTur)
		{
			EyLog.Log("Mime Yap", "Başladı", KepHesabi.kepHesap);
			EyMimeYapSonuc result = EYazismaMimeYap.MimeYap(WebServiceClient, KepHesabi, Ekler, Kime, Icerik, Konu, PaketTurId, OzetAlg, IcerikTur, PaketTur, Config);
			EyLog.Log("Mime Yap", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyPaketIndirSonuc PaketIndir(string KepMesajId, string GuvenlikId, string MesajKayitAdresi, EYazismaPart MesajIndirilecekPart, string dizin)
		{
			EyLog.Log("Paket Indir", "Başladı", KepHesabi.kepHesap);
			EyPaketIndirSonuc result = EYazismaPaketIndir.PaketIndir(WebServiceClient, KepHesabi, KepMesajId, GuvenlikId, MesajIndirilecekPart, dizin, MesajKayitAdresi, true);
			EyLog.Log("Paket Indir", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyPaketIndirSonuc PaketIndir(string KepMesajId, string GuvenlikId, EYazismaPart mesajIndirilecekPart, string dizin)
		{
			EyLog.Log("Paket Indir", "Başladı", KepHesabi.kepHesap);
			EyPaketIndirSonuc result = EYazismaPaketIndir.PaketIndir(WebServiceClient, KepHesabi, KepMesajId, GuvenlikId, mesajIndirilecekPart, dizin);
			EyLog.Log("Paket Indir", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyPaketIndirSonuc PaketIndir(int MesajSiraNo, string GuvenlikId, string MesajKayitAdresi, EYazismaPart MesajIndirilecekPart, string dizin)
		{
			EyLog.Log("Paket Indir", "Başladı", KepHesabi.kepHesap);
			EyPaketIndirSonuc result = EYazismaPaketIndir.PaketIndir(WebServiceClient, KepHesabi, MesajSiraNo, GuvenlikId, MesajIndirilecekPart, dizin, MesajKayitAdresi, true);
			EyLog.Log("Paket Indir", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyPaketIndirSonuc PaketIndir(int MesajSiraNo, string GuvenlikId, EYazismaPart MesajIndirilecekPart, string dizin)
		{
			EyLog.Log("Paket Indir", "Başladı", KepHesabi.kepHesap);
			EyPaketIndirSonuc result = EYazismaPaketIndir.PaketIndir(WebServiceClient, KepHesabi, MesajSiraNo, GuvenlikId, MesajIndirilecekPart, dizin);
			EyLog.Log("Paket Indir", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyDurumSonuc PaketSil(string KepMesajId, string dizin)
		{
			EyLog.Log("Paket Sil", "Başladı", KepHesabi.kepHesap);
			EyDurumSonuc result = EYazismaPaketSil.PaketSil(WebServiceClient, KepHesabi, KepMesajId, dizin);
			EyLog.Log("Paket Sil", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyDurumSonuc PaketSil(int MesajSiraNo, string dizin)
		{
			EyLog.Log("Paket Sil", "Başladı", KepHesabi.kepHesap);
			EyDurumSonuc result = EYazismaPaketSil.PaketSil(WebServiceClient, KepHesabi, MesajSiraNo, dizin);
			EyLog.Log("Paket Sil", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyPaketSonuc PaketSorgula(DateTime? IlkTarih, DateTime? SonTarih, string dizin)
		{
			EyLog.Log("Paket Sorgula", "Başladı", KepHesabi.kepHesap);
			EyPaketSonuc result = EYazismaPaketSorgula.PaketSorgula(WebServiceClient, KepHesabi, IlkTarih, SonTarih, dizin);
			EyLog.Log("Paket Sorgula", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyDurumSonuc SmimeGonder(string ImzaliDosyaAdresi, string KepMesajId)
		{
			EyLog.Log("Smime Gönder", "Başladı", KepHesabi.kepHesap);
			EyDurumSonuc result = EYazismaSmimeGonder.SmimeGonder(WebServiceClient, KepHesabi, ImzaliDosyaAdresi, KepMesajId);
			EyLog.Log("Smime Gönder", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyDurumSonuc SmimeGonder(byte[] ImzaliDosyaBytes, string KepMesajId)
		{
			EyLog.Log("Smime Gönder", "Başladı", KepHesabi.kepHesap);
			EyDurumSonuc result = EYazismaSmimeGonder.SmimeGonder(WebServiceClient, KepHesabi, ImzaliDosyaBytes, KepMesajId);
			EyLog.Log("Smime Gönder", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyMimeOlusturSonuc MimeOlustur(List<string> Kime, List<string> Cc, List<string> Bcc, string Konu, string Icerik, List<Ek> Ekler, EYazismaPaketTur MailTip, string MailTipId, OzetAlg OzetAlg)
		{
			EyLog.Log("Mime Oluştur", "Başladı", KepHesabi.kepHesap);
			EyMimeOlusturSonuc result = EYazismaMimeOlustur.MimeYap(KepHesabi.kepHesap, Kime, Cc, Bcc, Konu, Icerik, Ekler, MailTip, MailTipId, OzetAlg, Config);
			EyLog.Log("Mime Oluştur", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyMimeOlusturSonuc MimeOlustur(List<string> Kime, List<string> Cc, List<string> Bcc, string Konu, List<string> Ekler, string Icerik, EYazismaPaketTur MailTip, string MailTipId, OzetAlg OzetAlg)
		{
			EyLog.Log("Mime Oluştur", "Başladı", KepHesabi.kepHesap);
			EyMimeOlusturSonuc result = EYazismaMimeOlustur.MimeYap(KepHesabi.kepHesap, Kime, Cc, Bcc, Konu, Icerik, Ekler, MailTip, MailTipId, OzetAlg, Config);
			EyLog.Log("Mime Oluştur", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyMimeOlusturSonuc MimeOlustur(EyMimeMessage mimeInfo, OzetAlg OzetAlg, List<Ek> Ekler)
		{
			EyLog.Log("Mime Oluştur", "Başladı", KepHesabi.kepHesap);
			EyMimeOlusturSonuc result = EYazismaMimeOlustur.MimeYap(KepHesabi.kepHesap, mimeInfo, Ekler, OzetAlg, Config);
			EyLog.Log("Mime Oluştur", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyMimeOlusturSonuc MimeOlustur(EyMimeMessage mimeInfo, List<string> Ekler, OzetAlg OzetAlg)
		{
			EyLog.Log("Mime Oluştur", "Başladı", KepHesabi.kepHesap);
			EyMimeOlusturSonuc result = EYazismaMimeOlustur.MimeYap(KepHesabi.kepHesap, mimeInfo, Ekler, OzetAlg, Config);
			EyLog.Log("Mime Oluştur", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EySmimeOlusturSonuc SmimeOlustur(List<string> Kime, List<string> Cc, List<string> Bcc, string Konu, byte[] Icerik, byte[] MimeP7sImzasi, EYazismaPaketTur MailTip, string MailTipId)
		{
			EyLog.Log("Smime Oluştur", "Başladı", KepHesabi.kepHesap);
			EySmimeOlusturSonuc result = EYazismaSmimeOlustur.SmimeYap(KepHesabi.kepHesap, Kime, Cc, Bcc, Konu, Icerik, MimeP7sImzasi, MailTip, MailTipId, Config);
			EyLog.Log("Smime Oluştur", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EySmimeOlusturSonuc SmimeOlustur(EyMimeMessage Mesaj, byte[] Icerik, byte[] MimeP7sImzasi)
		{
			EyLog.Log("SMime Oluştur", "Başladı", KepHesabi.kepHesap);
			EySmimeOlusturSonuc result = EYazismaSmimeOlustur.SMimeYap(Mesaj, Icerik, MimeP7sImzasi, Config);
			EyLog.Log("SMime Oluştur", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyTevdiListesiSonuc TevdiListesiOlustur(string birimID)
		{
			EyLog.Log("Tevdi Listesi Olustur", "Başladı", KepHesabi.kepHesap);
			EyTevdiListesiSonuc result = EYazismaTevdiListesiOlustur.TevdiListesiOlustur(WebServiceClient, KepHesabi, birimID);
			EyLog.Log("Tevdi Listesi Olustur", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyDurumSonuc TevdiListesiSil(string tevdiListeNo)
		{
			EyLog.Log("Tevdi Listesi Sil", "Başladı", KepHesabi.kepHesap);
			EyDurumSonuc result = EYazismaTevdiListesiSil.TevdiListesiSil(WebServiceClient, KepHesabi, tevdiListeNo);
			EyLog.Log("Tevdi Listesi Sil", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyYetkiliKayitSonuc YetkiliKayit(string TcNo, string TelefonNo, string Adi, string Soyadi, string EPosta)
		{
			EyLog.Log("Yetkili Kayıt", "Başladı", KepHesabi.kepHesap);
			EyYetkiliKayitSonuc result = EYazismaYetkiliKayit.YetkiliKayit(WebServiceClient, KepHesabi, TcNo, TelefonNo, Adi, Soyadi, EPosta);
			EyLog.Log("Yetkili Kayıt", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyYetkiliKayitSonuc YetkiliGuncelle(string TcNo, string TelefonNo, string Adi, string Soyadi, string EPosta)
		{
			EyLog.Log("Yetkili Kayıt", "Başladı", KepHesabi.kepHesap);
			EyYetkiliKayitSonuc result = EYazismaYetkiliKayit.YetkiliKayit(WebServiceClient, KepHesabi, TcNo, TelefonNo, Adi, Soyadi, EPosta, "UPDATE");
			EyLog.Log("Yetkili Kayıt", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyDurumSonuc YetkiliSil(string TcNo, string Adi, string Soyadi)
		{
			EyLog.Log("Yetkili Sil", "Başladı", KepHesabi.kepHesap);
			EyDurumSonuc result = EYazismaYetkiliSil.YetkiliSil(WebServiceClient, KepHesabi, TcNo, Adi, Soyadi);
			EyLog.Log("Yetkili Sil", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyYukleTebligatSonuc TebligatYukle(byte[] SmimeKepIletisi, string barkod, string birimID, string birimAdi, bool donusumlu)
		{
			EyLog.Log("Yukle Tebligat", "Başladı", KepHesabi.kepHesap);
			EyYukleTebligatSonuc result = EYazismaYukleTebligat.YukleTebligat(WebServiceClient, KepHesabi, SmimeKepIletisi, barkod, birimID, birimAdi, donusumlu);
			EyLog.Log("Yukle Tebligat", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyYukleTebligatSonuc TebligatYukle(string YuklenecekDosyaAdresi, string barkod, string birimID, string birimAdi, bool donusumlu)
		{
			EyLog.Log("Yukle Tebligat", "Başladı", KepHesabi.kepHesap);
			EyYukleTebligatSonuc result = EYazismaYukleTebligat.YukleTebligat(WebServiceClient, KepHesabi, YuklenecekDosyaAdresi, barkod, birimID, birimAdi, donusumlu);
			EyLog.Log("Yukle Tebligat", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}
	}
}
