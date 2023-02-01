namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal abstract class AbstractTlsEncryptionCredentials : AbstractTlsCredentials, TlsEncryptionCredentials, TlsCredentials
	{
		public abstract byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret);
	}
}
