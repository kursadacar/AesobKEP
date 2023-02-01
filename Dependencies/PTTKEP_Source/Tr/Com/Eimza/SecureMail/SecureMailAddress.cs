using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Tr.Com.Eimza.EYazisma;
using Tr.Com.Eimza.SmartCard;

namespace Tr.Com.Eimza.SecureMail
{
	internal class SecureMailAddress
	{
		public string Address
		{
			get
			{
				return InternalMailAddress.Address;
			}
		}

		public string DisplayName
		{
			get
			{
				return InternalMailAddress.DisplayName;
			}
		}

		public string Host
		{
			get
			{
				return InternalMailAddress.Host;
			}
		}

		public string User
		{
			get
			{
				return InternalMailAddress.User;
			}
		}

		public X509Certificate2 SigningCertificate { get; private set; }

		public SmartCardReader Reader { get; private set; }

		public OzetAlg? OzetAlg { get; private set; }

		internal MailAddress InternalMailAddress { get; private set; }

		public SecureMailAddress(string address)
		{
			InternalMailAddress = new MailAddress(address);
		}

		public SecureMailAddress(string address, string displayName)
		{
			InternalMailAddress = new MailAddress(address, displayName);
		}

		public SecureMailAddress(string address, string displayName, X509Certificate2 signingCert)
			: this(address, displayName)
		{
			if (signingCert != null && !signingCert.HasPrivateKey)
			{
				throw new CryptographicException("The specified signing certificate doesn't contain a private key.");
			}
			SigningCertificate = signingCert;
		}

		public SecureMailAddress(string address, SmartCardReader reader)
			: this(address)
		{
			if (reader.SmartCardParams.SmartCard.Pkcs11Module == null)
			{
				throw new CryptographicException("The specified smartcard reader doesn't contain a Pkcs11Module.");
			}
			Reader = reader;
			OzetAlg = null;
		}

		public SecureMailAddress(string address, SmartCardReader reader, OzetAlg ozetAlg)
			: this(address)
		{
			if (reader.SmartCardParams.SmartCard.Pkcs11Module == null)
			{
				throw new CryptographicException("The specified smartcard reader doesn't contain a Pkcs11Module.");
			}
			Reader = reader;
			OzetAlg = ozetAlg;
		}

		public SecureMailAddress(string address, string displayName, SmartCardReader reader)
			: this(address, displayName)
		{
			if (reader.SmartCardParams.SmartCard.Pkcs11Module == null)
			{
				throw new CryptographicException("The specified smartcard reader doesn't contain a Pkcs11Module.");
			}
			Reader = reader;
			OzetAlg = null;
		}

		public SecureMailAddress(string address, string displayName, SmartCardReader reader, OzetAlg ozetAlg)
			: this(address, displayName)
		{
			if (reader.SmartCardParams.SmartCard.Pkcs11Module == null)
			{
				throw new CryptographicException("The specified smartcard reader doesn't contain a Pkcs11Module.");
			}
			Reader = reader;
			OzetAlg = ozetAlg;
		}
	}
}
