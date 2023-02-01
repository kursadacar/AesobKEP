using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Multiplier;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Generators
{
	internal class RsaKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		private static readonly BigInteger DefaultPublicExponent = BigInteger.ValueOf(65537L);

		private const int DefaultTests = 12;

		private RsaKeyGenerationParameters param;

		public void Init(KeyGenerationParameters parameters)
		{
			if (parameters is RsaKeyGenerationParameters)
			{
				param = (RsaKeyGenerationParameters)parameters;
			}
			else
			{
				param = new RsaKeyGenerationParameters(DefaultPublicExponent, parameters.Random, parameters.Strength, 12);
			}
		}

		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			int strength = param.Strength;
			int num = strength >> 1;
			int bitlength = strength - num;
			int num2 = strength / 3;
			int num3 = strength >> 2;
			BigInteger publicExponent = param.PublicExponent;
			BigInteger bigInteger = ChooseRandomPrime(bitlength, publicExponent);
			BigInteger bigInteger2;
			BigInteger bigInteger3;
			while (true)
			{
				bigInteger2 = ChooseRandomPrime(num, publicExponent);
				if (bigInteger2.Subtract(bigInteger).Abs().BitLength < num2)
				{
					continue;
				}
				bigInteger3 = bigInteger.Multiply(bigInteger2);
				if (bigInteger3.BitLength != strength)
				{
					bigInteger = bigInteger.Max(bigInteger2);
					continue;
				}
				if (WNafUtilities.GetNafWeight(bigInteger3) >= num3)
				{
					break;
				}
				bigInteger = ChooseRandomPrime(bitlength, publicExponent);
			}
			BigInteger bigInteger4;
			if (bigInteger.CompareTo(bigInteger2) < 0)
			{
				bigInteger4 = bigInteger;
				bigInteger = bigInteger2;
				bigInteger2 = bigInteger4;
			}
			BigInteger bigInteger5 = bigInteger.Subtract(BigInteger.One);
			BigInteger bigInteger6 = bigInteger2.Subtract(BigInteger.One);
			bigInteger4 = bigInteger5.Multiply(bigInteger6);
			BigInteger bigInteger7 = publicExponent.ModInverse(bigInteger4);
			BigInteger dP = bigInteger7.Remainder(bigInteger5);
			BigInteger dQ = bigInteger7.Remainder(bigInteger6);
			BigInteger qInv = bigInteger2.ModInverse(bigInteger);
			return new AsymmetricCipherKeyPair(new RsaKeyParameters(false, bigInteger3, publicExponent), new RsaPrivateCrtKeyParameters(bigInteger3, publicExponent, bigInteger7, bigInteger, bigInteger2, dP, dQ, qInv));
		}

		protected virtual BigInteger ChooseRandomPrime(int bitlength, BigInteger e)
		{
			BigInteger bigInteger;
			do
			{
				bigInteger = new BigInteger(bitlength, 1, param.Random);
			}
			while (bigInteger.Mod(e).Equals(BigInteger.One) || !bigInteger.IsProbablePrime(param.Certainty) || !e.Gcd(bigInteger.Subtract(BigInteger.One)).Equals(BigInteger.One));
			return bigInteger;
		}
	}
}
