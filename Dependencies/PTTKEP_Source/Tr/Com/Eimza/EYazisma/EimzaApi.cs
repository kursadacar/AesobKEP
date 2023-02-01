using System;
using System.Collections.Generic;
using System.Reflection;
using Tr.Com.Eimza.EYazisma.Config;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using Tr.Com.Eimza.EYazisma.SmartCard;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.SmartCard;
using Tr.Com.Eimza.SmartCard.Tools;

namespace Tr.Com.Eimza.EYazisma
{
	public class EimzaApi : IDisposable
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly EYazismaApiConfig Config;

		private readonly eyKepHesapGirisP KepHesabi;

		private readonly eyServisPortTypeClient WebServiceClient;

		public EimzaApi(string KepHesapAdi, string TcNo, string Parola, string Sifre, EYazismaApiConfig Ayarlar)
		{
			KepHesabi = new eyKepHesapGirisP();
			KepHesabi.kepHesap = KepHesapAdi;
			KepHesabi.tcno = TcNo;
			KepHesabi.parola = Parola;
			KepHesabi.sifre = Sifre;
			Config = Ayarlar;
			WebServiceClient = Config.EyServisPortClient;
		}

		~EimzaApi()
		{
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		public void Dispose()
		{
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
		}

		public EySmartCardSonuc GetSmartCardAndCertificateList()
		{
			EyLog.Log("Akıllı Kart Sorgula", "Başladı", KepHesabi.kepHesap);
			EySmartCardSonuc result = new EySmartCardSonuc
			{
				SmartCardList = GetSmartCardList()
			};
			EyLog.Log("Akıllı Kart Sorgula", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyHashImzalaSonuc HashImzala(string MimeHashBase64, string SmartCardPin, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Hash Imzala", "Başladı", KepHesabi.kepHesap);
			EYazismaHashImzala eYazismaHashImzala = new EYazismaHashImzala();
			EyHashImzalaSonuc result = eYazismaHashImzala.HashImzala(MimeHashBase64, SmartCardPin, SmartCardIndex, CertificateIndex);
			eYazismaHashImzala.CloseSmartCardSession();
			EyLog.Log("Hash Imzala", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyHashImzalaSonuc HashImzala(string MimeHashBase64, string SmartCardPin, OzetAlg OzetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Hash Imzala", "Başladı", KepHesabi.kepHesap);
			EYazismaHashImzala eYazismaHashImzala = new EYazismaHashImzala();
			EyHashImzalaSonuc result = eYazismaHashImzala.HashImzala(MimeHashBase64, SmartCardPin, OzetAlg, SmartCardIndex, CertificateIndex);
			eYazismaHashImzala.CloseSmartCardSession();
			EyLog.Log("Hash Imzala", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyHashImzalaSonuc HashImzala(byte[] MimeHash, string SmartCardPin, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Hash Imzala", "Başladı", KepHesabi.kepHesap);
			EYazismaHashImzala eYazismaHashImzala = new EYazismaHashImzala();
			EyHashImzalaSonuc result = eYazismaHashImzala.HashImzala(MimeHash, SmartCardPin, SmartCardIndex, CertificateIndex);
			eYazismaHashImzala.CloseSmartCardSession();
			EyLog.Log("Hash Imzala", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyHashImzalaSonuc HashImzala(byte[] MimeHash, string SmartCardPin, OzetAlg OzetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Hash Imzala", "Başladı", KepHesabi.kepHesap);
			EYazismaHashImzala eYazismaHashImzala = new EYazismaHashImzala();
			EyHashImzalaSonuc result = eYazismaHashImzala.HashImzala(MimeHash, SmartCardPin, OzetAlg, SmartCardIndex, CertificateIndex);
			eYazismaHashImzala.CloseSmartCardSession();
			EyLog.Log("Hash Imzala", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyImzalaGonderSonuc ImzalaGonder(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, List<string> EklerAdresi, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Imzala Gönder", "Başladı", KepHesabi.kepHesap);
			EYazismaImzalaGonder eYazismaImzalaGonder = new EYazismaImzalaGonder();
			EyImzalaGonderSonuc result = eYazismaImzalaGonder.ImzalaGonder(WebServiceClient, KepHesabi, SmartCardPIN, Kime, CC, Bcc, Konu, Icerik, EYazismaIcerikTur.TEXT, EklerAdresi, PaketTur, PaketTurId, SmartCardIndex, CertificateIndex, Config);
			eYazismaImzalaGonder.CloseSmartCardSession();
			EyLog.Log("Imzala Gönder", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyImzalaGonderSonuc ImzalaGonder(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, List<string> EklerAdresi, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, OzetAlg ozetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Imzala Gönder", "Başladı", KepHesabi.kepHesap);
			EYazismaImzalaGonder eYazismaImzalaGonder = new EYazismaImzalaGonder();
			EyImzalaGonderSonuc result = eYazismaImzalaGonder.ImzalaGonder(WebServiceClient, KepHesabi, SmartCardPIN, Kime, CC, Bcc, Konu, Icerik, EYazismaIcerikTur.TEXT, EklerAdresi, PaketTur, PaketTurId, ozetAlg, SmartCardIndex, CertificateIndex, Config);
			eYazismaImzalaGonder.CloseSmartCardSession();
			EyLog.Log("Imzala Gönder", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyImzalaGonderSonuc ImzalaGonder(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaPaketTur PaketTur, string PaketTurId, List<Ek> EklerAdresi, string SmartCardPIN, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Imzala Gönder", "Başladı", KepHesabi.kepHesap);
			EYazismaImzalaGonder eYazismaImzalaGonder = new EYazismaImzalaGonder();
			EyImzalaGonderSonuc result = eYazismaImzalaGonder.ImzalaGonder(WebServiceClient, KepHesabi, SmartCardPIN, Kime, CC, Bcc, Konu, Icerik, EYazismaIcerikTur.TEXT, EklerAdresi, PaketTur, PaketTurId, SmartCardIndex, CertificateIndex, Config);
			eYazismaImzalaGonder.CloseSmartCardSession();
			EyLog.Log("Imzala Gönder", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyImzalaGonderSonuc ImzalaGonder(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, List<Ek> EklerAdresi, OzetAlg ozetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Imzala Gönder", "Başladı", KepHesabi.kepHesap);
			EYazismaImzalaGonder eYazismaImzalaGonder = new EYazismaImzalaGonder();
			EyImzalaGonderSonuc result = eYazismaImzalaGonder.ImzalaGonder(WebServiceClient, KepHesabi, SmartCardPIN, Kime, CC, Bcc, Konu, Icerik, EYazismaIcerikTur.TEXT, EklerAdresi, PaketTur, PaketTurId, ozetAlg, SmartCardIndex, CertificateIndex, Config);
			eYazismaImzalaGonder.CloseSmartCardSession();
			EyLog.Log("Imzala Gönder", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyImzalaGonderSonuc ImzalaGonderV2(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaIcerikTur IcerikTuru, List<string> EklerAdresi, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Imzala Gönder V2", "Başladı", KepHesabi.kepHesap);
			EYazismaImzalaGonderV2 eYazismaImzalaGonderV = new EYazismaImzalaGonderV2();
			EyImzalaGonderSonuc result = eYazismaImzalaGonderV.ImzalaGonder(WebServiceClient, KepHesabi, SmartCardPIN, Kime, CC, Bcc, Konu, Icerik, IcerikTuru, EklerAdresi, PaketTur, PaketTurId, SmartCardIndex, CertificateIndex, Config);
			eYazismaImzalaGonderV.CloseSmartCardSession();
			EyLog.Log("Imzala Gönder V2", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyImzalaGonderSonuc ImzalaGonderV2(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaIcerikTur IcerikTuru, List<string> EklerAdresi, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, OzetAlg ozetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Imzala Gönder V2", "Başladı", KepHesabi.kepHesap);
			EYazismaImzalaGonderV2 eYazismaImzalaGonderV = new EYazismaImzalaGonderV2();
			EyImzalaGonderSonuc result = eYazismaImzalaGonderV.ImzalaGonder(WebServiceClient, KepHesabi, SmartCardPIN, Kime, CC, Bcc, Konu, Icerik, IcerikTuru, EklerAdresi, PaketTur, PaketTurId, ozetAlg, SmartCardIndex, CertificateIndex, Config);
			eYazismaImzalaGonderV.CloseSmartCardSession();
			EyLog.Log("Imzala Gönder V2", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyImzalaGonderSonuc ImzalaGonderV2(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaIcerikTur IcerikTuru, EYazismaPaketTur PaketTur, string PaketTurId, List<Ek> EklerAdresi, string SmartCardPIN, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Imzala Gönder V2", "Başladı", KepHesabi.kepHesap);
			EYazismaImzalaGonderV2 eYazismaImzalaGonderV = new EYazismaImzalaGonderV2();
			EyImzalaGonderSonuc result = eYazismaImzalaGonderV.ImzalaGonder(WebServiceClient, KepHesabi, SmartCardPIN, Kime, CC, Bcc, Konu, Icerik, IcerikTuru, EklerAdresi, PaketTur, PaketTurId, SmartCardIndex, CertificateIndex, Config);
			eYazismaImzalaGonderV.CloseSmartCardSession();
			EyLog.Log("Imzala Gönder V2", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EyImzalaGonderSonuc ImzalaGonderV2(List<string> Kime, List<string> CC, List<string> Bcc, string Konu, string Icerik, EYazismaIcerikTur IcerikTuru, EYazismaPaketTur PaketTur, string PaketTurId, string SmartCardPIN, List<Ek> EklerAdresi, OzetAlg ozetAlg, int SmartCardIndex = 0, int CertificateIndex = 0)
		{
			EyLog.Log("Imzala Gönder V2", "Başladı", KepHesabi.kepHesap);
			EYazismaImzalaGonderV2 eYazismaImzalaGonderV = new EYazismaImzalaGonderV2();
			EyImzalaGonderSonuc result = eYazismaImzalaGonderV.ImzalaGonder(WebServiceClient, KepHesabi, SmartCardPIN, Kime, CC, Bcc, Konu, Icerik, IcerikTuru, EklerAdresi, PaketTur, PaketTurId, ozetAlg, SmartCardIndex, CertificateIndex, Config);
			eYazismaImzalaGonderV.CloseSmartCardSession();
			EyLog.Log("Imzala Gönder V2", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EySmimeParcalaSonuc SmimeParcala(string SmimeKepIletisiBase64)
		{
			EyLog.Log("S/Mime Parçala", "Başladı", KepHesabi.kepHesap);
			EySmimeParcalaSonuc result = EYazismaSmimeParser.SmimeParcala(SmimeKepIletisiBase64);
			EyLog.Log("S/Mime Parçala", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public EySmimeParcalaSonuc SmimeParcala(byte[] SmimeKepIletisi)
		{
			EyLog.Log("S/Mime Parçala", "Başladı", KepHesabi.kepHesap);
			EySmimeParcalaSonuc result = EYazismaSmimeParser.SmimeParcala(SmimeKepIletisi);
			EyLog.Log("S/Mime Parçala", "Bitti", KepHesabi.kepHesap);
			LogUtilities.Write(Config.EyazismaApiLog, Config.EyLoglari, Config.LogConfigFile);
			return result;
		}

		public List<EYazismaCertificate> GetCertificateList()
		{
			List<EYazismaCertificate> list = new List<EYazismaCertificate>();
			List<string> terminalNames = SmartCardUtilities.GetTerminalNames();
			for (int i = 0; i < terminalNames.Count; i++)
			{
				try
				{
					SmartCardReader smartCardReader = new SmartCardReader(i);
					try
					{
						smartCardReader.Initialize();
					}
					catch (Exception exception)
					{
						LOG.Warn("Akıllı Kart ile Bağlantı Kurulamadı.", exception);
						goto end_IL_0014;
					}
					List<X509Certificate> certList = smartCardReader.CertList;
					EYazismaSmartCard eYazismaSmartCard = new EYazismaSmartCard(i, terminalNames[i]);
					for (int j = 0; j < certList.Count; j++)
					{
						EYazismaCertificate item = new EYazismaCertificate(j, certList[j], eYazismaSmartCard);
						eYazismaSmartCard.CertificateList.Add(item);
						list.Add(item);
					}
					end_IL_0014:;
				}
				catch (Exception exception2)
				{
					LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				}
			}
			return list;
		}

		public List<EYazismaSmartCard> GetSmartCardList()
		{
			List<EYazismaSmartCard> list = new List<EYazismaSmartCard>();
			List<string> terminalNames = SmartCardUtilities.GetTerminalNames();
			for (int i = 0; i < terminalNames.Count; i++)
			{
				try
				{
					SmartCardReader smartCardReader = new SmartCardReader(i);
					try
					{
						smartCardReader.Initialize();
					}
					catch (Exception exception)
					{
						LOG.Warn("Akıllı Kart ile Bağlantı Kurulamadı.", exception);
						goto end_IL_0014;
					}
					List<X509Certificate> certList = smartCardReader.CertList;
					EYazismaSmartCard eYazismaSmartCard = new EYazismaSmartCard(i, terminalNames[i]);
					for (int j = 0; j < certList.Count; j++)
					{
						EYazismaCertificate item = new EYazismaCertificate(j, certList[j], eYazismaSmartCard);
						eYazismaSmartCard.CertificateList.Add(item);
					}
					list.Add(eYazismaSmartCard);
					end_IL_0014:;
				}
				catch (Exception exception2)
				{
					LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				}
			}
			return list;
		}
	}
}
