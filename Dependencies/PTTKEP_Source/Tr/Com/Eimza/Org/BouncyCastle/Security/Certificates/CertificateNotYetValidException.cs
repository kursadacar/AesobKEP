using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates
{
	[Serializable]
	internal class CertificateNotYetValidException : CertificateException
	{
		public CertificateNotYetValidException()
		{
		}

		public CertificateNotYetValidException(string message)
			: base(message)
		{
		}

		public CertificateNotYetValidException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}
