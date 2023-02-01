using System;
using System.Collections.Generic;
using System.Reflection;
using Tr.Com.Eimza.Log4Net;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.H;
using Tr.Com.Eimza.SmartCard.Tools;

namespace Tr.Com.Eimza.SmartCard
{
	internal class SmartCardReader
	{
		private SmartCardParameters SignerParameters;

		private List<X509Certificate> certList = new List<X509Certificate>();

		private List<ObjectHandle> privateList = new List<ObjectHandle>();

		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public SmartCardParameters SmartCardParams
		{
			get
			{
				return SignerParameters;
			}
			set
			{
				SignerParameters = value;
			}
		}

		public List<X509Certificate> CertList
		{
			get
			{
				return certList;
			}
			set
			{
				certList = value;
			}
		}

		public SmartCardReader(string PIN, int SelectedSmartCard = 0)
		{
			SignerParameters = new SmartCardParameters();
			SignerParameters.SmartCard = new SmartCard();
			SignerParameters.SelectedSmartCard = SelectedSmartCard;
			SignerParameters.SmartCard.SmartCardPIN = PIN;
		}

		public SmartCardReader(int SelectedSmartCard = 0)
		{
			SignerParameters = new SmartCardParameters();
			SignerParameters.SmartCard = new SmartCard();
			SignerParameters.SelectedSmartCard = SelectedSmartCard;
		}

		public void SetSmartCardPIN(string PIN)
		{
			SignerParameters.SmartCard.SmartCardPIN = PIN;
		}

		public void Initialize()
		{
			try
			{
				string[] terminalList = WinsCard.GetTerminalList();
				if (terminalList.Length == 0)
				{
					throw new Exception("Sisteme Takılı Akıllı Kart Bulunamadı.");
				}
				if (terminalList.Length < SignerParameters.SelectedSmartCard + 1)
				{
					throw new Exception("Seçili Akıllı Kart Index'i Yanlış. Lütfen Akıllı Kartın Index Değerini Kontrol Ediniz.");
				}
				string text = terminalList[SignerParameters.SelectedSmartCard];
				SignerParameters.SmartCardType = SmartCardUtilities.GetSmartCardType(text);
				if (SignerParameters.SmartCardType == null)
				{
					throw new Exception("Akıllı Kart Tanınamadı. Kartın ATR Değerinin smartcard-config.xml Dosyasında Tanımlandığından Emin Olunuz.");
				}
				SignerParameters.SelectedCertIndex = 0;
				SignerParameters.SmartCard.Pkcs11Module = new Tr.Com.Eimza.Pkcs11.H.Pkcs11(SignerParameters.SmartCardType.LibraryName, false);
				SignerParameters.SmartCard.InitializedDll = SignerParameters.SmartCardType.LibraryName;
				SignerParameters.SmartCard.Slot = SmartCardUtilities.GetSlot(SignerParameters.SmartCardType, text);
				SignerParameters.SmartCard.SlotInfo = SignerParameters.SmartCard.Slot.GetSlotInfo();
				SignerParameters.SmartCard.TokenInfo = SignerParameters.SmartCard.Slot.GetTokenInfo();
				SignerParameters.SmartCard.LibraryInfo = SignerParameters.SmartCard.Pkcs11Module.GetInfo();
				FindAllCertificate();
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("CKR_CRYPTOKI_ALREADY_INITIALIZED"))
				{
					SignerParameters.SmartCard.Pkcs11Module.Dispose();
					throw new Exception("Akıllı Kart ile Zaten Bağlantı Kurulmuş.");
				}
				if (ex.Message.Contains("Could not load module."))
				{
					throw new Exception("Akıllı Kart'a ait DLL Dosyası Yüklenemedi.");
				}
				throw ex;
			}
		}

