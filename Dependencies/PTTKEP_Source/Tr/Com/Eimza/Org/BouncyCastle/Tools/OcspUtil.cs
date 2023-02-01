using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
	internal static class OcspUtil
	{
		public static X509Certificate GetOcspCertificate(OcspResp ocspRep)
		{
			return new X509Certificate(X509CertificateStructure.GetInstance(ocspRep.ToBasicOcspResponse().Certs[0]));
		}

		public static X509Certificate GetOcspCertificate(BasicOcspResp basicOcspRep)
		{
			return new X509Certificate(X509CertificateStructure.GetInstance(basicOcspRep.Resp.Certs[0]));
		}

		public static X509Certificate GetOcspCertificate(OcspResponse ocspResponse)
		{
			return new X509Certificate(X509CertificateStructure.GetInstance(ocspResponse.ToBasicOcspResponse().Certs[0]));
		}

		public static string GetOCSPURL(X509Certificate certificate)
		{
			try
			{
				Asn1Object extensionValue = CertificateUtil.GetExtensionValue(certificate, X509Extensions.AuthorityInfoAccess.Id);
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

		public static bool IsSignatureVerify(BasicOcspResp ocspResponse, X509Certificate ocspCert)
		{
			try
			{
				AsymmetricKeyParameter publicKey = ocspCert.GetPublicKey();
				return ocspResponse.Verify(publicKey);
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
	}
}
