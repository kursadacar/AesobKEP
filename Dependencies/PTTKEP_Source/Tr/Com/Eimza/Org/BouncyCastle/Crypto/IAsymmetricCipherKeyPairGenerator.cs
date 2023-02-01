namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	internal interface IAsymmetricCipherKeyPairGenerator
	{
		void Init(KeyGenerationParameters parameters);

		AsymmetricCipherKeyPair GenerateKeyPair();
	}
}
