using System.Collections.Generic;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;

namespace Tr.Com.Eimza.Org.BouncyCastle.OID
{
	internal static class DigestAlgorithms
	{
		public const string SHA1 = "1.3.14.3.2.26";

		public const string SHA256 = "2.16.840.1.101.3.4.2.1";

		public const string SHA384 = "2.16.840.1.101.3.4.2.2";

		public const string SHA512 = "2.16.840.1.101.3.4.2.3";

		public const string SHA224 = "2.16.840.1.101.3.4.2.4";

		public const string RSA_WITH_MD5 = "1.2.840.113549.1.1.4";

		public const string RSA_WITH_SHA1 = "1.2.840.113549.1.1.5";

		public const string RSA_WITH_SHA256 = "1.2.840.113549.1.1.11";

		public const string RSA_WITH_SHA384 = "1.2.840.113549.1.1.12";

		public const string RSA_WITH_SHA512 = "1.2.840.113549.1.1.13";

		public static readonly DerObjectIdentifier SHA1_OID;

		public static readonly DerObjectIdentifier SHA256_OID;

		public static readonly DerObjectIdentifier SHA384_OID;

		public static readonly DerObjectIdentifier SHA512_OID;

		public static readonly DerObjectIdentifier SHA224_OID;

		public static readonly DerObjectIdentifier RSA_WITH_MD5_OID;

		public static readonly DerObjectIdentifier RSA_WITH_SHA1_OID;

		public static readonly DerObjectIdentifier RSA_WITH_SHA256_OID;

		public static readonly DerObjectIdentifier RSA_WITH_SHA384_OID;

		public static readonly DerObjectIdentifier RSA_WITH_SHA512_OID;

		public static readonly List<DerObjectIdentifier> REVOKED_HASH_ALG_LIST;

		static DigestAlgorithms()
		{
			SHA1_OID = new DerObjectIdentifier("1.3.14.3.2.26");
			SHA256_OID = new DerObjectIdentifier("2.16.840.1.101.3.4.2.1");
			SHA384_OID = new DerObjectIdentifier("2.16.840.1.101.3.4.2.2");
			SHA512_OID = new DerObjectIdentifier("2.16.840.1.101.3.4.2.3");
			SHA224_OID = new DerObjectIdentifier("2.16.840.1.101.3.4.2.4");
			RSA_WITH_MD5_OID = new DerObjectIdentifier("1.2.840.113549.1.1.4");
			RSA_WITH_SHA1_OID = new DerObjectIdentifier("1.2.840.113549.1.1.5");
			RSA_WITH_SHA256_OID = new DerObjectIdentifier("1.2.840.113549.1.1.11");
			RSA_WITH_SHA384_OID = new DerObjectIdentifier("1.2.840.113549.1.1.12");
			RSA_WITH_SHA512_OID = new DerObjectIdentifier("1.2.840.113549.1.1.13");
			REVOKED_HASH_ALG_LIST = new List<DerObjectIdentifier>();
			REVOKED_HASH_ALG_LIST.Add(RSA_WITH_SHA1_OID);
			REVOKED_HASH_ALG_LIST.Add(RSA_WITH_MD5_OID);
		}
	}
}
