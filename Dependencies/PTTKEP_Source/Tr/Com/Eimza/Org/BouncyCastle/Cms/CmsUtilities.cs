using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal static class CmsUtilities
	{
		public static int MaximumMemory
		{
			get
			{
				return int.MaxValue;
			}
		}

		public static ContentInfo ReadContentInfo(byte[] input)
		{
			return ReadContentInfo(new Asn1InputStream(input));
		}

		public static ContentInfo ReadContentInfo(Stream input)
		{
			return ReadContentInfo(new Asn1InputStream(input, MaximumMemory));
		}

		public static ContentInfo ReadContentInfo(Asn1InputStream aIn)
		{
			try
			{
				return ContentInfo.GetInstance(aIn.ReadObject());
			}
			catch (IOException e)
			{
				throw new CmsException("IOException reading content.", e);
			}
			catch (InvalidCastException e2)
			{
				throw new CmsException("Malformed content.", e2);
			}
			catch (ArgumentException e3)
			{
				throw new CmsException("Malformed content.", e3);
			}
		}

		public static byte[] StreamToByteArray(Stream inStream)
		{
			return Streams.ReadAll(inStream);
		}

		public static byte[] StreamToByteArray(Stream inStream, int limit)
		{
			return Streams.ReadAllLimited(inStream, limit);
		}

		public static IList GetCertificatesFromStore(IX509Store certStore)
		{
			try
			{
				IList list = Platform.CreateArrayList();
				if (certStore != null)
				{
					foreach (X509Certificate match in certStore.GetMatches(null))
					{
						list.Add(X509CertificateStructure.GetInstance(Asn1Object.FromByteArray(match.GetEncoded())));
					}
				}
				return list;
			}
			catch (CertificateEncodingException e)
			{
				throw new CmsException("error encoding certs", e);
			}
			catch (Exception e2)
			{
				throw new CmsException("error processing certs", e2);
			}
		}

		public static IList GetCrlsFromStore(IX509Store crlStore)
		{
			try
			{
				IList list = Platform.CreateArrayList();
				if (crlStore != null)
				{
					foreach (X509Crl match in crlStore.GetMatches(null))
					{
						list.Add(CertificateList.GetInstance(Asn1Object.FromByteArray(match.GetEncoded())));
					}
				}
				return list;
			}
			catch (CrlException e)
			{
				throw new CmsException("error encoding crls", e);
			}
			catch (Exception e2)
			{
				throw new CmsException("error processing crls", e2);
			}
		}

		public static Asn1Set CreateBerSetFromList(IList berObjects)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
			foreach (Asn1Encodable berObject in berObjects)
			{
				asn1EncodableVector.Add(berObject);
			}
			return new BerSet(asn1EncodableVector);
		}

		public static Asn1Set CreateDerSetFromList(IList derObjects)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
			foreach (Asn1Encodable derObject in derObjects)
			{
				asn1EncodableVector.Add(derObject);
			}
			return new DerSet(asn1EncodableVector);
		}

		public static Stream CreateBerOctetOutputStream(Stream s, int tagNo, bool isExplicit, int bufferSize)
		{
			return new BerOctetStringGenerator(s, tagNo, isExplicit).GetOctetOutputStream(bufferSize);
		}

		public static TbsCertificateStructure GetTbsCertificateStructure(X509Certificate cert)
		{
			return TbsCertificateStructure.GetInstance(Asn1Object.FromByteArray(cert.GetTbsCertificate()));
		}

		public static IssuerAndSerialNumber GetIssuerAndSerialNumber(X509Certificate cert)
		{
			TbsCertificateStructure tbsCertificateStructure = GetTbsCertificateStructure(cert);
			return new IssuerAndSerialNumber(tbsCertificateStructure.Issuer, tbsCertificateStructure.SerialNumber.Value);
		}
	}
}
