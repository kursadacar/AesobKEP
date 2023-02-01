namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal interface TlsAuthentication
	{
		void NotifyServerCertificate(Certificate serverCertificate);

		TlsCredentials GetClientCredentials(CertificateRequest certificateRequest);
	}
}
