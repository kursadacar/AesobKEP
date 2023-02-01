namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	internal interface ISigner
	{
		string AlgorithmName { get; }

		void Init(bool forSigning, ICipherParameters parameters);

		void Update(byte input);

		void BlockUpdate(byte[] input, int inOff, int length);

		byte[] GenerateSignature();

		byte[] CurrentSignature();

		bool VerifySignature(byte[] signature);

		void Reset();
	}
}
