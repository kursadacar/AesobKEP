using System;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;

namespace Tr.Com.Eimza.Org.BouncyCastle.OID
{
	internal sealed class SignatureAlgorithm
	{
		public static readonly SignatureAlgorithm RSA = new SignatureAlgorithm("RSA", CmsSignedGenerator.EncryptionRsa, "RSA/ECB/PKCS1Padding");

		public static readonly SignatureAlgorithm DSA = new SignatureAlgorithm("DSA", CmsSignedGenerator.EncryptionDsa, string.Empty);

		public static readonly SignatureAlgorithm ECDSA = new SignatureAlgorithm("ECDSA", CmsSignedGenerator.EncryptionECDsa, "ECDSA");

		private string name;

		private string oid;

		private string padding;

		private SignatureAlgorithm(string name, string oid, string padding)
		{
			this.name = name;
			this.oid = oid;
			this.padding = padding;
		}

		public string GetSignatureAlgorithm(DigestAlgorithm algorithm)
		{
			if (Equals(RSA))
			{
				if (algorithm.Equals(DigestAlgorithm.SHA1))
				{
					return "SHA1WITHRSA";
				}
				if (algorithm.Equals(DigestAlgorithm.SHA256))
				{
					return "SHA256WITHRSA";
				}
				if (algorithm.Equals(DigestAlgorithm.SHA384))
				{
					return "SHA384WITHRSA";
				}
				if (algorithm.Equals(DigestAlgorithm.SHA512))
				{
					return "SHA512WITHRSA";
				}
			}
			else if (Equals(ECDSA))
			{
				if (algorithm.Equals(DigestAlgorithm.SHA1))
				{
					return "SHA1WITHECDSA";
				}
				if (algorithm.Equals(DigestAlgorithm.SHA256))
				{
					return "SHA256WITHECDSA";
				}
				if (algorithm.Equals(DigestAlgorithm.SHA384))
				{
					return "SHA384WITHECDSA";
				}
				if (algorithm.Equals(DigestAlgorithm.SHA512))
				{
					return "SHA512WITHECDSA";
				}
			}
			throw new NotSupportedException();
		}

		public string GetXMLSignatureAlgorithm(DigestAlgorithm digestAlgo)
		{
			if (Equals(RSA))
			{
				if (digestAlgo.Equals(DigestAlgorithm.SHA1))
				{
					return "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
				}
				if (digestAlgo.Equals(DigestAlgorithm.SHA256))
				{
					return "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
				}
				if (digestAlgo.Equals(DigestAlgorithm.SHA256))
				{
					return "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";
				}
			}
			else if (Equals(ECDSA))
			{
				if (digestAlgo.Equals(DigestAlgorithm.SHA1))
				{
					return "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha1";
				}
				if (digestAlgo.Equals(DigestAlgorithm.SHA256))
				{
					return "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256";
				}
				if (digestAlgo.Equals(DigestAlgorithm.SHA256))
				{
					return "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512";
				}
			}
			throw new NotSupportedException();
		}

		public string GetName()
		{
			return name;
		}

		public void SetName(string name)
		{
			this.name = name;
		}

		public string GetOid()
		{
			return oid;
		}

		public void SetOid(string oid)
		{
			this.oid = oid;
		}

		public string GetPadding()
		{
			return padding;
		}

		public void SetPadding(string padding)
		{
			this.padding = padding;
		}

		public override int GetHashCode()
		{
			int num = 1;
			return 31 * num + ((name != null) ? name.GetHashCode() : 0);
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (!(obj is SignatureAlgorithm))
			{
				return false;
			}
			SignatureAlgorithm signatureAlgorithm = (SignatureAlgorithm)obj;
			if (name == null)
			{
				if (signatureAlgorithm.name != null)
				{
					return false;
				}
			}
			else if (!name.Equals(signatureAlgorithm.name))
			{
				return false;
			}
			return true;
		}
	}
}
