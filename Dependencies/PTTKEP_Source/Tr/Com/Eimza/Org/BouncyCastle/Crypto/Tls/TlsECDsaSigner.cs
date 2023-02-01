using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsECDsaSigner : TlsDsaSigner
	{
		protected override byte SignatureAlgorithm
		{
			get
			{
				return 3;
			}
		}

		public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
		{
			return publicKey is ECPublicKeyParameters;
		}

		protected override IDsa CreateDsaImpl(byte hashAlgorithm)
		{
			return new ECDsaSigner(new HMacDsaKCalculator(TlsUtilities.CreateHash(hashAlgorithm)));
		}
	}
}
