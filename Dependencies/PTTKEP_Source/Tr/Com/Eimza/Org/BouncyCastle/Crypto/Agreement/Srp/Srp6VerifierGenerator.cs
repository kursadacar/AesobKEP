using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Agreement.Srp
{
	internal class Srp6VerifierGenerator
	{
		protected BigInteger N;

		protected BigInteger g;

		protected IDigest digest;

		public virtual void Init(BigInteger N, BigInteger g, IDigest digest)
		{
			this.N = N;
			this.g = g;
			this.digest = digest;
		}

		public virtual BigInteger GenerateVerifier(byte[] salt, byte[] identity, byte[] password)
		{
			BigInteger e = Srp6Utilities.CalculateX(digest, N, salt, identity, password);
			return g.ModPow(e, N);
		}
	}
}
