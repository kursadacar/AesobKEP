using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ocsp
{
	internal class CertificateID
	{
		public const string HashSha1 = "1.3.14.3.2.26";

		public const string HashSha256 = "2.16.840.1.101.3.4.2.1";

		public const string HashSha384 = "2.16.840.1.101.3.4.2.2";

		public const string HashSha512 = "2.16.840.1.101.3.4.2.3";

		private readonly CertID id;

		public string HashAlgOid
		{
			get
			{
				return id.HashAlgorithm.ObjectID.Id;
			}
		}

		public CertID CertID
		{
			get
			{
				return id;
			}
		}

		public BigInteger SerialNumber
		{
			get
			{
				return id.SerialNumber.Value;
			}
		}

		public CertificateID(CertID id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.id = id;
		}

		public CertificateID(string hashAlgorithm, X509Certificate issuerCert, BigInteger serialNumber)
		{
			AlgorithmIdentifier hashAlg = new AlgorithmIdentifier(new DerObjectIdentifier(hashAlgorithm), DerNull.Instance);
			id = CreateCertID(hashAlg, issuerCert, new DerInteger(serialNumber));
		}

		public byte[] GetIssuerNameHash()
		{
			return id.IssuerNameHash.GetOctets();
		}

		public byte[] GetIssuerKeyHash()
		{
			return id.IssuerKeyHash.GetOctets();
		}

		public bool MatchesIssuer(X509Certificate issuerCert)
		{
			return CreateCertID(id.HashAlgorithm, issuerCert, id.SerialNumber).Equals(id);
		}

		public CertID ToAsn1Object()
		{
			return id;
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			CertificateID certificateID = obj as CertificateID;
			if (certificateID == null)
			{
				return false;
			}
			return id.ToAsn1Object().Equals(certificateID.id.ToAsn1Object());
		}

		public override int GetHashCode()
		{
			return id.ToAsn1Object().GetHashCode();
		}

		public static CertificateID DeriveCertificateID(CertificateID original, BigInteger newSerialNumber)
		{
			return new CertificateID(new CertID(original.id.HashAlgorithm, original.id.IssuerNameHash, original.id.IssuerKeyHash, new DerInteger(newSerialNumber)));
		}

		public static CertID CreateCertID(AlgorithmIdentifier hashAlg, X509Certificate issuerCert, DerInteger serialNumber)
		{
			try
			{
				string algorithm = hashAlg.ObjectID.Id;
				X509Name subjectX509Principal = PrincipalUtilities.GetSubjectX509Principal(issuerCert);
				byte[] str = DigestUtilities.CalculateDigest(algorithm, subjectX509Principal.GetEncoded());
				SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(issuerCert.GetPublicKey());
				byte[] str2 = DigestUtilities.CalculateDigest(algorithm, subjectPublicKeyInfo.PublicKeyData.GetBytes());
				return new CertID(hashAlg, new DerOctetString(str), new DerOctetString(str2), serialNumber);
			}
			catch (Exception ex)
			{
				throw new OcspException("problem creating ID: " + ((ex != null) ? ex.ToString() : null), ex);
			}
		}
	}
}
