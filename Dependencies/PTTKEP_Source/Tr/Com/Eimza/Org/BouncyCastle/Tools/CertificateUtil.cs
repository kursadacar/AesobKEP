using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
    internal static class CertificateUtil
	{
		public static bool IsSelfSigned(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate cert)
		{
			try
			{
				AsymmetricKeyParameter publicKey = cert.GetPublicKey();
				cert.Verify(publicKey);
				return true;
			}
			catch (SignatureException)
			{
				return false;
			}
			catch (InvalidKeyException)
			{
				return false;
			}
		}

		public static bool IsCA(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate cert)
		{
			BasicConstraints instance;
			using (Asn1InputStream asn1InputStream = new Asn1InputStream(((DerOctetString)cert.GetExtensionValue(X509Extensions.BasicConstraints)).GetOctets()))
			{
				instance = BasicConstraints.GetInstance((Asn1Sequence)asn1InputStream.ReadObject());
			}
			return instance.IsCA();
		}

		public static bool IsSelfIssued(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate cert)
		{
			return cert.CertificateStructure.Subject.Equivalent(cert.CertificateStructure.Issuer);
		}

		public static bool IsSignatureVerify(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate signerCert, Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate intermediateCert, Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate rootCert)
		{
			try
			{
				AsymmetricKeyParameter publicKey = intermediateCert.GetPublicKey();
				signerCert.Verify(publicKey);
				publicKey = rootCert.GetPublicKey();
				intermediateCert.Verify(publicKey);
				publicKey = rootCert.GetPublicKey();
				rootCert.Verify(publicKey);
				return true;
			}
			catch (SignatureException)
			{
				return false;
			}
			catch (InvalidKeyException)
			{
				return false;
			}
		}

		public static bool IsSignatureVerify(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate signerCert, Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate issuerCert)
		{
			try
			{
				AsymmetricKeyParameter publicKey = issuerCert.GetPublicKey();
				signerCert.Verify(publicKey);
				return true;
			}
			catch (SignatureException)
			{
				return false;
			}
			catch (InvalidKeyException)
			{
				return false;
			}
		}

		public static bool IsSignatureVerify(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate selfSignedCert)
		{
			try
			{
				AsymmetricKeyParameter publicKey = selfSignedCert.GetPublicKey();
				selfSignedCert.Verify(publicKey);
				return true;
			}
			catch (SignatureException)
			{
				return false;
			}
			catch (InvalidKeyException)
			{
				return false;
			}
		}

		public static bool IsSignatureVerify(AsymmetricKeyParameter publicKey, ISigner signature, byte[] SignedData, byte[] SignerSignature)
		{
			signature.Init(false, publicKey);
			signature.BlockUpdate(SignedData, 0, SignedData.Length);
			if (!signature.VerifySignature(SignerSignature))
			{
				return false;
			}
			return true;
		}

		public static string GetIssuerCertificateURL(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate certificate)
		{
			if (certificate.IsSelfIssued)
			{
				return null;
			}
			try
			{
				Asn1Object extensionValue = GetExtensionValue(certificate, X509Extensions.AuthorityInfoAccess);
				if (extensionValue == null)
				{
					return null;
				}
				AccessDescription[] accessDescriptions = AuthorityInformationAccess.GetInstance(extensionValue).GetAccessDescriptions();
				foreach (AccessDescription accessDescription in accessDescriptions)
				{
					if (accessDescription.AccessMethod.Equals(AccessDescription.IdADCAIssuers))
					{
						GeneralName accessLocation = accessDescription.AccessLocation;
						if (accessLocation.TagNo == 6)
						{
							return ((DerIA5String)((DerTaggedObject)accessLocation.ToAsn1Object()).GetObject()).GetString();
						}
					}
				}
				return null;
			}
			catch (IOException)
			{
				return null;
			}
		}

		public static string GetCRLURL(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate certificate)
		{
			try
			{
				Asn1Object extensionValue = GetExtensionValue(certificate, X509Extensions.CrlDistributionPoints);
				if (extensionValue == null)
				{
					return null;
				}
				DistributionPoint[] distributionPoints = CrlDistPoint.GetInstance(extensionValue).GetDistributionPoints();
				for (int i = 0; i < distributionPoints.Length; i++)
				{
					DistributionPointName distributionPointName = distributionPoints[i].DistributionPointName;
					if (distributionPointName.PointType != 0)
					{
						continue;
					}
					GeneralName[] names = ((GeneralNames)distributionPointName.Name).GetNames();
					foreach (GeneralName generalName in names)
					{
						if (generalName.TagNo == 6)
						{
							return DerIA5String.GetInstance((Asn1TaggedObject)generalName.ToAsn1Object(), false).GetString();
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		public static string GetOCSPURL(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate certificate)
		{
			try
			{
				Asn1Object extensionValue = GetExtensionValue(certificate, X509Extensions.AuthorityInfoAccess.Id);
				if (extensionValue == null)
				{
					return null;
				}
				AccessDescription[] accessDescriptions = AuthorityInformationAccess.GetInstance(extensionValue).GetAccessDescriptions();
				foreach (AccessDescription accessDescription in accessDescriptions)
				{
					if (accessDescription.AccessMethod.Equals(X509ObjectIdentifiers.OcspAccessMethod))
					{
						GeneralName accessLocation = accessDescription.AccessLocation;
						if (accessLocation.TagNo == 6)
						{
							return ((DerIA5String)((DerTaggedObject)accessLocation.ToAsn1Object()).GetObject()).GetString();
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		public static string GetTSAURL(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate certificate)
		{
			Asn1Object asn1Object = GetExtensionValue(certificate, new DerObjectIdentifier("1.2.840.113583.1.1.9.1"));
			if (asn1Object == null)
			{
				return null;
			}
			try
			{
				if (asn1Object is DerOctetString)
				{
					asn1Object = Asn1Object.FromByteArray(((DerOctetString)asn1Object).GetOctets());
				}
				return ToStringName(TimeStampAccessLocation.GetInstance(Asn1Sequence.GetInstance(asn1Object)).TsaURL);
			}
			catch (IOException)
			{
				return null;
			}
		}

		public static Asn1Object GetExtensionValue(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate cert, string oid)
		{
			Asn1OctetString extensionValue = cert.GetExtensionValue(new DerObjectIdentifier(oid));
			if (extensionValue == null)
			{
				return null;
			}
			byte[] derEncoded = extensionValue.GetDerEncoded();
			if (derEncoded == null)
			{
				return null;
			}
			return new Asn1InputStream(new MemoryStream(((Asn1OctetString)new Asn1InputStream(new MemoryStream(derEncoded)).ReadObject()).GetOctets())).ReadObject();
		}

		public static Asn1Object GetExtensionValue(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate cert, DerObjectIdentifier oid)
		{
			Asn1OctetString extensionValue = cert.GetExtensionValue(oid);
			if (extensionValue == null)
			{
				return null;
			}
			byte[] derEncoded = extensionValue.GetDerEncoded();
			if (derEncoded == null)
			{
				return null;
			}
			return new Asn1InputStream(new MemoryStream(((Asn1OctetString)new Asn1InputStream(new MemoryStream(derEncoded)).ReadObject()).GetOctets())).ReadObject();
		}

		public static string ToStringName(Asn1Object names)
		{
			Asn1TaggedObject obj = (Asn1TaggedObject)names;
			return Encoding.UTF8.GetString(Asn1OctetString.GetInstance(obj, false).GetOctets());
		}

		public static Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate GetIssuerCertificate(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate certificate)
		{
			try
			{
				string issuerCertificateURL = GetIssuerCertificateURL(certificate);
				if (issuerCertificateURL == null)
				{
					return null;
				}
				if (issuerCertificateURL.StartsWith("http://") || issuerCertificateURL.StartsWith("https://"))
				{
					return GetCertificate(issuerCertificateURL);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return null;
		}

		public static Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate GetCertificate(string downloadUrl)
		{
			if (downloadUrl != null)
			{
				try
				{
					return GetCertFromData(new MyWebClient().DownloadData(downloadUrl));
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			return null;
		}

		public static Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate GetCertFromData(byte[] certData)
		{
			return new Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate(certData);
		}

		public static IX509Store CreateCertificateStore(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate cert)
		{
			List<Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate> list = new List<Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate>();
			if (cert != null)
			{
				list.Add(cert);
			}
			return X509StoreFactory.Create("CERTIFICATE/COLLECTION", new X509CollectionStoreParameters(list));
		}

		public static IX509Store CreateCertificateStore(ICollection certsList)
		{
			return X509StoreFactory.Create("CERTIFICATE/COLLECTION", new X509CollectionStoreParameters(certsList));
		}

		public static Asn1Sequence GetX509Extension(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate SignerCert, DerObjectIdentifier ID)
		{
			try
			{
				return (Asn1Sequence)new Asn1InputStream(SignerCert.GetExtensionValue(ID).GetOctets()).ReadObject();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static Asn1Sequence GetX509Extension(byte[] Obj)
		{
			return (Asn1Sequence)new Asn1InputStream(Obj).ReadObject();
		}

		public static X509Chain GetChain(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate signingCertificate)
		{
			X509Certificate2 certificate = new X509Certificate2(signingCertificate.GetEncoded());
			try
			{
				X509Chain x509Chain = new X509Chain();
				x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
				x509Chain.Build(certificate);
				return x509Chain;
			}
			catch
			{
				return null;
			}
		}

		public static X509Chain GetChain(X509Certificate2 signingCertificate)
		{
			try
			{
				X509Chain x509Chain = new X509Chain();
				x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
				x509Chain.Build(signingCertificate);
				return x509Chain;
			}
			catch
			{
				return null;
			}
		}

		public static Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate GetIssuerCertificateFromChain(Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate signerCert)
		{
			X509Certificate2 x509Certificate = new X509Certificate2(signerCert.GetEncoded());
			try
			{
				X509Chain chain = GetChain(x509Certificate);
				if (chain.ChainElements.Count == 1)
				{
					return null;
				}
				string nameInfo = x509Certificate.GetNameInfo(X509NameType.SimpleName, true);
				X509ChainElementEnumerator enumerator = chain.ChainElements.GetEnumerator();
				while (enumerator.MoveNext())
				{
					X509ChainElement current = enumerator.Current;
					if (current.Certificate.GetNameInfo(X509NameType.SimpleName, false) == nameInfo)
					{
						return new Tr.Com.Eimza.Org.BouncyCastle.X509.X509Certificate(current.Certificate.RawData);
					}
				}
			}
			catch
			{
			}
			return null;
		}
	}
}
