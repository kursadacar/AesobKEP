using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.OID
{
	internal sealed class DigestAlgorithm
	{
		public static readonly DigestAlgorithm SHA1 = new DigestAlgorithm("SHA-1", "1.3.14.3.2.26", "SHA1");

		public static readonly DigestAlgorithm SHA256 = new DigestAlgorithm("SHA-256", "2.16.840.1.101.3.4.2.1", "SHA256");

		public static readonly DigestAlgorithm SHA384 = new DigestAlgorithm("SHA-384", "2.16.840.1.101.3.4.2.3", "SHA384");

		public static readonly DigestAlgorithm SHA512 = new DigestAlgorithm("SHA-512", "2.16.840.1.101.3.4.2.3", "SHA512");

		private readonly string name;

		private string oid;

		private string xmlId;

		private DigestAlgorithm(string name, string oid, string xmlId)
		{
			this.name = name;
			this.oid = oid;
			this.xmlId = xmlId;
		}

		public static DigestAlgorithm GetByName(string algoName)
		{
			if ("SHA-1".Equals(algoName) || "SHA1".Equals(algoName))
			{
				return SHA1;
			}
			if ("SHA-256".Equals(algoName) || "SHA256".Equals(algoName))
			{
				return SHA256;
			}
			if ("SHA-384".Equals(algoName) || "SHA384".Equals(algoName))
			{
				return SHA256;
			}
			if ("SHA-512".Equals(algoName) || "SHA512".Equals(algoName))
			{
				return SHA512;
			}
			throw new NoSuchAlgorithmException("Desteklenmeyen Algoritma : " + algoName);
		}

		public string GetName()
		{
			return name;
		}

		public string GetOid()
		{
			return oid;
		}

		public string GetXmlId()
		{
			return xmlId;
		}

		public AlgorithmIdentifier GetAlgorithmIdentifier()
		{
			return new AlgorithmIdentifier(new DerObjectIdentifier(GetOid()), DerNull.Instance);
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
			if (!(obj is DigestAlgorithm))
			{
				return false;
			}
			DigestAlgorithm digestAlgorithm = (DigestAlgorithm)obj;
			if (name == null)
			{
				if (digestAlgorithm.name != null)
				{
					return false;
				}
			}
			else if (!name.Equals(digestAlgorithm.name))
			{
				return false;
			}
			return true;
		}
	}
}
