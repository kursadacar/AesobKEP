using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms
{
	internal abstract class CmsAttributes
	{
		public static readonly DerObjectIdentifier ContentType = PkcsObjectIdentifiers.Pkcs9AtContentType;

		public static readonly DerObjectIdentifier MessageDigest = PkcsObjectIdentifiers.Pkcs9AtMessageDigest;

		public static readonly DerObjectIdentifier SigningTime = PkcsObjectIdentifiers.Pkcs9AtSigningTime;

		public static readonly DerObjectIdentifier CounterSignature = PkcsObjectIdentifiers.Pkcs9AtCounterSignature;

		public static readonly DerObjectIdentifier ContentHint = PkcsObjectIdentifiers.IdAAContentHint;
	}
}
