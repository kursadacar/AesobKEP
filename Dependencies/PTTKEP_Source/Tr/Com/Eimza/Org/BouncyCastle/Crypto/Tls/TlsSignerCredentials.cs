namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal interface TlsSignerCredentials : TlsCredentials
	{
		SignatureAndHashAlgorithm SignatureAndHashAlgorithm { get; }

		byte[] GenerateCertificateSignature(byte[] hash);
	}
}
