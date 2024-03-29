namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal interface TlsCipher
	{
		int GetPlaintextLimit(int ciphertextLimit);

		byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len);

		byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len);
	}
}
