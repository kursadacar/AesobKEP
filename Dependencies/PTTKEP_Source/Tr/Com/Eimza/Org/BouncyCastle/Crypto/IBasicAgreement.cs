using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	internal interface IBasicAgreement
	{
		void Init(ICipherParameters parameters);

		int GetFieldSize();

		BigInteger CalculateAgreement(ICipherParameters pubKey);
	}
}
