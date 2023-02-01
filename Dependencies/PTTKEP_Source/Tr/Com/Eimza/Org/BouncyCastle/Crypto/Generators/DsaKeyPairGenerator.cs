using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Multiplier;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Generators
{
	internal class DsaKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		private static readonly BigInteger One = BigInteger.One;

		private DsaKeyGenerationParameters param;

		public void Init(KeyGenerationParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			param = (DsaKeyGenerationParameters)parameters;
		}

		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			DsaParameters parameters = param.Parameters;
			BigInteger x = GeneratePrivateKey(parameters.Q, param.Random);
			return new AsymmetricCipherKeyPair(new DsaPublicKeyParameters(CalculatePublicKey(parameters.P, parameters.G, x), parameters), new DsaPrivateKeyParameters(x, parameters));
		}

		private static BigInteger GeneratePrivateKey(BigInteger q, SecureRandom random)
		{
			int num = q.BitLength >> 2;
			BigInteger bigInteger;
			do
			{
				bigInteger = BigIntegers.CreateRandomInRange(One, q.Subtract(One), random);
			}
			while (WNafUtilities.GetNafWeight(bigInteger) < num);
			return bigInteger;
		}

		private static BigInteger CalculatePublicKey(BigInteger p, BigInteger g, BigInteger x)
		{
			return g.ModPow(x, p);
		}
	}
}
