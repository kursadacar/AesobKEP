using System;
using System.Collections.Generic;
using System.Reflection;
using Tr.Com.Eimza.Cades.Tools;
using Tr.Com.Eimza.EYazisma.Utilities;
using Tr.Com.Eimza.Log4Net;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ess;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Nist;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Oiw;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;
using Tr.Com.Eimza.SmartCard;

namespace Tr.Com.Eimza.EYazisma
{
    internal class EYazismaHashImzala
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly object Sync = new object();

		private SmartCardReader reader;

		private X509Certificate signerCert;

		internal EyHashImzalaSonuc HashImzala(string mimeHashStr, string smartCardPasswd, OzetAlg ozetAlg, int SelectedSmartCardIndex, int SelectedCertificateIndex)
		{
			EyHashImzalaSonuc eyHashImzalaSonuc = new EyHashImzalaSonuc();
			if (string.IsNullOrEmpty(mimeHashStr))
			{
				EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Değeri Boş Bırakılamaz.");
				eyHashImzalaSonuc.Durum = 161;
				eyHashImzalaSonuc.HataAciklama = "Hash Değeri Boş Bırakılamaz.";
				return eyHashImzalaSonuc;
			}
			byte[] array = null;
			try
			{
				array = Convert.FromBase64String(mimeHashStr);
			}
			catch (Exception ex)
			{
				LOG.Error("Hash Değeri Base64 Kodlama Biçimine Uygun Olmayan Karakterler İçermektedir.", ex);
				eyHashImzalaSonuc.Durum = 161;
				eyHashImzalaSonuc.HataAciklama = "Hash Değeri Base64 Kodlama Biçimine Uygun Olmayan Karakterler İçermektedir.\nC# Exception : " + ex.Message;
				return eyHashImzalaSonuc;
			}
			return HashImzala(array, smartCardPasswd, ozetAlg, SelectedSmartCardIndex, SelectedCertificateIndex);
		}

		internal EyHashImzalaSonuc HashImzala(string mimeHashStr, string smartCardPasswd, int SelectedSmartCardIndex, int SelectedCertificateIndex)
		{
			EyHashImzalaSonuc eyHashImzalaSonuc = new EyHashImzalaSonuc();
			if (string.IsNullOrEmpty(mimeHashStr))
			{
				EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Değeri Boş Bırakılamaz.");
				eyHashImzalaSonuc.Durum = 161;
				eyHashImzalaSonuc.HataAciklama = "Hash Değeri Boş Bırakılamaz.";
				return eyHashImzalaSonuc;
			}
			byte[] array = null;
			try
			{
				array = Convert.FromBase64String(mimeHashStr);
			}
			catch (Exception ex)
			{
				LOG.Error("Hash Değeri Base64 Kodlama Biçimine Uygun Olmayan Karakterler İçermektedir.", ex);
				eyHashImzalaSonuc.Durum = 161;
				eyHashImzalaSonuc.HataAciklama = "Hash Değeri Base64 Kodlama Biçimine Uygun Olmayan Karakterler İçermektedir.\nC# Exception : " + ex.Message;
				return eyHashImzalaSonuc;
			}
			return HashImzala(array, smartCardPasswd, SelectedSmartCardIndex, SelectedCertificateIndex);
		}

