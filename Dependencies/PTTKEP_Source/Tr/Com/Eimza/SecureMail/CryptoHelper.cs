using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Tr.Com.Eimza.Cades.Tools;
using Tr.Com.Eimza.EYazisma;
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
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;
using Tr.Com.Eimza.SmartCard;

namespace Tr.Com.Eimza.SecureMail
{
    internal static class CryptoHelper
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate signerCert;

		private static AsymmetricKeyParameter key;

		internal static byte[] GetSignature(string message, X509Certificate2 signingCertificate)
		{
			signerCert = DotNetUtilities.FromX509Certificate(signingCertificate);
			key = CertificateUtils.GetPrivateKeyFromPfx("800059_testuc@test.com.tr.pfx", "800059");
			DefaultSignedAttributeTableGenerator defaultSignedAttributeTable = GetDefaultSignedAttributeTable();
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			string encryptionOID = "1.2.840.113549.1.1.11";
			CmsSignedDataGenerator cmsSignedDataGenerator = new CmsSignedDataGenerator();
			cmsSignedDataGenerator.AddSigner(key, signerCert, encryptionOID, CmsSignedGenerator.DigestSha256, defaultSignedAttributeTable, null);
			IX509Store certStore = CertificateUtils.CreateCertificateStore(signerCert);
			cmsSignedDataGenerator.AddCertificates(certStore);
			CmsProcessableByteArray cmsProcessableByteArray = new CmsProcessableByteArray(bytes);
			CmsSignedData cmsSignedData = cmsSignedDataGenerator.Generate(cmsProcessableByteArray);
			cmsSignedData = new CmsSignedData(cmsProcessableByteArray, cmsSignedData.GetEncoded());
			return cmsSignedData.GetEncoded();
		}

