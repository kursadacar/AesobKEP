using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Tr.Com.Eimza.EYazisma.Config;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	public class EYazismaApi : IDisposable
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly EYazismaApiConfig Config;

		private readonly EimzaApi eimzaApi;

		private readonly SharedApi sharedApi;

		public EYazismaApi(string KepHesapAdi, string TcNo, string Parola, string Sifre)
		{
			SHA256Managed sHA256Managed = new SHA256Managed();
			if (!KepHesapKontrol(KepHesapAdi).Equals(Convert.ToBase64String(sHA256Managed.ComputeHash(Encoding.ASCII.GetBytes(KepHesapAdi)))))
			{
				throw new Exception("KepHesapAdi : \"" + KepHesapAdi + "\" Posta ve Telgraf Teşkilatı Anonim Şirketi'ne ait Kep hesabı değildir.");
			}
			Config = new EYazismaApiConfig();
			eimzaApi = new EimzaApi(KepHesapAdi, TcNo, Parola, Sifre, Config);
			sharedApi = new SharedApi(KepHesapAdi, TcNo, Parola, Sifre, Config);
			LogUtilities.CreateLogFile(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		public EYazismaApi(string KepHesapAdi, string TcNo, string Parola, string Sifre, string WsdlAdresi)
		{
			SHA256Managed sHA256Managed = new SHA256Managed();
			if (!KepHesapKontrol(KepHesapAdi).Equals(Convert.ToBase64String(sHA256Managed.ComputeHash(Encoding.ASCII.GetBytes(KepHesapAdi)))))
			{
				throw new Exception("KepHesapAdi : \"" + KepHesapAdi + "\" Posta ve Telgraf Teşkilatı Anonim Şirketi'ne ait Kep hesabı değildir.");
			}
			Config = new EYazismaApiConfig
			{
				WsdlUrl = WsdlAdresi
			};
			eimzaApi = new EimzaApi(KepHesapAdi, TcNo, Parola, Sifre, Config);
			sharedApi = new SharedApi(KepHesapAdi, TcNo, Parola, Sifre, Config);
			LogUtilities.CreateLogFile(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		public EYazismaApi(string KepHesapAdi, string TcNo, string Parola, string Sifre, BasicHttpBinding WebServiceConfigs)
		{
			SHA256Managed sHA256Managed = new SHA256Managed();
			if (!KepHesapKontrol(KepHesapAdi).Equals(Convert.ToBase64String(sHA256Managed.ComputeHash(Encoding.ASCII.GetBytes(KepHesapAdi)))))
			{
				throw new Exception("KepHesapAdi : \"" + KepHesapAdi + "\" Posta ve Telgraf Teşkilatı Anonim Şirketi'ne ait Kep hesabı değildir.");
			}
			Config = new EYazismaApiConfig
			{
				WebServiceAyarlari = WebServiceConfigs
			};
			eimzaApi = new EimzaApi(KepHesapAdi, TcNo, Parola, Sifre, Config);
			sharedApi = new SharedApi(KepHesapAdi, TcNo, Parola, Sifre, Config);
			LogUtilities.CreateLogFile(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		public EYazismaApi(string KepHesapAdi, string TcNo, string Parola, string Sifre, BasicHttpBinding WebServiceConfigs, string WsdlAdresi)
		{
			SHA256Managed sHA256Managed = new SHA256Managed();
			if (!KepHesapKontrol(KepHesapAdi).Equals(Convert.ToBase64String(sHA256Managed.ComputeHash(Encoding.ASCII.GetBytes(KepHesapAdi)))))
			{
				throw new Exception("KepHesapAdi : \"" + KepHesapAdi + "\" Posta ve Telgraf Teşkilatı Anonim Şirketi'ne ait Kep hesabı değildir.");
			}
			Config = new EYazismaApiConfig
			{
				WsdlUrl = WsdlAdresi,
				WebServiceAyarlari = WebServiceConfigs
			};
			eimzaApi = new EimzaApi(KepHesapAdi, TcNo, Parola, Sifre, Config);
			sharedApi = new SharedApi(KepHesapAdi, TcNo, Parola, Sifre, Config);
			LogUtilities.CreateLogFile(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		internal EYazismaApi(string KepHesapAdi, string TcNo, string Parola, string Sifre, EYazismaApiConfig Ayarlar)
		{
			SHA256Managed sHA256Managed = new SHA256Managed();
			if (!KepHesapKontrol(KepHesapAdi).Equals(Convert.ToBase64String(sHA256Managed.ComputeHash(Encoding.ASCII.GetBytes(KepHesapAdi)))))
			{
				throw new Exception("KepHesapAdi : \"" + KepHesapAdi + "\" Posta ve Telgraf Teşkilatı Anonim Şirketi'ne ait Kep hesabı değildir.");
			}
			Config = Ayarlar;
			eimzaApi = new EimzaApi(KepHesapAdi, TcNo, Parola, Sifre, Config);
			sharedApi = new SharedApi(KepHesapAdi, TcNo, Parola, Sifre, Config);
			LogUtilities.CreateLogFile(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		~EYazismaApi()
		{
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		public EyDurumSonuc AlindiOnay(string KepMesajId, string dizin = "INBOX")
		{
			return sharedApi.AlindiOnay(KepMesajId, dizin);
		}

		public EyDurumSonuc AlindiOnay(int MesajSiraNo, string dizin = "INBOX")
		{
			return sharedApi.AlindiOnay(MesajSiraNo, dizin);
		}

		public EyPaketDelilIndirSonuc DelilIndir(string DelilID, string DelilKayitAdresi)
		{
			return sharedApi.DelilIndir(DelilID, DelilKayitAdresi);
		}

		public EyPaketDelilIndirSonuc DelilIndir(string DelilID)
		{
			return sharedApi.DelilIndir(DelilID);
		}

		public EyPaketDelilSonuc DelilSorgula(string KepMesajId)
		{
			return sharedApi.DelilSorgula(KepMesajId);
		}

		public EyPaketDelilSonuc DelilSorgula(int KepSiraNo)
		{
			return sharedApi.DelilSorgula(KepSiraNo);
		}

		public void Dispose()
		{
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		public EyDizinSonuc DizinSorgula()
		{
			return sharedApi.DizinSorgula();
		}

		public EySmartCardSonuc GetSmartCardAndCertificateList()
		{
			return eimzaApi.GetSmartCardAndCertificateList();
		}

		public EyGirisSonuc Giris(EYazismaGirisTur GirisTuru)
		{
			return sharedApi.Giris(GirisTuru);
		}

		[Obsolete]
		public EyGuvenliGirisSonuc GuvenliGiris(EYazismaGirisTur GirisTuru, string SmsKey, byte[] P7sImza, string GuvenlikId)
		{
			return sharedApi.GuvenliGiris(GirisTuru, SmsKey, P7sImza, GuvenlikId);
		}

		[Obsolete]
		public EyGuvenliGirisSonuc GuvenliGiris(EYazismaGirisTur GirisTuru, string GuvenlikId, string SmsKey, string P7sAdresi)
		{
			return sharedApi.GuvenliGiris(GirisTuru, GuvenlikId, SmsKey, P7sAdresi);
		}

		public EyGuvenliGirisSonuc GuvenliEGiris(string GuvenlikId, byte[] P7sImza)
		{
			return sharedApi.GuvenliEGiris(GuvenlikId, P7sImza);
		}

		public EyGuvenliGirisSonuc GuvenliEGiris(string GuvenlikId, string P7sAdresi)
		{
			return sharedApi.GuvenliEGiris(GuvenlikId, P7sAdresi);
		}

		public EyGuvenliGirisSonuc GuvenliGiris(string GuvenlikId, string SmsKey)
		{
			return sharedApi.GuvenliGiris(GuvenlikId, SmsKey);
		}

		public EyHashImzalaSonuc HashImzala(string MimeHashBase64, string SmartCardPin, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.HashImzala(MimeHashBase64, SmartCardPin, SmartCardIndex, CertificateIndex);
		}

		public EyHashImzalaSonuc HashImzala(string MimeHashBase64, string SmartCardPin, OzetAlg OzetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.HashImzala(MimeHashBase64, SmartCardPin, OzetAlg, SmartCardIndex, CertificateIndex);
		}

		public EyHashImzalaSonuc HashImzala(byte[] MimeHash, string SmartCardPin, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.HashImzala(MimeHash, SmartCardPin, SmartCardIndex, CertificateIndex);
		}

		public EyHashImzalaSonuc HashImzala(byte[] MimeHash, string SmartCardPin, OzetAlg OzetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.HashImzala(MimeHash, SmartCardPin, OzetAlg, SmartCardIndex, CertificateIndex);
		}

		public EyImzalaGonderSonuc ImzalaGonder(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, List<string> EklerAdresi, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.ImzalaGonder(Kime, CC, Bcc, Konu, Icerik, EklerAdresi, PaketTur, PaketTurId, SmartCardPIN, SmartCardIndex, CertificateIndex);
		}

		public EyImzalaGonderSonuc ImzalaGonder(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, List<string> EklerAdresi, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, OzetAlg ozetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.ImzalaGonder(Kime, CC, Bcc, Konu, Icerik, EklerAdresi, PaketTur, PaketTurId, SmartCardPIN, ozetAlg, SmartCardIndex, CertificateIndex);
		}

		public EyImzalaGonderSonuc ImzalaGonder(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaPaketTur PaketTur, string PaketTurId, List<Ek> EklerAdresi, string SmartCardPIN, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.ImzalaGonder(Kime, CC, Bcc, Konu, Icerik, PaketTur, PaketTurId, EklerAdresi, SmartCardPIN, SmartCardIndex, CertificateIndex);
		}

		public EyImzalaGonderSonuc ImzalaGonder(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, List<Ek> EklerAdresi, OzetAlg ozetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.ImzalaGonder(Kime, CC, Bcc, Konu, Icerik, PaketTur, PaketTurId, SmartCardPIN, EklerAdresi, ozetAlg, SmartCardIndex, CertificateIndex);
		}

		public EyImzalaGonderSonuc ImzalaGonderV2(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaIcerikTur IcerikTuru, List<string> EklerAdresi, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.ImzalaGonderV2(Kime, CC, Bcc, Konu, Icerik, IcerikTuru, EklerAdresi, PaketTur, PaketTurId, SmartCardPIN, SmartCardIndex, CertificateIndex);
		}

		public EyImzalaGonderSonuc ImzalaGonderV2(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaIcerikTur IcerikTuru, List<string> EklerAdresi, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, OzetAlg ozetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.ImzalaGonderV2(Kime, CC, Bcc, Konu, Icerik, IcerikTuru, EklerAdresi, PaketTur, PaketTurId, SmartCardPIN, ozetAlg, SmartCardIndex, CertificateIndex);
		}

		public EyImzalaGonderSonuc ImzalaGonderV2(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaIcerikTur IcerikTuru, EYazismaPaketTur PaketTur, string PaketTurId, List<Ek> EklerAdresi, string SmartCardPIN, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.ImzalaGonderV2(Kime, CC, Bcc, Konu, Icerik, IcerikTuru, PaketTur, PaketTurId, EklerAdresi, SmartCardPIN, SmartCardIndex, CertificateIndex);
		}

		public EyImzalaGonderSonuc ImzalaGonderV2(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaIcerikTur IcerikTuru, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, List<Ek> EklerAdresi, OzetAlg ozetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			return eimzaApi.ImzalaGonderV2(Kime, CC, Bcc, Konu, Icerik, IcerikTuru, PaketTur, PaketTurId, SmartCardPIN, EklerAdresi, ozetAlg, SmartCardIndex, CertificateIndex);
		}

		private EyYukleSonuc KepIletiGonder(string YuklenecekDosyaAdresi, EYazismaPaketTur? PaketTuru)
		{
			return sharedApi.Yukle(YuklenecekDosyaAdresi, PaketTuru);
		}

		private EyYukleSonuc KepIletiGonder(byte[] YuklenecekDosya, EYazismaPaketTur? PaketTuru)
		{
			return sharedApi.Yukle(YuklenecekDosya, PaketTuru);
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
			return sharedApi.KontorSorgula();
		}

		public EyKotaSonuc KotaSorgula()
		{
			return sharedApi.KotaSorgula();
		}

		public EyMimeOlusturSonuc MimeOlustur(List<string> Kime, List<string> Cc, List<string> Bcc, string Konu, string Icerik, List<Ek> Ekler, EYazismaPaketTur MailTip, string MailTipId, OzetAlg OzetAlg)
		{
			return sharedApi.MimeOlustur(Kime, Cc, Bcc, Konu, Icerik, Ekler, MailTip, MailTipId, OzetAlg);
		}

		public EyMimeOlusturSonuc MimeOlustur(List<string> Kime, List<string> Cc, List<string> Bcc, string Konu, List<string> Ekler, string Icerik, EYazismaPaketTur MailTip, string MailTipId, OzetAlg OzetAlg)
		{
			return sharedApi.MimeOlustur(Kime, Cc, Bcc, Konu, Ekler, Icerik, MailTip, MailTipId, OzetAlg);
		}

		public EyMimeOlusturSonuc MimeOlustur(EyMimeMessage mimeInfo, OzetAlg OzetAlg, List<Ek> Ekler)
		{
			return sharedApi.MimeOlustur(mimeInfo, OzetAlg, Ekler);
		}

		public EyMimeOlusturSonuc MimeOlustur(EyMimeMessage mimeInfo, List<string> Ekler, OzetAlg OzetAlg)
		{
			return sharedApi.MimeOlustur(mimeInfo, Ekler, OzetAlg);
		}

		public EyMimeYapSonuc MimeYap(List<string> Kime, string Konu, string Icerik, List<string> EklerinAdresi, EYazismaPaketTur PaketTur, string PaketTurId, EYazismaOzetAlg OzetAlg, EYazismaIcerikTur IcerikTur)
		{
			return sharedApi.MimeYap(Kime, Konu, Icerik, EklerinAdresi, PaketTur, PaketTurId, OzetAlg, IcerikTur);
		}

		public EyMimeYapSonuc MimeYap(List<string> Kime, string Konu, string Icerik, EYazismaPaketTur PaketTur, string PaketTurId, List<Ek> Ekler, EYazismaOzetAlg OzetAlg, EYazismaIcerikTur IcerikTur)
		{
			return sharedApi.MimeYap(Kime, Konu, Icerik, PaketTur, PaketTurId, Ekler, OzetAlg, IcerikTur);
		}

		public EyPaketIndirSonuc PaketIndir(string KepMesajId, string GuvenlikId, string MesajKayitAdresi, EYazismaPart MesajIndirilecekPart, string dizin = "INBOX")
		{
			return sharedApi.PaketIndir(KepMesajId, GuvenlikId, MesajKayitAdresi, MesajIndirilecekPart, dizin);
		}

		public EyPaketIndirSonuc PaketIndir(string KepMesajId, string GuvenlikId, EYazismaPart MesajIndirilecekPart, string dizin = "INBOX")
		{
			return sharedApi.PaketIndir(KepMesajId, GuvenlikId, MesajIndirilecekPart, dizin);
		}

		public EyPaketIndirSonuc PaketIndir(int MesajSiraNo, string GuvenlikId, string MesajKayitAdresi, EYazismaPart MesajIndirilecekPart, string dizin = "INBOX")
		{
			return sharedApi.PaketIndir(MesajSiraNo, GuvenlikId, MesajKayitAdresi, MesajIndirilecekPart, dizin);
		}

		public EyPaketIndirSonuc PaketIndir(int MesajSiraNo, string GuvenlikId, EYazismaPart MesajIndirilecekPart, string dizin = "INBOX")
		{
			return sharedApi.PaketIndir(MesajSiraNo, GuvenlikId, MesajIndirilecekPart, dizin);
		}

		public EyDurumSonuc PaketSil(string KepMesajId, string dizin = "INBOX")
		{
			return sharedApi.PaketSil(KepMesajId, dizin);
		}

		public EyDurumSonuc PaketSil(int MesajSiraNo, string dizin = "INBOX")
		{
			return sharedApi.PaketSil(MesajSiraNo, dizin);
		}

		public EyPaketSonuc PaketSorgula(DateTime? IlkTarih, DateTime? SonTarih, string dizin = "INBOX")
		{
			return sharedApi.PaketSorgula(IlkTarih, SonTarih, dizin);
		}

		public EyDurumSonuc SmimeGonder(string ImzaliDosyaAdresi, string KepMesajId)
		{
			return sharedApi.SmimeGonder(ImzaliDosyaAdresi, KepMesajId);
		}

		public EyDurumSonuc SmimeGonder(byte[] ImzaliDosyaBytes, string KepMesajId)
		{
			return sharedApi.SmimeGonder(ImzaliDosyaBytes, KepMesajId);
		}

		public EySmimeOlusturSonuc SmimeOlustur(List<string> Kime, List<string> Cc, List<string> Bcc, string Konu, byte[] Icerik, byte[] MimeP7sImzasi, EYazismaPaketTur MailTip, string MailTipId)
		{
			return sharedApi.SmimeOlustur(Kime, Cc, Bcc, Konu, Icerik, MimeP7sImzasi, MailTip, MailTipId);
		}

		public EySmimeOlusturSonuc SmimeOlustur(EyMimeMessage Mesaj, byte[] Icerik, byte[] MimeP7sImzasi)
		{
			return sharedApi.SmimeOlustur(Mesaj, Icerik, MimeP7sImzasi);
		}

		public EySmimeParcalaSonuc SmimeParcala(string SmimeKepIletisiBase64)
		{
			return eimzaApi.SmimeParcala(SmimeKepIletisiBase64);
		}

		public EySmimeParcalaSonuc SmimeParcala(byte[] SmimeKepIletisi)
		{
			return eimzaApi.SmimeParcala(SmimeKepIletisi);
		}

		public EyTevdiListesiSonuc TevdiListesiOlustur(string birimID)
		{
			return sharedApi.TevdiListesiOlustur(birimID);
		}

		public EyDurumSonuc TevdiListesiSil(string tevdiListeNo)
		{
			return sharedApi.TevdiListesiSil(tevdiListeNo);
		}

		public EyYetkiliKayitSonuc YetkiliKayit(string TcNo, string TelefonNo, string Adi, string Soyadi, string EPosta)
		{
			return sharedApi.YetkiliKayit(TcNo, TelefonNo, Adi, Soyadi, EPosta);
		}

		public EyYetkiliKayitSonuc YetkiliGuncelle(string TcNo, string TelefonNo, string Adi, string Soyadi, string EPosta)
		{
			return sharedApi.YetkiliGuncelle(TcNo, TelefonNo, Adi, Soyadi, EPosta);
		}

		public EyDurumSonuc YetkiliSil(string TcNo, string Adi, string Soyadi)
		{
			return sharedApi.YetkiliSil(TcNo, Adi, Soyadi);
		}

		public EyYukleTebligatSonuc TebligatYukle(byte[] SmimeKepIletisi, string barkod, string birimID, string birimAdi, bool donusumlu)
		{
			return sharedApi.TebligatYukle(SmimeKepIletisi, barkod, birimID, birimAdi, donusumlu);
		}

		public EyYukleTebligatSonuc TebligatYukle(string YuklenecekDosyaAdresi, string barkod, string birimID, string birimAdi, bool donusumlu)
		{
			return sharedApi.TebligatYukle(YuklenecekDosyaAdresi, barkod, birimID, birimAdi, donusumlu);
		}

		private string KepHesapKontrol(string KepHesapAdi)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			Thread.CurrentThread.CurrentCulture.DateTimeFormat = Util.TR.DateTimeFormat;
			if (KepHesapAdi.ToLower().Contains('@'))
			{
				string text = KepHesapAdi.ToLower().Split('@')[1];
				if (text.Contains("testkep.pttkep.gov.tr") || text.Contains("hs01.kep.tr"))
				{
					return Convert.ToBase64String(new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(KepHesapAdi)));
				}
				EyLog.Log("EYazismaApi Yapıcı", EyLogTuru.HATA, "KepHesapAdi " + KepHesapAdi + "Posta ve Telgraf Teşkilatı Anonim Şirketi'ne ait Kep hesabı değildir.");
				return "1";
			}
			EyLog.Log("EYazismaApi Yapıcı", EyLogTuru.HATA, "KepHesapAdi " + KepHesapAdi + "'in Mail Formatını Kontrol Ediniz...");
			return "1";
		}
	}
}
