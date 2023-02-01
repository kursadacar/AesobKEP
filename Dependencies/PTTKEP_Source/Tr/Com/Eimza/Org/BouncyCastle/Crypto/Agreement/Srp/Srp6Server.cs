using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Agreement.Srp
{
	internal class Srp6Server
	{
		protected BigInteger N;

		protected BigInteger g;

		protected BigInteger v;

		protected SecureRandom random;

		protected IDigest digest;

		protected BigInteger A;

		protected BigInteger privB;

		protected BigInteger pubB;

		protected BigInteger u;

		protected BigInteger S;

		public virtual void Init(BigInteger N, BigInteger g, BigInteger v, IDigest digest, SecureRandom random)
		{
			this.N = N;
			this.g = g;
			this.v = v;
			this.random = random;
			this.digest = digest;
		}

		public virtual BigInteger GenerateServerCredentials()
		{
			BigInteger bigInteger = Srp6Utilities.CalculateK(digest, N, g);
			privB = SelectPrivateValue();
			pubB = bigInteger.Multiply(v).Mod(N).Add(g.ModPow(privB, N))
				.Mod(N);
			return pubB;
		}

		public virtual BigInteger CalculateSecret(BigInteger clientA)
		{
			A = Srp6Utilities.ValidatePublicValue(N, clientA);
			u = Srp6Utilities.CalculateU(digest, N, A, pubB);
			S = CalculateS();
			return S;
		}

		protected virtual BigInteger SelectPrivateValue()
		{
			return Srp6Utilities.GeneratePrivateValue(digest, N, g, random);
		}

		private BigInteger CalculateS()
		{
			return v.ModPow(u, N).Multiply(A).Mod(N)
				.ModPow(privB, N);
		}
	}
}
