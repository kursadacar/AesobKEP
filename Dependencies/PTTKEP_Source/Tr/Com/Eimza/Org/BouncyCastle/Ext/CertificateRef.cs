using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Esf;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Encoders;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class CertificateRef
	{
		private string digestAlgorithm;

		private byte[] digestValue;

		private string issuerName;

		private string issuerSerial;

		public OtherCertID CertificateID { get; set; }

		public override string ToString()
		{
			return "CertificateRef[issuerName=" + issuerName + ",issuerSerial=" + issuerSerial + ",digest=" + Hex.ToHexString(digestValue) + "]";
		}

		public CertificateRef()
		{
		}

		public CertificateRef(OtherCertID certificateID)
		{
			CertificateID = certificateID;
			SetDigestAlgorithm(CertificateID.OtherCertHash.HashAlgorithm.ObjectID.Id);
			CertificateID.OtherCertHash.GetHashValue();
			SetDigestValue(CertificateID.OtherCertHash.GetHashValue());
			if (CertificateID.IssuerSerial != null)
			{
				if (CertificateID.IssuerSerial.Issuer != null)
				{
					SetIssuerName(CertificateID.IssuerSerial.Issuer.ToString());
				}
				if (CertificateID.IssuerSerial.Serial != null)
				{
					SetIssuerSerial(CertificateID.IssuerSerial.Serial.ToString());
				}
			}
		}

		public string GetDigestAlgorithm()
		{
			return digestAlgorithm;
		}

		public void SetDigestAlgorithm(string digestAlgorithm)
		{
			this.digestAlgorithm = digestAlgorithm;
		}

		public byte[] GetDigestValue()
		{
			return digestValue;
		}

		public void SetDigestValue(byte[] digestValue)
		{
			this.digestValue = digestValue;
		}

		public string GetIssuerName()
		{
			return issuerName;
		}

		public void SetIssuerName(string issuerName)
		{
			this.issuerName = issuerName;
		}

		public string GetIssuerSerial()
		{
			return issuerSerial;
		}

		public void SetIssuerSerial(string issuerSerial)
		{
			this.issuerSerial = issuerSerial;
		}
	}
}