		public void FindToken(int SelectedCertificateIndex)
		{
			try
			{
				SignerParameters.SelectedCertIndex = SelectedCertificateIndex;
				LoadPrivateKeys();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void FindAllCertificate()
		{
			try
			{
				Session session = SignerParameters.SmartCard.Slot.OpenSession(true);
				List<ObjectAttribute> list = new List<ObjectAttribute>();
				list.Add(new ObjectAttribute(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE));
				list.Add(new ObjectAttribute(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509));
				session.FindObjectsInit(list);
				List<ObjectHandle> list2 = session.FindObjects(20);
				for (int i = 0; i < list2.Count; i++)
				{
					List<CKA> list3 = new List<CKA>();
					list3.Add(CKA.CKA_VALUE);
					List<ObjectAttribute> attributeValue = session.GetAttributeValue(list2[i], list3);
					X509CertificateParser x509CertificateParser = new X509CertificateParser();
					byte[] valueAsByteArray = attributeValue[0].GetValueAsByteArray();
					X509Certificate x509Certificate = x509CertificateParser.ReadCertificate(valueAsByteArray);
					bool[] keyUsage = x509Certificate.GetKeyUsage();
					if (keyUsage != null && keyUsage.Length > 2 && keyUsage[1])
					{
						certList.Add(x509Certificate);
					}
				}
				if (certList.Count == 0)
				{
					throw new Exception("Akıllı Kartta Sertifika Bulunamadı.");
				}
				SignerParameters.SmartCard.TokenCertList = certList;
				session.FindObjectsFinal();
			}
			catch (Exception ex)
			{
				if (!ex.Message.Contains("Akıllı Kartta Sertifika Bulunamadı."))
				{
					throw ex;
				}
				throw new Exception("Akıllı Karttan Sertifikalar Okunurken Hata Meydana Geldi.", ex);
			}
		}

		public void LoadPrivateKeys()
		{
			try
			{
				SignerParameters.SmartCard.Session = SignerParameters.SmartCard.Slot.OpenSession(false);
				try
				{
					SignerParameters.SmartCard.Session.Login(CKU.CKU_USER, SignerParameters.SmartCard.SmartCardPIN);
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("CKR_PIN_INCORRECT"))
					{
						throw new Exception("Akıllı Kartın PIN'i Yanlış.");
					}
					if (ex.Message.Contains("CKR_PIN_INVALID"))
					{
						throw new Exception("Akıllı Kartın PIN'i Geçersiz Karakter İçeriyor.");
					}
					if (ex.Message.Contains("CKR_PIN_LOCKED"))
					{
						throw new Exception("Akıllı Kartın PIN'i Bloke Olmuş.");
					}
					if (ex.Message.Contains("CKR_PIN_LEN_RANGE"))
					{
						throw new Exception("Akıllı Kartın PIN Uzunluğu Yanlış.");
					}
					throw new Exception("Akıllı Karta Giriş Yapılamadı.", ex);
				}
				List<ObjectAttribute> list = new List<ObjectAttribute>();
				list.Add(new ObjectAttribute(CKA.CKA_CLASS, 3uL));
				list.Add(new ObjectAttribute(CKA.CKA_KEY_TYPE, CKK.CKK_RSA));
				SignerParameters.SmartCard.Session.FindObjectsInit(list);
				List<ObjectHandle> list2 = SignerParameters.SmartCard.Session.FindObjects(10);
				for (int i = 0; i < list2.Count; i++)
				{
					privateList.Add(list2[i]);
				}
				if (privateList.Count == 0)
				{
					throw new Exception("Akıllı Kartta PrivateKey Bulunamadı.");
				}
				SignerParameters.SmartCard.PrivateKeyList = privateList;
				SignerParameters.SmartCard.Session.FindObjectsFinal();
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("TOKEN_NOT_RECOGNIZED"))
				{
					throw new Exception("Akıllı Kart Tanınamadı.");
				}
				if (ex2.Message.Contains("SESSION_HANDLE_INVALID"))
				{
					throw new Exception("Akıllı Karta Giriş Yapılamadı.");
				}
				if (ex2.Message.Contains("Akıllı Kartta PrivateKey Bulunamadı."))
				{
					throw ex2;
				}
				throw new Exception("Akıllı Karttan PrivateKey Okunurken Hata Meydana Geldi.", ex2);
			}
		}

		public byte[] GetDataSignature(byte[] data, DerObjectIdentifier digAlgOid)
		{
			byte[] result = null;
			try
			{
				IDigest digest = DigestUtilities.GetDigest(digAlgOid);
				byte[] array = new byte[digest.GetDigestSize()];
				digest.BlockUpdate(data, 0, data.Length);
				digest.DoFinal(array, 0);
				DigestInfo digestInfo = new DigestInfo(new AlgorithmIdentifier(digAlgOid, DerNull.Instance), array);
				Session session = SignerParameters.SmartCard.Session;
				Mechanism mechanism = null;
				mechanism = new Mechanism(CKM.CKM_RSA_PKCS);
				if (SignerParameters.SmartCard.PrivateKeyList.Count < SignerParameters.SelectedSmartCard + 1)
				{
					throw new Exception("Seçili Sertifika Index Değerine ait PrivateKey Bulunamamıştır. Lütfen Sertifika Index Değerini Kontrol Ediniz.");
				}
				result = session.Sign(mechanism, SignerParameters.SmartCard.PrivateKeyList[SignerParameters.SelectedCertIndex], digestInfo.GetDerEncoded());
				return result;
			}
			catch (Exception exception)
			{
				LOG.Error("Akıllı Kart ile imzalama Yapılırken Hata Meydana Geldi.", exception);
				return result;
			}
		}

		public byte[] GetHashSignature(byte[] data, DerObjectIdentifier digAlgOid)
		{
			byte[] result = null;
			try
			{
				DigestInfo digestInfo = new DigestInfo(new AlgorithmIdentifier(digAlgOid, DerNull.Instance), data);
				Session session = SignerParameters.SmartCard.Session;
				Mechanism mechanism = null;
				mechanism = new Mechanism(CKM.CKM_RSA_PKCS);
				if (SignerParameters.SmartCard.PrivateKeyList.Count < SignerParameters.SelectedSmartCard + 1)
				{
					throw new Exception("Seçili Sertifika Index Değerine ait PrivateKey Bulunamamıştır. Lütfen Sertifika Index Değerini Kontrol Ediniz.");
				}
				result = session.Sign(mechanism, SignerParameters.SmartCard.PrivateKeyList[SignerParameters.SelectedCertIndex], digestInfo.GetDerEncoded());
				return result;
			}
			catch (Exception exception)
			{
				LOG.Error("Akıllı Kart ile imzalama Yapılırken Hata Meydana Geldi.", exception);
				return result;
			}
		}

		public byte[] GetHashSignature(byte[] data, string digAlgOid)
		{
			return GetHashSignature(data, new DerObjectIdentifier(digAlgOid));
		}

		public void CloseSession()
		{
			try
			{
				SmartCardParams.SmartCard.Session.Logout();
				SmartCardParams.SmartCard.Session.CloseSession();
			}
			catch (Exception)
			{
			}
		}
	}
}
