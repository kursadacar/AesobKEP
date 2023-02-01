using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Smime
{
	internal abstract class SmimeAttributes
	{
		public static readonly DerObjectIdentifier SmimeCapabilities = PkcsObjectIdentifiers.Pkcs9AtSmimeCapabilities;

		public static readonly DerObjectIdentifier EncrypKeyPref = PkcsObjectIdentifiers.IdAAEncrypKeyPref;
	}
}
