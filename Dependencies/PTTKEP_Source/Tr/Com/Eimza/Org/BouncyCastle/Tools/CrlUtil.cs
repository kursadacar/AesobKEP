using System.Net;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
	internal static class CrlUtil
	{
		public static X509Crl GetX509Crl(X509Certificate certificate)
		{
			try
			{
				string cRLURL = CertificateUtil.GetCRLURL(certificate);
				if (cRLURL == null)
				{
					return null;
				}
				if (cRLURL.StartsWith("http://") || cRLURL.StartsWith("https://"))
				{
					X509Crl crl = GetCrl(cRLURL);
					if (!crl.IsRevoked(certificate))
					{
						return crl;
					}
				}
			}
			catch (CrlException)
			{
			}
			catch (CertificateException)
			{
			}
			return null;
		}

		public static X509Crl GetCrl(string downloadUrl)
		{
			if (downloadUrl != null)
			{
				BcUtil.FindFileNameFromUrl(downloadUrl);
				try
				{
					return GetCrlFromData(new MyWebClient().DownloadData(downloadUrl));
				}
				catch (WebException)
				{
				}
			}
			return null;
		}

		public static X509Crl GetCrlFromData(byte[] crlData)
		{
			return new X509Crl(crlData);
		}

		public static string GetCRLURL(X509Certificate certificate)
		{
			try
			{
				Asn1Object extensionValue = CertificateUtil.GetExtensionValue(certificate, X509Extensions.CrlDistributionPoints.Id);
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

		public static bool IsSignatureValid(X509Crl crl, X509Certificate crlIssuer)
		{
			try
			{
				AsymmetricKeyParameter publicKey = crlIssuer.GetPublicKey();
				crl.Verify(publicKey);
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
	}
}
