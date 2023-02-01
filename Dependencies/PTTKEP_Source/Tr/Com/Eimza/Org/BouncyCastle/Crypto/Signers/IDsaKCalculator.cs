using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers
{
	internal interface IDsaKCalculator
	{
		bool IsDeterministic { get; }

		void Init(BigInteger n, SecureRandom random);

		void Init(BigInteger n, BigInteger d, byte[] message);

		BigInteger NextK();
	}
}
