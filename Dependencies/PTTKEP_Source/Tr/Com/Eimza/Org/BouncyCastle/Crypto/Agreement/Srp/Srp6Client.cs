using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Agreement.Srp
{
	internal class Srp6Client
	{
		protected BigInteger N;

		protected BigInteger g;

		protected BigInteger privA;

		protected BigInteger pubA;

		protected BigInteger B;

		protected BigInteger x;

		protected BigInteger u;

		protected BigInteger S;

		protected IDigest digest;

		protected SecureRandom random;

		public virtual void Init(BigInteger N, BigInteger g, IDigest digest, SecureRandom random)
		{
			this.N = N;
			this.g = g;
			this.digest = digest;
			this.random = random;
		}

		public virtual BigInteger GenerateClientCredentials(byte[] salt, byte[] identity, byte[] password)
		{
			x = Srp6Utilities.CalculateX(digest, N, salt, identity, password);
			privA = SelectPrivateValue();
			pubA = g.ModPow(privA, N);
			return pubA;
		}

		public virtual BigInteger CalculateSecret(BigInteger serverB)
		{
			B = Srp6Utilities.ValidatePublicValue(N, serverB);
			u = Srp6Utilities.CalculateU(digest, N, pubA, B);
			S = CalculateS();
			return S;
		}

		protected virtual BigInteger SelectPrivateValue()
		{
			return Srp6Utilities.GeneratePrivateValue(digest, N, g, random);
		}

		private BigInteger CalculateS()
		{
			BigInteger val = Srp6Utilities.CalculateK(digest, N, g);
			BigInteger e = u.Multiply(x).Add(privA);
			BigInteger n = g.ModPow(x, N).Multiply(val).Mod(N);
			return B.Subtract(n).Mod(N).ModPow(e, N);
		}
	}
}
