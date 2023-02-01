using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace ActiveUp.Net.Security
{
	public class SslHandShake
	{
		private string _hostName;

		private SslProtocols _sslProtocol = SslProtocols.Default;

		private LocalCertificateSelectionCallback _clientCallback;

		private RemoteCertificateValidationCallback _serverCallback;

		private X509CertificateCollection _clientCertificates;

		private bool _checkRevocation;

		public string HostName
		{
			get
			{
				return _hostName;
			}
			set
			{
				_hostName = value;
			}
		}

		public SslProtocols SslProtocol
		{
			get
			{
				return _sslProtocol;
			}
			set
			{
				_sslProtocol = value;
			}
		}

		public LocalCertificateSelectionCallback ClientCertificateSelectionCallback
		{
			get
			{
				return _clientCallback;
			}
			set
			{
				_clientCallback = value;
			}
		}

		public RemoteCertificateValidationCallback ServerCertificateValidationCallback
		{
			get
			{
				return _serverCallback;
			}
			set
			{
				_serverCallback = value;
			}
		}

		public X509CertificateCollection ClientCertificates
		{
			get
			{
				return _clientCertificates;
			}
			set
			{
				_clientCertificates = value;
			}
		}

		public bool CheckRevocation
		{
			get
			{
				return _checkRevocation;
			}
			set
			{
				_checkRevocation = value;
			}
		}

		public SslHandShake(string hostName, SslProtocols sslProtocol, RemoteCertificateValidationCallback serverCallback, LocalCertificateSelectionCallback clientCallback, X509CertificateCollection clientCertificates, bool checkRevocation)
		{
			_hostName = hostName;
			_sslProtocol = sslProtocol;
			_serverCallback = serverCallback;
			_clientCallback = clientCallback;
			_clientCertificates = clientCertificates;
			_checkRevocation = checkRevocation;
		}

		public SslHandShake(string hostName, SslProtocols sslProtocol, RemoteCertificateValidationCallback serverCallback, LocalCertificateSelectionCallback clientCallback, X509CertificateCollection clientCertificates)
			: this(hostName, sslProtocol, serverCallback, clientCallback, clientCertificates, false)
		{
		}

		public SslHandShake(string hostName, SslProtocols sslProtocol, RemoteCertificateValidationCallback serverCallback, LocalCertificateSelectionCallback clientCallback)
			: this(hostName, sslProtocol, serverCallback, clientCallback, null, false)
		{
		}

		public SslHandShake(string hostName, SslProtocols sslProtocol, RemoteCertificateValidationCallback serverCallback)
			: this(hostName, sslProtocol, serverCallback, null, null, false)
		{
		}

		public SslHandShake(string hostName, SslProtocols sslProtocol)
			: this(hostName, sslProtocol, null, null, null, false)
		{
		}

		public SslHandShake(string hostName)
			: this(hostName, SslProtocols.Default, null, null, null, false)
		{
		}
	}
}
