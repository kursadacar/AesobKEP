namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal interface TlsAgreementCredentials : TlsCredentials
	{
		byte[] GenerateAgreement(AsymmetricKeyParameter peerPublicKey);
	}
}
