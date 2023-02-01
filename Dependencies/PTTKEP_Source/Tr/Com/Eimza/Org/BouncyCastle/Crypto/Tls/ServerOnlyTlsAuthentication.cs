namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal abstract class ServerOnlyTlsAuthentication : TlsAuthentication
	{
		public abstract void NotifyServerCertificate(Certificate serverCertificate);

		public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest)
		{
			return null;
		}
	}
}
