using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Generators
{
	internal class DHKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		private DHKeyGenerationParameters param;

		public virtual void Init(KeyGenerationParameters parameters)
		{
			param = (DHKeyGenerationParameters)parameters;
		}

		public virtual AsymmetricCipherKeyPair GenerateKeyPair()
		{
			DHKeyGeneratorHelper instance = DHKeyGeneratorHelper.Instance;
			DHParameters parameters = param.Parameters;
			BigInteger x = instance.CalculatePrivate(parameters, param.Random);
			return new AsymmetricCipherKeyPair(new DHPublicKeyParameters(instance.CalculatePublic(parameters, x), parameters), new DHPrivateKeyParameters(x, parameters));
		}
	}
}
