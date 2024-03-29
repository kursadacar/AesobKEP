namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities.Encoders
{
	internal interface ITranslator
	{
		int GetEncodedBlockSize();

		int Encode(byte[] input, int inOff, int length, byte[] outBytes, int outOff);

		int GetDecodedBlockSize();

		int Decode(byte[] input, int inOff, int length, byte[] outBytes, int outOff);
	}
}
