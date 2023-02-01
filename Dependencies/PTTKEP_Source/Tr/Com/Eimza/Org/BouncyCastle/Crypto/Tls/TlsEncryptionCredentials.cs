namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal interface TlsEncryptionCredentials : TlsCredentials
	{
		byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret);
	}
}
