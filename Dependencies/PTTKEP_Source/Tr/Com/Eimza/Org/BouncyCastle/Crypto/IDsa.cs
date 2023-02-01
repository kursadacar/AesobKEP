using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	internal interface IDsa
	{
		string AlgorithmName { get; }

		void Init(bool forSigning, ICipherParameters parameters);

		BigInteger[] GenerateSignature(byte[] message);

		bool VerifySignature(byte[] message, BigInteger r, BigInteger s);
	}
}
