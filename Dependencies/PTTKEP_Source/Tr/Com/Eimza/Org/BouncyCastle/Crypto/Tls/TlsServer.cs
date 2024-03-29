using System.Collections;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal interface TlsServer : TlsPeer
	{
		void Init(TlsServerContext context);

		void NotifyClientVersion(ProtocolVersion clientVersion);

		void NotifyOfferedCipherSuites(int[] offeredCipherSuites);

		void NotifyOfferedCompressionMethods(byte[] offeredCompressionMethods);

		void ProcessClientExtensions(IDictionary clientExtensions);

		ProtocolVersion GetServerVersion();

		int GetSelectedCipherSuite();

		byte GetSelectedCompressionMethod();

		IDictionary GetServerExtensions();

		IList GetServerSupplementalData();

		TlsCredentials GetCredentials();

		CertificateStatus GetCertificateStatus();

		TlsKeyExchange GetKeyExchange();

		CertificateRequest GetCertificateRequest();

		void ProcessClientSupplementalData(IList clientSupplementalData);

		void NotifyClientCertificate(Certificate clientCertificate);

		NewSessionTicket GetNewSessionTicket();
	}
}
