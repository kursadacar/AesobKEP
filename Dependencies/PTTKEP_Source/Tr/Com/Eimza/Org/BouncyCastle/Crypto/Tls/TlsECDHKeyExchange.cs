using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsECDHKeyExchange : AbstractTlsKeyExchange
	{
		protected TlsSigner mTlsSigner;

		protected int[] mNamedCurves;

		protected byte[] mClientECPointFormats;

		protected byte[] mServerECPointFormats;

		protected AsymmetricKeyParameter mServerPublicKey;

		protected TlsAgreementCredentials mAgreementCredentials;

		protected ECPrivateKeyParameters mECAgreePrivateKey;

		protected ECPublicKeyParameters mECAgreePublicKey;

		public override bool RequiresServerKeyExchange
		{
			get
			{
				int num = mKeyExchange;
				if (num == 17 || (uint)(num - 19) <= 1u)
				{
					return true;
				}
				return false;
			}
		}

		public TlsECDHKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, int[] namedCurves, byte[] clientECPointFormats, byte[] serverECPointFormats)
			: base(keyExchange, supportedSignatureAlgorithms)
		{
			switch (keyExchange)
			{
			case 19:
				mTlsSigner = new TlsRsaSigner();
				break;
			case 17:
				mTlsSigner = new TlsECDsaSigner();
				break;
			case 16:
			case 18:
				mTlsSigner = null;
				break;
			default:
				throw new InvalidOperationException("unsupported key exchange algorithm");
			}
			mNamedCurves = namedCurves;
			mClientECPointFormats = clientECPointFormats;
			mServerECPointFormats = serverECPointFormats;
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
					mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey((ECPublicKeyParameters)mServerPublicKey);
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
				if ((uint)(b - 1) > 1u && (uint)(b - 64) > 2u)
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
				mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralClientKeyExchange(context.SecureRandom, mServerECPointFormats, mECAgreePublicKey.Parameters, output);
			}
		}

		public override void ProcessClientCertificate(Certificate clientCertificate)
		{
		}

		public override void ProcessClientKeyExchange(Stream input)
		{
			if (mECAgreePublicKey == null)
			{
				byte[] encoding = TlsUtilities.ReadOpaque8(input);
				ECDomainParameters parameters = mECAgreePrivateKey.Parameters;
				mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(mServerECPointFormats, parameters, encoding));
			}
		}

		public override byte[] GeneratePremasterSecret()
		{
			if (mAgreementCredentials != null)
			{
				return mAgreementCredentials.GenerateAgreement(mECAgreePublicKey);
			}
			if (mECAgreePrivateKey != null)
			{
				return TlsEccUtilities.CalculateECDHBasicAgreement(mECAgreePublicKey, mECAgreePrivateKey);
			}
			throw new TlsFatalAlert(80);
		}
	}
}
