using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	internal class NaccacheSternKeyParameters : AsymmetricKeyParameter
	{
		private readonly BigInteger g;

		private readonly BigInteger n;

		private readonly int lowerSigmaBound;

		public BigInteger G
		{
			get
			{
				return g;
			}
		}

		public int LowerSigmaBound
		{
			get
			{
				return lowerSigmaBound;
			}
		}

		public BigInteger Modulus
		{
			get
			{
				return n;
			}
		}

		public NaccacheSternKeyParameters(bool privateKey, BigInteger g, BigInteger n, int lowerSigmaBound)
			: base(privateKey)
		{
			this.g = g;
			this.n = n;
			this.lowerSigmaBound = lowerSigmaBound;
		}
	}
}
