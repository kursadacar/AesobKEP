namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal interface TlsCipherFactory
	{
		TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm);
	}
}
