using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Generators
{
	internal class ElGamalKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		private ElGamalKeyGenerationParameters param;

		public void Init(KeyGenerationParameters parameters)
		{
			param = (ElGamalKeyGenerationParameters)parameters;
		}

		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			DHKeyGeneratorHelper instance = DHKeyGeneratorHelper.Instance;
			ElGamalParameters parameters = param.Parameters;
			DHParameters dhParams = new DHParameters(parameters.P, parameters.G, null, 0, parameters.L);
			BigInteger x = instance.CalculatePrivate(dhParams, param.Random);
			return new AsymmetricCipherKeyPair(new ElGamalPublicKeyParameters(instance.CalculatePublic(dhParams, x), parameters), new ElGamalPrivateKeyParameters(x, parameters));
		}
	}
}
