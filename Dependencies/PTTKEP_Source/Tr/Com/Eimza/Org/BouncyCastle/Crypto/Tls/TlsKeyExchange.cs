using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal interface TlsKeyExchange
	{
		bool RequiresServerKeyExchange { get; }

		void Init(TlsContext context);

		void SkipServerCredentials();

		void ProcessServerCredentials(TlsCredentials serverCredentials);

		void ProcessServerCertificate(Certificate serverCertificate);

		byte[] GenerateServerKeyExchange();

		void SkipServerKeyExchange();

		void ProcessServerKeyExchange(Stream input);

		void ValidateCertificateRequest(CertificateRequest certificateRequest);

		void SkipClientCredentials();

		void ProcessClientCredentials(TlsCredentials clientCredentials);

		void ProcessClientCertificate(Certificate clientCertificate);

		void GenerateClientKeyExchange(Stream output);

		void ProcessClientKeyExchange(Stream input);

		byte[] GeneratePremasterSecret();
	}
}