		internal static byte[] GetSignatureWithSmartCard(string message, SmartCardReader reader)
		{
			try
			{
				string encryptionOID = "";
				string digestOID = "";
				byte[] bytes = Encoding.UTF8.GetBytes(message);
				reader.SmartCardParams.SignatureAlgorithm = reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex].SigAlgOid;
				signerCert = reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex];
				DefaultSignedAttributeTableGenerator defaultSignedAttributeTable = GetDefaultSignedAttributeTable();
				if (defaultSignedAttributeTable == null)
				{
					throw new Exception("Imzalama Işlemi Başarısız Oldu. Imzalı Özellikler Oluşturulamadı.");
				}
				switch (signerCert.SigAlgOid)
				{
				case "1.2.840.113549.1.1.5":
					encryptionOID = "1.2.840.113549.1.1.11";
					digestOID = "2.16.840.1.101.3.4.2.1";
					break;
				case "1.2.840.113549.1.1.11":
					encryptionOID = "1.2.840.113549.1.1.11";
					digestOID = "2.16.840.1.101.3.4.2.1";
					break;
				case "1.2.840.113549.1.1.12":
					encryptionOID = "1.2.840.113549.1.1.12";
					digestOID = "2.16.840.1.101.3.4.2.2";
					break;
				case "1.2.840.113549.1.1.13":
					encryptionOID = "1.2.840.113549.1.1.13";
					digestOID = "2.16.840.1.101.3.4.2.3";
					break;
				}
				CmsSignedDataGenerator cmsSignedDataGenerator = new CmsSignedDataGenerator();
				cmsSignedDataGenerator.AddSigner(new NullPrivateKey(), signerCert, encryptionOID, digestOID, defaultSignedAttributeTable, null);
				IX509Store certStore = CertificateUtils.CreateCertificateStore(signerCert);
				SmartCardSigner signerProvider = new SmartCardSigner(reader);
				cmsSignedDataGenerator.SignerProvider = signerProvider;
				cmsSignedDataGenerator.AddCertificates(certStore);
				CmsProcessableByteArray cmsProcessableByteArray = new CmsProcessableByteArray(bytes);
				CmsSignedData cmsSignedData = cmsSignedDataGenerator.Generate(cmsProcessableByteArray);
				cmsSignedData = new CmsSignedData(cmsProcessableByteArray, cmsSignedData.GetEncoded());
				return cmsSignedData.GetEncoded();
			}
			catch (Exception exception)
			{
				LOG.Error("Smime Imzalama işlemi Gerçekleştirilirken Hata Meydana Geldi.", exception);
				return null;
			}
		}

		internal static byte[] GetSignatureWithSmartCard(string message, SmartCardReader reader, OzetAlg ozetAlg)
		{
			try
			{
				string encryptionOID = "";
				string digestOID = "";
				byte[] bytes = Encoding.UTF8.GetBytes(message);
				reader.SmartCardParams.SignatureAlgorithm = reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex].SigAlgOid;
				signerCert = reader.SmartCardParams.SmartCard.TokenCertList[reader.SmartCardParams.SelectedCertIndex];
				DefaultSignedAttributeTableGenerator defaultSignedAttributeTable = GetDefaultSignedAttributeTable();
				if (defaultSignedAttributeTable == null)
				{
					throw new Exception("Imzalama Işlemi Başarısız Oldu. Imzalı Özellikler Oluşturulamadı.");
				}
				switch (ozetAlg)
				{
				case OzetAlg.SHA256:
					encryptionOID = "1.2.840.113549.1.1.11";
					digestOID = "2.16.840.1.101.3.4.2.1";
					break;
				case OzetAlg.SHA512:
					encryptionOID = "1.2.840.113549.1.1.13";
					digestOID = "2.16.840.1.101.3.4.2.3";
					break;
				}
				CmsSignedDataGenerator cmsSignedDataGenerator = new CmsSignedDataGenerator();
				cmsSignedDataGenerator.AddSigner(new NullPrivateKey(), signerCert, encryptionOID, digestOID, defaultSignedAttributeTable, null);
				IX509Store certStore = CertificateUtils.CreateCertificateStore(signerCert);
				SmartCardSigner signerProvider = new SmartCardSigner(reader);
				cmsSignedDataGenerator.SignerProvider = signerProvider;
				cmsSignedDataGenerator.AddCertificates(certStore);
				CmsProcessableByteArray cmsProcessableByteArray = new CmsProcessableByteArray(bytes);
				CmsSignedData cmsSignedData = cmsSignedDataGenerator.Generate(cmsProcessableByteArray);
				cmsSignedData = new CmsSignedData(cmsProcessableByteArray, cmsSignedData.GetEncoded());
				return cmsSignedData.GetEncoded();
			}
			catch (Exception exception)
			{
				LOG.Error("Smime Imzalama işlemi Gerçekleştirilirken Hata Meydana Geldi.", exception);
				return null;
			}
		}

		private static DefaultSignedAttributeTableGenerator GetDefaultSignedAttributeTable()
		{
			Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttributeTable = GetSignedAttributeTable();
			if (signedAttributeTable == null)
			{
				return null;
			}
			return new DefaultSignedAttributeTableGenerator(signedAttributeTable);
		}

		private static Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable GetSignedAttributeTable()
		{
			Dictionary<DerObjectIdentifier, Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute> signedAttribute = GetSignedAttribute();
			if (signedAttribute == null)
			{
				return null;
			}
			return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.AttributeTable(signedAttribute);
		}

		private static Dictionary<DerObjectIdentifier, Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute> GetSignedAttribute()
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

		private static Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute MakeSigningCertificateAttribute()
		{
			Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = null;
			try
			{
				byte[] array = null;
				if (signerCert.SigAlgName.Equals("SHA-1withRSA"))
				{
					array = Utilities.GetByteToByteHash(signerCert.GetEncoded(), DigestUtilities.GetDigest(OiwObjectIdentifiers.IdSha1));
					new IssuerSerial(new GeneralNames(new GeneralName(signerCert.IssuerDN)), new DerInteger(signerCert.SerialNumber));
					DerSet attrValues = new DerSet(new SigningCertificate(new EssCertID(array)));
					return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute(PkcsObjectIdentifiers.IdAASigningCertificate, attrValues);
				}
				array = Utilities.GetByteToByteHash(signerCert.GetEncoded(), DigestUtilities.GetDigest(NistObjectIdentifiers.IdSha256));
				IssuerSerial issuerSerial = new IssuerSerial(new GeneralNames(new GeneralName(signerCert.IssuerDN)), new DerInteger(signerCert.SerialNumber));
				EssCertIDv2 essCertIDv = new EssCertIDv2(new AlgorithmIdentifier(NistObjectIdentifiers.IdSha256.Id), array, issuerSerial);
				DerSet attrValues2 = new DerSet(new SigningCertificateV2(new EssCertIDv2[1] { essCertIDv }));
				return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute(PkcsObjectIdentifiers.IdAASigningCertificateV2, attrValues2);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
