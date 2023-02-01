using Tr.Com.Eimza.Org.BouncyCastle.Asn1.CryptoPro;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Multiplier;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Generators
{
	internal class Gost3410KeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		private Gost3410KeyGenerationParameters param;

		public void Init(KeyGenerationParameters parameters)
		{
			if (parameters is Gost3410KeyGenerationParameters)
			{
				param = (Gost3410KeyGenerationParameters)parameters;
				return;
			}
			Gost3410KeyGenerationParameters gost3410KeyGenerationParameters = new Gost3410KeyGenerationParameters(parameters.Random, CryptoProObjectIdentifiers.GostR3410x94CryptoProA);
			int strength = parameters.Strength;
			int num = gost3410KeyGenerationParameters.Parameters.P.BitLength - 1;
			param = gost3410KeyGenerationParameters;
		}

		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			SecureRandom random = param.Random;
			Gost3410Parameters parameters = param.Parameters;
			BigInteger q = parameters.Q;
			int num = 64;
			BigInteger bigInteger;
			do
			{
				bigInteger = new BigInteger(256, random);
			}
			while (bigInteger.SignValue < 1 || bigInteger.CompareTo(q) >= 0 || WNafUtilities.GetNafWeight(bigInteger) < num);
			BigInteger p = parameters.P;
			BigInteger y = parameters.A.ModPow(bigInteger, p);
			if (param.PublicKeyParamSet != null)
			{
				return new AsymmetricCipherKeyPair(new Gost3410PublicKeyParameters(y, param.PublicKeyParamSet), new Gost3410PrivateKeyParameters(bigInteger, param.PublicKeyParamSet));
			}
			return new AsymmetricCipherKeyPair(new Gost3410PublicKeyParameters(y, parameters), new Gost3410PrivateKeyParameters(bigInteger, parameters));
		}
	}
}
