using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal abstract class AbstractTlsSignerCredentials : AbstractTlsCredentials, TlsSignerCredentials, TlsCredentials
	{
		public virtual SignatureAndHashAlgorithm SignatureAndHashAlgorithm
		{
			get
			{
				throw new InvalidOperationException("TlsSignerCredentials implementation does not support (D)TLS 1.2+");
			}
		}

		public abstract byte[] GenerateCertificateSignature(byte[] hash);
	}
}