		internal EyHashImzalaSonuc HashImzala(byte[] mimeHash, string smartCardPasswd, OzetAlg ozetAlg, int SelectedSmartCardIndex, int SelectedCertificateIndex)
		{
			EyHashImzalaSonuc eyHashImzalaSonuc = new EyHashImzalaSonuc();
			if (mimeHash == null)
			{
				EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Değeri Boş Bırakılamaz.");
				eyHashImzalaSonuc.Durum = 161;
				eyHashImzalaSonuc.HataAciklama = "Hash Değeri Boş Bırakılamaz.";
				return eyHashImzalaSonuc;
			}
			if (mimeHash.Length == 20)
			{
				EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Imzalama fonksiyonu SHA-1 ile hesaplanmış özet değerlerini imzalamamaktadır. Çünkü SHA-1 standartlara göre artık güvenilir bir özet algoritması olarak kabul edilmemektedir...!!!");
				eyHashImzalaSonuc = new EyHashImzalaSonuc();
				eyHashImzalaSonuc.Durum = 161;
				eyHashImzalaSonuc.HataAciklama = "Hash Imzalama fonksiyonu SHA-1 ile hesaplanmış özet değerlerini imzalamamaktadır. Çünkü SHA-1 standartlara göre artık güvenilir bir özet algoritması olarak kabul edilmemektedir...!!!";
				return eyHashImzalaSonuc;
			}
			switch (ozetAlg)
			{
			case OzetAlg.SHA256:
				if (mimeHash.Length != 32)
				{
					EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Verisi Verilen Hash Algoritması ile Hesaplanmamış.");
					eyHashImzalaSonuc = new EyHashImzalaSonuc();
					eyHashImzalaSonuc.Durum = 161;
					eyHashImzalaSonuc.HataAciklama = "Hash Verisi Verilen Hash Algoritması ile Hesaplanmamış.";
					return eyHashImzalaSonuc;
				}
				break;
			case OzetAlg.SHA512:
				if (mimeHash.Length != 64)
				{
					EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Verisi Verilen Hash Algoritması ile Hesaplanmamış.");
					eyHashImzalaSonuc = new EyHashImzalaSonuc();
					eyHashImzalaSonuc.Durum = 161;
					eyHashImzalaSonuc.HataAciklama = "Hash Verisi Verilen Hash Algoritması ile Hesaplanmamış.";
					return eyHashImzalaSonuc;
				}
				break;
			}
			try
			{
				if (smartCardPasswd != null)
				{
					lock (Sync)
					{
						reader = new SmartCardReader(smartCardPasswd, SelectedSmartCardIndex);
						try
						{
							reader.Initialize();
						}
						catch (Exception ex)
						{
							eyHashImzalaSonuc = new EyHashImzalaSonuc();
							eyHashImzalaSonuc.Durum = 161;
							eyHashImzalaSonuc.HataAciklama = "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.\nC# Exception : " + ex.Message;
							LOG.Error(ex);
							EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.C# Exception : " + ex.Message);
							return eyHashImzalaSonuc;
						}
						try
						{
							reader.FindToken(SelectedCertificateIndex);
						}
						catch (Exception ex2)
						{
							eyHashImzalaSonuc = new EyHashImzalaSonuc();
							eyHashImzalaSonuc.Durum = 161;
							eyHashImzalaSonuc.HataAciklama = "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.\nC# Exception : " + ex2.Message;
							LOG.Error(ex2);
							EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.C# Exception : " + ex2.Message);
							return eyHashImzalaSonuc;
						}
						try
						{
							string text = "";
							string digestOID = "";
							reader.SmartCardParams.SignatureAlgorithm = reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex].SigAlgOid;
							reader.SmartCardParams.HashToBeSigned = mimeHash;
							signerCert = reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex];
							DefaultSignedAttributeTableGenerator defaultSignedAttributeTable = GetDefaultSignedAttributeTable();
							if (defaultSignedAttributeTable == null)
							{
								throw new Exception("Imzalama Işlemi Başarısız Oldu. Imzalı Özellikler Oluşturulamadı.");
							}
							switch (ozetAlg)
							{
							case OzetAlg.SHA256:
								text = "1.2.840.113549.1.1.11";
								digestOID = "2.16.840.1.101.3.4.2.1";
								break;
							case OzetAlg.SHA512:
								text = "1.2.840.113549.1.1.13";
								digestOID = "2.16.840.1.101.3.4.2.3";
								break;
							}
							CmsSignedDataGenerator cmsSignedDataGenerator = new CmsSignedDataGenerator();
							cmsSignedDataGenerator.AddSigner(new NullPrivateKey(), signerCert, text, digestOID, defaultSignedAttributeTable, null);
							IX509Store certStore = CertificateUtils.CreateCertificateStore(signerCert);
							cmsSignedDataGenerator.AddCertificates(certStore);
							SmartCardSigner signerProvider = new SmartCardSigner(reader, text);
							cmsSignedDataGenerator.SignerProvider = signerProvider;
							CmsProcessableByteArray cmsProcessableByteArray = new CmsProcessableByteArray(reader.SmartCardParams.HashToBeSigned);
							CmsSignedData cmsSignedData = cmsSignedDataGenerator.Generate(cmsProcessableByteArray);
							cmsSignedData = new CmsSignedData(cmsProcessableByteArray, cmsSignedData.GetEncoded());
							eyHashImzalaSonuc.Durum = 0;
							eyHashImzalaSonuc.HataAciklama = "Hash Imzalama Başarılı";
							eyHashImzalaSonuc.P7sFile = cmsSignedData.GetEncoded();
							EyLog.Log("Hash Imzala", EyLogTuru.BILGI, "Durum: 0", "Sonuç: Hash Imzalama Başarılı");
							return eyHashImzalaSonuc;
						}
						catch (Exception exception)
						{
							LOG.Error("Imzalama işlemi Gerçekleştirilirken Hata Meydana Geldi.", exception);
							return null;
						}
					}
				}
				eyHashImzalaSonuc = new EyHashImzalaSonuc();
				eyHashImzalaSonuc.Durum = 161;
				eyHashImzalaSonuc.HataAciklama = "Akıllı Kartın Şifresini Boş Bırakamazsınız";
				EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Akıllı Kartın Şifresini Boş Bırakamazsınız");
				return eyHashImzalaSonuc;
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}

		internal EyHashImzalaSonuc HashImzala(byte[] mimeHash, string smartCardPasswd, int SelectedSmartCardIndex, int SelectedCertificateIndex)
		{
			EyHashImzalaSonuc eyHashImzalaSonuc = new EyHashImzalaSonuc();
			if (mimeHash == null)
			{
				EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Değeri Boş Bırakılamaz.");
				eyHashImzalaSonuc.Durum = 161;
				eyHashImzalaSonuc.HataAciklama = "Hash Değeri Boş Bırakılamaz.";
				return eyHashImzalaSonuc;
			}
			if (mimeHash.Length == 20)
			{
				EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Imzalama fonksiyonu SHA-1 ile hesaplanmış özet değerlerini imzalamamaktadır. Çünkü SHA-1 standartlara göre artık güvenilir bir özet algoritması olarak kabul edilmemektedir...!!!");
				eyHashImzalaSonuc = new EyHashImzalaSonuc();
				eyHashImzalaSonuc.Durum = 161;
				eyHashImzalaSonuc.HataAciklama = "Hash Imzalama fonksiyonu SHA-1 ile hesaplanmış özet değerlerini imzalamamaktadır. Çünkü SHA-1 standartlara göre artık güvenilir bir özet algoritması olarak kabul edilmemektedir...!!!";
				return eyHashImzalaSonuc;
			}
			try
			{
				if (smartCardPasswd != null)
				{
					lock (Sync)
					{
						reader = new SmartCardReader(smartCardPasswd, SelectedSmartCardIndex);
						try
						{
							reader.Initialize();
						}
						catch (Exception ex)
						{
							eyHashImzalaSonuc = new EyHashImzalaSonuc();
							eyHashImzalaSonuc.Durum = 161;
							eyHashImzalaSonuc.HataAciklama = "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.\nC# Exception : " + ex.Message;
							LOG.Error(ex);
							EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Akıllı Kart ile Bağlantı Kurulamadı. Akıllı Kartın Takılı Olduğundan ve Driver'larının Kurulu Olduğundan Emin Olunuz.C# Exception : " + ex.Message);
							return eyHashImzalaSonuc;
						}
						try
						{
							reader.FindToken(SelectedCertificateIndex);
						}
						catch (Exception ex2)
						{
							eyHashImzalaSonuc = new EyHashImzalaSonuc();
							eyHashImzalaSonuc.Durum = 161;
							eyHashImzalaSonuc.HataAciklama = "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.\nC# Exception : " + ex2.Message;
							LOG.Error(ex2);
							EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Private Key Karttan Çekilemedi. Lütfen Akıllı Kartınızın PIN'ini Kontrol Ediniz.C# Exception : " + ex2.Message);
							return eyHashImzalaSonuc;
						}
						try
						{
							string text = "";
							string digestOID = "";
							reader.SmartCardParams.SignatureAlgorithm = reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex].SigAlgOid;
							reader.SmartCardParams.HashToBeSigned = mimeHash;
							signerCert = reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex];
							DefaultSignedAttributeTableGenerator defaultSignedAttributeTable = GetDefaultSignedAttributeTable();
							if (defaultSignedAttributeTable == null)
							{
								throw new Exception("Imzalama Işlemi Başarısız Oldu. Imzalı Özellikler Oluşturulamadı.");
							}
							switch (signerCert.SigAlgOid)
							{
							case "1.2.840.113549.1.1.5":
								if (mimeHash.Length != 32)
								{
									EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Verisi, Akıllı Karttaki Sertifikanın Özet Algoritması ile Hesaplanmamış. Özet Değerinizin SHA-256 ile Hesaplanmış Olması Gerekmektedir...");
									eyHashImzalaSonuc = new EyHashImzalaSonuc();
									eyHashImzalaSonuc.Durum = 161;
									eyHashImzalaSonuc.HataAciklama = "Hash Verisi, Akıllı Karttaki Sertifikanın Özet Algoritması ile Hesaplanmamış.";
									return eyHashImzalaSonuc;
								}
								text = "1.2.840.113549.1.1.11";
								digestOID = "2.16.840.1.101.3.4.2.1";
								break;
							case "1.2.840.113549.1.1.11":
								if (mimeHash.Length != 32)
								{
									EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Verisi, Akıllı Karttaki Sertifikanın Özet Algoritması ile Hesaplanmamış. Özet Değerinizin SHA-256 ile Hesaplanmış Olması Gerekmektedir...");
									eyHashImzalaSonuc = new EyHashImzalaSonuc();
									eyHashImzalaSonuc.Durum = 161;
									eyHashImzalaSonuc.HataAciklama = "Hash Verisi, Akıllı Karttaki Sertifikanın Özet Algoritması ile Hesaplanmamış.";
									return eyHashImzalaSonuc;
								}
								text = "1.2.840.113549.1.1.11";
								digestOID = "2.16.840.1.101.3.4.2.1";
								break;
							case "1.2.840.113549.1.1.12":
								if (mimeHash.Length != 48)
								{
									EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Verisi, Akıllı Karttaki Sertifikanın Özet Algoritması ile Hesaplanmamış. Özet Değerinizin SHA-384 ile Hesaplanmış Olması Gerekmektedir...");
									eyHashImzalaSonuc = new EyHashImzalaSonuc();
									eyHashImzalaSonuc.Durum = 161;
									eyHashImzalaSonuc.HataAciklama = "Hash Verisi, Akıllı Karttaki Sertifikanın Özet Algoritması ile Hesaplanmamış.";
									return eyHashImzalaSonuc;
								}
								text = "1.2.840.113549.1.1.12";
								digestOID = "2.16.840.1.101.3.4.2.2";
								break;
							case "1.2.840.113549.1.1.13":
								if (mimeHash.Length != 64)
								{
									EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Verisi, Akıllı Karttaki Sertifikanın Özet Algoritması ile Hesaplanmamış. Özet Değerinizin SHA-512 ile Hesaplanmış Olması Gerekmektedir...");
									eyHashImzalaSonuc = new EyHashImzalaSonuc();
									eyHashImzalaSonuc.Durum = 161;
									eyHashImzalaSonuc.HataAciklama = "Hash Verisi, Akıllı Karttaki Sertifikanın Özet Algoritması ile Hesaplanmamış.";
									return eyHashImzalaSonuc;
								}
								text = "1.2.840.113549.1.1.13";
								digestOID = "2.16.840.1.101.3.4.2.3";
								break;
							case "1.2.840.10045.4.3.3":
								if (mimeHash.Length != 32)
								{
									EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Hash Verisi, Özet Değerinizin SHA-256 ile Hesaplanmış Olması Gerekmektedir...");
									eyHashImzalaSonuc = new EyHashImzalaSonuc();
									eyHashImzalaSonuc.Durum = 161;
									eyHashImzalaSonuc.HataAciklama = "Hash Verisi, Akıllı Karttaki Sertifikanın Özet Algoritması ile Hesaplanmamış.";
									return eyHashImzalaSonuc;
								}
								text = "1.2.840.113549.1.1.11";
								digestOID = "2.16.840.1.101.3.4.2.1";
								break;
							}
							CmsSignedDataGenerator cmsSignedDataGenerator = new CmsSignedDataGenerator();
							cmsSignedDataGenerator.AddSigner(new NullPrivateKey(), signerCert, text, digestOID, defaultSignedAttributeTable, null);
							IX509Store certStore = CertificateUtils.CreateCertificateStore(signerCert);
							cmsSignedDataGenerator.AddCertificates(certStore);
							SmartCardSigner signerProvider = new SmartCardSigner(reader, text);
							cmsSignedDataGenerator.SignerProvider = signerProvider;
							CmsProcessableByteArray cmsProcessableByteArray = new CmsProcessableByteArray(reader.SmartCardParams.HashToBeSigned);
							CmsSignedData cmsSignedData = cmsSignedDataGenerator.Generate(cmsProcessableByteArray);
							cmsSignedData = new CmsSignedData(cmsProcessableByteArray, cmsSignedData.GetEncoded());
							eyHashImzalaSonuc.Durum = 0;
							eyHashImzalaSonuc.HataAciklama = "Hash Imzalama Başarılı";
							eyHashImzalaSonuc.P7sFile = cmsSignedData.GetEncoded();
							EyLog.Log("Hash Imzala", EyLogTuru.BILGI, "Durum: 0", "Sonuç: Hash Imzalama Başarılı");
							return eyHashImzalaSonuc;
						}
						catch (Exception exception)
						{
							LOG.Error("Imzalama işlemi Gerçekleştirilirken Hata Meydana Geldi.", exception);
							return null;
						}
					}
				}
				eyHashImzalaSonuc = new EyHashImzalaSonuc();
				eyHashImzalaSonuc.Durum = 161;
				eyHashImzalaSonuc.HataAciklama = "Akıllı Kartın Şifresini Boş Bırakamazsınız";
				EyLog.Log("Hash Imzala", EyLogTuru.HATA, "Akıllı Kartın Şifresini Boş Bırakamazsınız");
				return eyHashImzalaSonuc;
			}
			catch (Exception exception2)
			{
				LOG.Error("Tahmin Edilemeyen Bir Hata Meydana Geldi. Hata Ayrıntıları için EYazismaApi.log Dosyasını Kontrol Ediniz.", exception2);
				return null;
			}
		}

		internal DefaultSignedAttributeTableGenerator GetDefaultSignedAttributeTable()
		{
			Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttributeTable = GetSignedAttributeTable();
			if (signedAttributeTable == null)
			{
				return null;
			}
			return new DefaultSignedAttributeTableGenerator(signedAttributeTable);
		}

		internal Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable GetSignedAttributeTable()
		{
			Dictionary<DerObjectIdentifier, Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute> signedAttribute = GetSignedAttribute();
			if (signedAttribute == null)
			{
				return null;
			}
			return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable(signedAttribute);
		}

		internal Dictionary<DerObjectIdentifier, Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute> GetSignedAttribute()
		{
			Dictionary<DerObjectIdentifier, Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute> dictionary = new Dictionary<DerObjectIdentifier, Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute>();
			Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = MakeSigningCertificateAttribute();
			if (attribute == null)
			{
				return null;
			}
			dictionary.Add(attribute.AttrType, attribute);
			return dictionary;
		}

		private Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute MakeSigningCertificateAttribute()
		{
			try
			{
				byte[] byteToByteHash;
				if (signerCert.SigAlgName.Equals("SHA-1withRSA"))
				{
					byteToByteHash = Tr.Com.Eimza.Cades.Tools.Utilities.GetByteToByteHash(signerCert.GetEncoded(), DigestUtilities.GetDigest(OiwObjectIdentifiers.IdSha1));
					DerSet attrValues = new DerSet(new SigningCertificate(new EssCertID(byteToByteHash)));
					return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute(PkcsObjectIdentifiers.IdAASigningCertificate, attrValues);
				}
				byteToByteHash = Tr.Com.Eimza.Cades.Tools.Utilities.GetByteToByteHash(signerCert.GetEncoded(), DigestUtilities.GetDigest(NistObjectIdentifiers.IdSha256));
				IssuerSerial issuerSerial = new IssuerSerial(new GeneralNames(new GeneralName(signerCert.IssuerDN)), new DerInteger(signerCert.SerialNumber));
				EssCertIDv2 essCertIDv = new EssCertIDv2(new AlgorithmIdentifier(NistObjectIdentifiers.IdSha256.Id), byteToByteHash, issuerSerial);
				DerSet attrValues2 = new DerSet(new SigningCertificateV2(new EssCertIDv2[1] { essCertIDv }));
				return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute(PkcsObjectIdentifiers.IdAASigningCertificateV2, attrValues2);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public void CloseSmartCardSession()
		{
			try
			{
				if (reader == null)
				{
					GC.Collect();
				}
				else if (reader.SmartCardParams.SmartCard.Session != null && reader.SmartCardParams.SmartCard.Slot != null)
				{
					reader.CloseSession();
				}
			}
			catch (Exception)
			{
				GC.Collect();
			}
		}
	}
}
