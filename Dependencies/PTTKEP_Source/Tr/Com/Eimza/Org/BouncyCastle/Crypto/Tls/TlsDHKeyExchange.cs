using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsDHKeyExchange : AbstractTlsKeyExchange
	{
		protected TlsSigner mTlsSigner;

		protected DHParameters mDHParameters;

		protected AsymmetricKeyParameter mServerPublicKey;

		protected DHPublicKeyParameters mDHAgreeServerPublicKey;

		protected TlsAgreementCredentials mAgreementCredentials;

		protected DHPrivateKeyParameters mDHAgreeClientPrivateKey;

		protected DHPrivateKeyParameters mDHAgreeServerPrivateKey;

		protected DHPublicKeyParameters mDHAgreeClientPublicKey;

		public override bool RequiresServerKeyExchange
		{
			get
			{
				int num = mKeyExchange;
				if (num == 3 || num == 5 || num == 11)
				{
					return true;
				}
				return false;
			}
		}

		public TlsDHKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, DHParameters dhParameters)
			: base(keyExchange, supportedSignatureAlgorithms)
		{
			switch (keyExchange)
			{
			case 7:
			case 9:
				mTlsSigner = null;
				break;
			case 5:
				mTlsSigner = new TlsRsaSigner();
				break;
			case 3:
				mTlsSigner = new TlsDssSigner();
				break;
			default:
				throw new InvalidOperationException("unsupported key exchange algorithm");
			}
			mDHParameters = dhParameters;
		}

		public override void Init(TlsContext context)
		{
			base.Init(context);
			if (mTlsSigner != null)
			{
				mTlsSigner.Init(context);
			}
		}

		public override void SkipServerCredentials()
		{
			throw new TlsFatalAlert(10);
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
				mServerPublicKey = PublicKeyFactory.CreateKey(subjectPublicKeyInfo);
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(43, alertCause);
			}
			if (mTlsSigner == null)
			{
				try
				{
					mDHAgreeServerPublicKey = TlsDHUtilities.ValidateDHPublicKey((DHPublicKeyParameters)mServerPublicKey);
				}
				catch (InvalidCastException alertCause2)
				{
					throw new TlsFatalAlert(46, alertCause2);
				}
				TlsUtilities.ValidateKeyUsage(certificateAt, 8);
			}
			else
			{
				if (!mTlsSigner.IsValidPublicKey(mServerPublicKey))
				{
					throw new TlsFatalAlert(46);
				}
				TlsUtilities.ValidateKeyUsage(certificateAt, 128);
			}
			base.ProcessServerCertificate(serverCertificate);
		}

		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			byte[] certificateTypes = certificateRequest.CertificateTypes;
			foreach (byte b in certificateTypes)
			{
				if ((uint)(b - 1) > 3u && b != 64)
				{
					throw new TlsFatalAlert(47);
				}
			}
		}

		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			if (clientCredentials is TlsAgreementCredentials)
			{
				mAgreementCredentials = (TlsAgreementCredentials)clientCredentials;
			}
			else if (!(clientCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
		}

		public override void GenerateClientKeyExchange(Stream output)
		{
			if (mAgreementCredentials == null)
			{
				mDHAgreeClientPrivateKey = TlsDHUtilities.GenerateEphemeralClientKeyExchange(context.SecureRandom, mDHAgreeServerPublicKey.Parameters, output);
			}
		}

		public override byte[] GeneratePremasterSecret()
		{
			if (mAgreementCredentials != null)
			{
				return mAgreementCredentials.GenerateAgreement(mDHAgreeServerPublicKey);
			}
			if (mDHAgreeServerPrivateKey != null)
			{
				return TlsDHUtilities.CalculateDHBasicAgreement(mDHAgreeClientPublicKey, mDHAgreeServerPrivateKey);
			}
			if (mDHAgreeClientPrivateKey != null)
			{
				return TlsDHUtilities.CalculateDHBasicAgreement(mDHAgreeServerPublicKey, mDHAgreeClientPrivateKey);
			}
			throw new TlsFatalAlert(80);
		}
	}
}
