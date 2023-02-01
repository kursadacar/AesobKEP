using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class SignerID : X509CertStoreSelector
	{
		public override int GetHashCode()
		{
			int num = Arrays.GetHashCode(base.SubjectKeyIdentifier);
			BigInteger bigInteger = base.SerialNumber;
			if (bigInteger != null)
			{
				num ^= bigInteger.GetHashCode();
			}
			X509Name x509Name = base.Issuer;
			if (x509Name != null)
			{
				num ^= x509Name.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return false;
			}
			SignerID signerID = obj as SignerID;
			if (signerID == null)
			{
				return false;
			}
			if (Arrays.AreEqual(base.SubjectKeyIdentifier, signerID.SubjectKeyIdentifier) && object.Equals(base.SerialNumber, signerID.SerialNumber))
			{
				return X509CertStoreSelector.IssuersMatch(base.Issuer, signerID.Issuer);
			}
			return false;
		}
	}
}
