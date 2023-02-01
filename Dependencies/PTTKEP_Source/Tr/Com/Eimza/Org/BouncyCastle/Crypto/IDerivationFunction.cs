namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	internal interface IDerivationFunction
	{
		IDigest Digest { get; }

		void Init(IDerivationParameters parameters);

		int GenerateBytes(byte[] output, int outOff, int length);
	}
}
