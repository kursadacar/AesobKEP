namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	internal interface ISignerWithRecovery : ISigner
	{
		bool HasFullMessage();

		byte[] GetRecoveredMessage();

		void UpdateWithRecoveredMessage(byte[] signature);
	}
}
