using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsDssSigner : TlsDsaSigner
	{
		protected override byte SignatureAlgorithm
		{
			get
			{
				return 2;
			}
		}

		public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
		{
			return publicKey is DsaPublicKeyParameters;
		}

		protected override IDsa CreateDsaImpl(byte hashAlgorithm)
		{
			return new DsaSigner(new HMacDsaKCalculator(TlsUtilities.CreateHash(hashAlgorithm)));
		}
	}
}
