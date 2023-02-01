using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsRsaKeyExchange : AbstractTlsKeyExchange
	{
		protected AsymmetricKeyParameter serverPublicKey;

		protected RsaKeyParameters rsaServerPublicKey;

		protected TlsEncryptionCredentials serverCredentials;

		protected byte[] premasterSecret;

		public TlsRsaKeyExchange(IList supportedSignatureAlgorithms)
			: base(1, supportedSignatureAlgorithms)
		{
		}

		public override void SkipServerCredentials()
		{
			throw new TlsFatalAlert(10);
		}

		public override void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			if (!(serverCredentials is TlsEncryptionCredentials))
			{
				throw new TlsFatalAlert(80);
			}
			ProcessServerCertificate(serverCredentials.Certificate);
			this.serverCredentials = (TlsEncryptionCredentials)serverCredentials;
		}

		public override void ProcessServerCertificate(Certificate serverCertificate)
		{
			if (serverCertificate.IsEmpty)
			{
				throw new TlsFatalAlert(42);
			}
			X509CertificateStructure certificateAt = serverCertificate.GetCertificateAt(0);
			SubjectPublicKeyInfo subjectPublicKeyInfo = certificateAt.SubjectPublicKeyInfo;
			try
			{
				serverPublicKey = PublicKeyFactory.CreateKey(subjectPublicKeyInfo);
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(43, alertCause);
			}
			if (serverPublicKey.IsPrivate)
			{
				throw new TlsFatalAlert(80);
			}
			rsaServerPublicKey = ValidateRsaPublicKey((RsaKeyParameters)serverPublicKey);
			TlsUtilities.ValidateKeyUsage(certificateAt, 32);
			base.ProcessServerCertificate(serverCertificate);
		}

		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			byte[] certificateTypes = certificateRequest.CertificateTypes;
			foreach (byte b in certificateTypes)
			{
				if ((uint)(b - 1) > 1u && b != 64)
				{
					throw new TlsFatalAlert(47);
				}
			}
		}

		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			if (!(clientCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
		}

		public override void GenerateClientKeyExchange(Stream output)
		{
			premasterSecret = TlsRsaUtilities.GenerateEncryptedPreMasterSecret(context, rsaServerPublicKey, output);
		}

		public override void ProcessClientKeyExchange(Stream input)
		{
			byte[] encryptedPreMasterSecret = ((!TlsUtilities.IsSsl(context)) ? TlsUtilities.ReadOpaque16(input) : Streams.ReadAll(input));
			premasterSecret = serverCredentials.DecryptPreMasterSecret(encryptedPreMasterSecret);
		}

		public override byte[] GeneratePremasterSecret()
		{
			if (premasterSecret == null)
			{
				throw new TlsFatalAlert(80);
			}
			byte[] result = premasterSecret;
			premasterSecret = null;
			return result;
		}

		protected virtual RsaKeyParameters ValidateRsaPublicKey(RsaKeyParameters key)
		{
			if (!key.Exponent.IsProbablePrime(2))
			{
				throw new TlsFatalAlert(47);
			}
			return key;
		}
	}
}
