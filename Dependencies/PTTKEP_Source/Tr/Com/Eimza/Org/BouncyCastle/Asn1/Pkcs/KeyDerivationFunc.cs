using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs
{
	internal class KeyDerivationFunc : AlgorithmIdentifier
	{
		internal KeyDerivationFunc(Asn1Sequence seq)
			: base(seq)
		{
		}

		public KeyDerivationFunc(DerObjectIdentifier id, Asn1Encodable parameters)
			: base(id, parameters)
		{
		}
	}
}
